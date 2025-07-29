using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DoctorController(IScheduleService scheduleService, IUserService userService, IDoctorService doctorService) : ControllerBase
	{
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var response = await doctorService.GetAllDoctorsAsync();
			return response.IsSuccess ? Ok(response) : StatusCode(500, response);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var response = await doctorService.GetDoctorByIdAsync(id);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> CreateDoctor([FromBody] AddDoctorRequestDto dto)
		{
			var result = await userService.AddDoctorAsync(dto);
			return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result) : BadRequest(result);
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("schedules")]
		public async Task<IActionResult> GetSchedulesByDoctorId()
		{
			var doctorId = GetGuidClaim("DoctorId");
			var response = await scheduleService.GetSchedulesByDoctorIdAsync(doctorId);

			return response.IsSuccess ? Ok(response) : Ok(response.Message);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{availability}")]
		public async Task<IActionResult> GetByAvailability(string availability)
		{
			var response = await doctorService.GetByAvailability(availability);
			return response.IsSuccess ? Ok(response) : Ok(response.Message);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{Specialty}")]
		public async Task<IActionResult> GetBySpecialty(string Specialty)
		{
			var doctorId = GetGuidClaim("DoctorId");
			var doc = await doctorService.GetDoctorByIdAsync(doctorId);

			if (!doc.IsSuccess || doc.Data == null)
				return Ok(doc.Message);

			var response = await doctorService.GetBySpecialtyAsync(Specialty);
			return response.IsSuccess ? Ok(response) : Ok(response.Message);
		}

		[Authorize(Roles = "Doctor")]
		[HttpPut("update-profile")]
		public async Task<IActionResult> UpdateProfile([FromBody] UpdateDoctorDTO doctorDto)
		{
			var doctorId = GetGuidClaim("DoctorId");
			var response = await doctorService.UpdateDoctorAsync(doctorId, doctorDto);
			return response.IsSuccess ? Ok(response) : Ok(response.Message);
		}

		[Authorize(Roles = "Doctor")]
		[HttpGet("schedules/{id}")]
		public async Task<IActionResult> GetScheduleById(Guid id)
		{
			var response = await scheduleService.GetScheduleByIdAsync(id);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response.Data);
		}

		[Authorize(Roles = "Doctor")]
		[HttpPost("schedule")]
		public async Task<IActionResult> CreateSchedule([FromBody] createScheduleRequestModel scheduleDto)
		{
			var doctorId = GetGuidClaim("DoctorId");
			if (scheduleDto == null)
			{
				return Ok("Schedule data is required.");
			}

			var response = await scheduleService.CreateScheduleAsync(scheduleDto, doctorId);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return CreatedAtAction(nameof(GetScheduleById), new { id = response.Data.Id }, response.Data);
		}

		[Authorize(Roles = "Doctor")]
		[HttpPut("schedules/{id}")]
		public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] createScheduleRequestModel scheduleDto)
		{
			if (scheduleDto == null)
			{
				return Ok("Schedule data is required.");
			}

			var response = await scheduleService.UpdateScheduleAsync(id, scheduleDto);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response);
		}

		[Authorize(Roles = "Doctor")]
		[HttpDelete("schedules/{id}")]
		public async Task<IActionResult> DeleteSchedule(Guid id)
		{
			var response = await scheduleService.DeleteScheduleAsync(id);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response.Message);
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await doctorService.DeleteDoctorAsync(id);
			return response.IsSuccess ? Ok(response.Message) : Ok(response.Message);
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
