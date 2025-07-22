using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScheduleController : ControllerBase
	{
		private readonly IScheduleService _scheduleService;

		public ScheduleController(IScheduleService scheduleService)
		{
			_scheduleService = scheduleService;
		}

		[HttpGet("doctors/{doctorId}/schedules")]
		public async Task<IActionResult> GetSchedulesByDoctorId(Guid doctorId)
		{
			var response = await _scheduleService.GetSchedulesByDoctorIdAsync(doctorId);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpGet("schedules/{id}")]
		public async Task<IActionResult> GetScheduleById(Guid id)
		{
			var response = await _scheduleService.GetScheduleByIdAsync(id);
			if (!response.IsSuccess)
			{
				return NotFound(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpPost("doctors/{doctorId}/schedules")]
		public async Task<IActionResult> CreateSchedule(Guid doctorId, [FromBody] ScheduleDTO scheduleDto)
		{
			if (scheduleDto == null)
			{
				return BadRequest("Schedule data is required.");
			}

			var response = await _scheduleService.CreateScheduleAsync(scheduleDto, doctorId);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return CreatedAtAction(nameof(GetScheduleById), new { id = response.Data.Id }, response.Data);
		}

		[HttpPut("schedules/{id}")]
		public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] ScheduleDTO scheduleDto)
		{
			if (scheduleDto == null)
			{
				return BadRequest("Schedule data is required.");
			}

			var response = await _scheduleService.UpdateScheduleAsync(id, scheduleDto);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpDelete("schedules/{id}")]
		public async Task<IActionResult> DeleteSchedule(Guid id)
		{
			var response = await _scheduleService.DeleteScheduleAsync(id);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return NoContent();
		}

		[HttpPost("schedules/validate")]
		public async Task<IActionResult> ValidateSchedule(Guid doctorId, [FromBody] ScheduleDTO scheduleDto)
		{
			if (scheduleDto == null)
			{
				return BadRequest("Schedule data is required.");
			}

			var response = await _scheduleService.ValidateScheduleAsync(doctorId, scheduleDto);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}
	}
}
