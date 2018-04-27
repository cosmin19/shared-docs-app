using System;
using System.Net;
using System.Text;
using Enviroself.Context;
using Enviroself.Infrastructure.DependencyManagement;
using Enviroself.Infrastructure.Jwt.Entities;
using Enviroself.Infrastructure.FeatureFolders;
using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Enviroself
{
    public class Startup
    {
        private const string SecretKey = "ProiectIDP_SharedFiles"; // todo: get this from somewhere secure
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Database
            services.AddDbContext<DbApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Azure")));
            #endregion

            #region MvcAndFeatureFolders
            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
            .AddRazorOptions(options =>
            {
                // {0} - Action Name
                // {1} - Controller Name
                // {2} - Area Name
                // {3} - Feature Name
                // Replace normal view location entirely

                options.ViewLocationFormats.Clear();

                options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
                options.ViewLocationFormats.Insert(0, "/Features/{3}/{0}.cshtml");
                options.ViewLocationFormats.Insert(0, "/Features/{3}/{1}/{0}.cshtml");

                options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{0}.cshtml");
                options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{1}/{0}.cshtml");

                options.ViewLocationExpanders.Add(new FeatureFoldersRazorViewEngine());
            });
            #endregion

            #region Security
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                RequireExpirationTime = false,
                ValidateLifetime = true, //validate the expiration and not before values in the token
                ClockSkew = TimeSpan.FromMinutes(0) //5 minute tolerance for the expiration date
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.RequireHttpsMetadata = false; // Check this out
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";

                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
            #endregion

            #region Cors
            services.AddCors(options => options.AddPolicy("Cors", bld =>
            {
                bld
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            #endregion

            #region ServicesAndRepositories
            // Register all service interface automatic 
            ContainerBuilder builder = new ContainerBuilder();
            IContainer container = builder.Register_Automatic(services);
            #endregion

            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbApplicationContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
            });

            app.UseStaticFiles();
            app.UseCors("Cors");
            app.UseAuthentication();

            dbContext.Database.EnsureCreated();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Admin",
                    template: "{area:exists}/{controller=Home}/{action=Get}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Get}/{id?}");
            });

            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Page not found");
            });
        }
    }
}
