using Entities;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingApp2Server.Services;
using Repository.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingApp2Server.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRepositoryManager _repository;

    public AccountController(UserManager<User> userManager,
        IAuthenticationService authenticationService,
        IRepositoryManager repository)
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
        _repository = repository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
    {
        if (userForRegistrationDto == null || !ModelState.IsValid)
            return BadRequest();

        var user = new User
        {
            UserName = userForRegistrationDto.Email,
            Email = userForRegistrationDto.Email
        };

        var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new ResponseDto {Errors = errors});
        }

        await _userManager.AddToRoleAsync(user, "Viewer");

        var tenant = new Tenant {TenantId = user.Email};
        _repository.Tenant.CreateTenant(tenant);
        await _repository.SaveAsync();

        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthenticationDto)
    {
        var user = await _userManager.FindByNameAsync(userForAuthenticationDto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user,
                userForAuthenticationDto.Password))
        {
            return Unauthorized(new AuthResponseDto
            {
                ErrorMessage = "Invalid Authentication"
            });
        }

        var token = await _authenticationService.GetToken(user);

        user.RefreshToken = _authenticationService.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Ok(new AuthResponseDto
        {
            IsAuthSuccessful = true,
            Token = token,
            RefreshToken = user.RefreshToken
        });
    }
}