using Entities.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BlazorProducts.Client.HttpRepository;

public interface IUsersHttpRepository
{
    Task<List<UserLite>> GetUsers();
    Task<HttpStatusCode> DeleteUser(UserLite user);
    Task<HttpStatusCode> UpdatePriviledgeOfUser(UserLite user);
}