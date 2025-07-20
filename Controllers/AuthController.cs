using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _authService;

		public AuthController(IUserService authService)
		{
			_authService = authService;
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterPatientRequestDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.SignUpAsync(model);
			if (!result.IsSuccess)
				return BadRequest(new { message = result.Message });

			return Ok(result.Data);
		}

		/// <summary>
		/// Logs in an existing user
		/// </summary>
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.LoginAsync(model);
			if (!result.IsSuccess)
				return Unauthorized(new { message = result.Message });

			return Ok(result.Data);
		}
	}
}
