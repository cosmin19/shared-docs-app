﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;

namespace ASP.NET_Core_Basic_Web_API.Infrastructure.FeatureFolders
{
    public class FeatureFoldersRazorViewEngine : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
              IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
            {
                throw new NullReferenceException("ControllerActionDescriptor cannot be null.");
            }

            string featureName = controllerActionDescriptor.Properties["feature"] as string;
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context) { }
    }
}
