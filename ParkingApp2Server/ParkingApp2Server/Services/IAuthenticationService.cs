using Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkingApp2Server.Services;

public interface IAuthenticationService
{
    Task<string> GetToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}