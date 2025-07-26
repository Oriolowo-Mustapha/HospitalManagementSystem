using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DoctorController(IScheduleService _scheduleService, IUserService _userService, IDoctorService _doctorService) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var response = await _doctorService.GetAllDoctorsAsync();
			return response.IsSuccess ? Ok(response) : StatusCode(500, response);
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var response = await _doctorService.GetDoctorByIdAsync(id);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateDoctor([FromBody] AddDoctorRequestDto dto)
		{
			var result = await _userService.AddDoctorAsync(dto);
			return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result) : BadRequest(result);
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("schedules")]
		public async Task<IActionResult> GetSchedulesByDoctorId()
		{
			var doctorId = GetGuidClaim("DoctorId");
			var response = await _scheduleService.GetSchedulesByDoctorIdAsync(doctorId);

			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpGet("availability/{availability}")]
		public async Task<IActionResult> GetByAvailability(string availability)
		{
			var response = await _doctorService.GetByAvailability(availability);
			return response.IsSuccess ? Ok(response) : Ok(response.Message);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("by-specialty")]
		public async Task<IActionResult> GetBySpecialty(string Specialty)
		{
			var doctorId = GetGuidClaim("DoctorId");
			var doc = await _doctorService.GetDoctorByIdAsync(doctorId);

			if (!doc.IsSuccess || doc.Data == null)
				return Ok(doc.Message);

			var response = await _doctorService.GetBySpecialtyAsync(Specialty);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[Authorize(Roles = "Doctor")]
		[HttpPut("update-profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorDTO doctorDto)
		{
			var doctorId = GetGuidClaim("DoctorId");
			var response = await _doctorService.UpdateDoctorAsync(doctorId, doctorDto);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await _doctorService.DeleteDoctorAsync(id);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		private string? GetClaimValue(string claimType)
		{
			return User.FindFirst(claimType)?.Value;
		}

		private Guid GetGuidClaim(string claimType)
		{
			var value = GetClaimValue(claimType);
			return Guid.TryParse(value, out var guid) ? guid : Guid.Empty;
		}
	}
}
