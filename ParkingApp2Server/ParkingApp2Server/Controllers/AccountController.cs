using AutoMapper;
using Contracts;
using Entities;
using Entities.Configuration;
using Entities.DTO;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParkingApp2Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingApp2Server.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IAuthenticationService _authenticationService;
		private readonly IMapper _mapper;
		private readonly PriviledgedUsersConfiguration _priviledgedUsersSettings;
		private readonly IRepositoryManager _repository;

		public AccountController(UserManager<User> userManager,
			IAuthenticationService authenticationService,
			IMapper mapper,
			IOptions<PriviledgedUsersConfiguration> priviledgedUsersSettings,
			IRepositoryManager repository)
		{
			_userManager = userManager;
			_authenticationService = authenticationService;
			_mapper = mapper;
			_priviledgedUsersSettings = priviledgedUsersSettings.Value;
			_repository = repository;
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser(
			 [FromBody] UserForRegistrationDto userForRegistrationDto)
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
				return BadRequest(new ResponseDto { Errors = errors });
			}

			await _userManager.AddToRoleAsync(user, "Viewer");

			// todo create tenant also
			var tenant = new Tenant { TenantId = user.Email};
			_repository.Tenant.CreateTenant(tenant);
			await _repository.SaveAsync();

			return StatusCode(201);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(
			[FromBody] UserForAuthenticationDto userForAuthenticationDto)
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
			
			//await _userManager.AddToRoleAsync(user, "Administrator");

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

		[HttpGet("Users")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> GetUsers()
		{
			var allUsers = _userManager.Users.ToList();			

			var userLite = _mapper.Map<IEnumerable<UserLite>>(allUsers);

			return Ok(userLite);
		}

		[HttpPost("DeleteUser")]
		[Authorize(Roles = "Administrator")]
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

			foreach (var login in logins.ToList())
			{
				await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
			}

			if (rolesForUser.Count > 0)
			{
				foreach (var item in rolesForUser.ToList())
				{
					await _userManager.RemoveFromRoleAsync(user, item);
				}
			}

			//Delete User
			await _userManager.DeleteAsync(user);

			return Ok();
		}

		[HttpPost("UpdatePriviledgeOfUser")]
		[Authorize(Roles = "Administrator")]
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
				var allUsers = _userManager.Users.ToList();
				var priviledgedUsersCount = allUsers.Where(w => w.Priviledged == true)
						.ToList()
						.Count;
				if (priviledgedUsersCount >= _priviledgedUsersSettings.MaxCount)
					return BadRequest();
			}			

			user.Priviledged = userLite.Priviledged;
			await _userManager.UpdateAsync(user);

			return NoContent();
		}
	}
}
