using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineStore.Abstract
{
    public interface IIdentityWorker //TODO Separate
    {
        Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password);


        Task<ClaimsIdentity> GetClaimsIdentity(string refreshToken);


        string GenerateJwtToken(ClaimsIdentity claims);


        string GenerateRefreshToken();

    }
}