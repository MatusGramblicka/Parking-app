using Entities.DTO;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IAuthenticationService
	{
		Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto);
		Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
		Task Logout();
		Task<string> RefreshToken();	
	}
}
