using Enviroself.Features.Account.Entities;
using System.Threading.Tasks;

namespace Enviroself.Services.Auth
{
    public interface IIdentityService
    {
        Task<User> GetCurrentPersonIdentityAsync();
    }
}
