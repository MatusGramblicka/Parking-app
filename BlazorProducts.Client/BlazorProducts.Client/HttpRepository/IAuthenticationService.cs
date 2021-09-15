using Entities.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository
{
    public interface IAuthenticationService
	{
		Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto);
		Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
		Task Logout();
		Task<string> RefreshToken();

		Task<List<UserLite>> GetUsers();	
		Task<HttpStatusCode> DeleteUser(UserLite user);
		Task<HttpStatusCode> UpdatePriviledgeOfUser(UserLite user);		
	}
}
