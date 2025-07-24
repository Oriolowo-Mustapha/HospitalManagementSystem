using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Enum;
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

		[HttpGet("{id}")]
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
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("Schedules")]
		public async Task<IActionResult> GetSchedulesByDoctorId()
		{
			var doctorId = GetGuidClaim("DoctorId");
			var response = await _scheduleService.GetSchedulesByDoctorIdAsync(doctorId);

			if (!response.IsSuccess)
			{
				return NotFound(new { message = response.Message });
			}

			return Ok(response);
		}

		[HttpGet("availability/{availability}")]
		public async Task<IActionResult> GetByAvailability(DoctorAvailability availability)
		{
			var response = await _doctorService.GetByAvailability(availability);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpGet("{specialty}")]
		public async Task<IActionResult> GetBySpecialty()
		{
			var doctorId = GetGuidClaim("DoctorId");
			var doc = await _doctorService.GetDoctorByIdAsync(doctorId);
			var response = await _doctorService.GetBySpecialtyAsync(doc.Data.Specialty);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpPut("update-profile")]
		public async Task<IActionResult> Update(Guid id, [FromBody] DoctorDTO doctorDto)
		{
			var response = await _doctorService.UpdateDoctorAsync(id, doctorDto);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpDelete("{id}")]
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
