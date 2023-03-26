using AutoMapper;
using Entities;
using Entities.Configuration;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParkingApp2Server.Controllers;

[Route("api/account")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly PriviledgedUsersConfiguration _priviledgedUsersSettings;

    public UsersController(UserManager<User> userManager,
        IMapper mapper,
        IOptions<PriviledgedUsersConfiguration> priviledgedUsersSettings)
    {
        _userManager = userManager;
        _mapper = mapper;
        _priviledgedUsersSettings = priviledgedUsersSettings.Value;
    }

    [HttpGet("Users")]
    public IActionResult GetUsers()
    {
        var allUsers = _userManager.Users.AsNoTracking();

        var userLite = _mapper.Map<IEnumerable<UserLite>>(allUsers);

        return Ok(userLite);
    }

    [HttpPost("DeleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody] UserLite userForDeletion)
    {
        var user = await _userManager.FindByNameAsync(userForDeletion.Email);

        if (user == null)
        {
            return Unauthorized(new AuthResponseDto
            {
                ErrorMessage = "Invalid Request"
            });
        }

        var logins = await _userManager.GetLoginsAsync(user);

        var rolesForUser = await _userManager.GetRolesAsync(user);

        foreach (var login in logins)
        {
            await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
        }

        if (rolesForUser.Count > 0)
        {
            foreach (var item in rolesForUser)
            {
                await _userManager.RemoveFromRoleAsync(user, item);
            }
        }

        await _userManager.DeleteAsync(user);

        return Ok();
    }

    [HttpPost("UpdatePriviledgeOfUser")]
    public async Task<IActionResult> UpdatePriviledgeOfUser([FromBody] UserLite userLite)
    {
        var user = await _userManager.FindByNameAsync(userLite.Email);

        if (user == null)
        {
            return BadRequest();
        }

        if (userLite.Priviledged)
        {
            // How many users are priviledged ones?
            var allUsers = _userManager.Users.AsNoTracking();
            var priviledgedUsersCount = allUsers.Where(w => w.Priviledged).ToList().Count;
            if (priviledgedUsersCount >= _priviledgedUsersSettings.MaxCount)
            {
                return BadRequest();
            }
        }

        user.Priviledged = userLite.Priviledged;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}