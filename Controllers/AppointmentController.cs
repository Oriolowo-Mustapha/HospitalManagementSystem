using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HospitalManagementSystem.Controllers
{
	[Authorize(Roles = "Doctor , Patient")]
	[Route("api/[controller]")]
	[ApiController]
	public class AppointmentController : ControllerBase
	{
		private readonly IAppointmentService _service;

		public AppointmentController(IAppointmentService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] AppointmentRequestDto dto)
		{
			if (dto == null)
			{
				return BadRequest("Appointment data is required.");
			}

			var result = await _service.CreateAppointmentAsync(dto);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return CreatedAtAction(nameof(Get), new { id = result.Data.Id }, result.Data);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _service.GetAllAppointmentsAsync();
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var result = await _service.GetAppointmentByIdAsync(id);
			if (!result.IsSuccess)
			{
				return NotFound(result.Message);
			}
			return Ok(result.Data);
		}

		[HttpGet("patient/{patientId}")]
		public async Task<IActionResult> GetByPatient(Guid patientId)
		{
			var result = await _service.GetAppointmentsByPatientIdAsync(patientId);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Data);
		}

		[HttpGet("doctor/{doctorId}")]
		public async Task<IActionResult> GetByDoctor(Guid doctorId)
		{
			var result = await _service.GetAppointmentsByDoctorIdAsync(doctorId);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Data);
		}

		[HttpPut("{id}/reschedule")]
		public async Task<IActionResult> Reschedule(Guid id, [FromBody] AppointmentUpdateDto dto)
		{
			if (dto == null)
			{
				return BadRequest("Appointment update data is required.");
			}

			var result = await _service.RescheduleAppointmentAsync(id, dto);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Data);
		}

		[HttpDelete("{id}/cancel")]
		public async Task<IActionResult> Cancel(Guid id)
		{
			var result = await _service.CancelAppointmentAsync(id);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Message);
		}

		[HttpPut("{id}/approve")]
		public async Task<IActionResult> Approve(Guid id)
		{
			var result = await _service.ApproveAppointmentAsync(id);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Message);
		}

		[HttpPut("{id}/disapprove")]
		public async Task<IActionResult> Disapprove(Guid id)
		{
			var result = await _service.DisapproveAppointmentAsync(id);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Message);
		}

		[HttpPut("{id}/notes")]
		public async Task<IActionResult> UpdateNotes(Guid id, [FromBody] UploadAppointmentNoteRequestDto dto)
		{
			if (dto == null)
			{
				return BadRequest("Note data is required.");
			}

			var result = await _service.UpdateAppointmentNote(id, dto);
			if (!result.IsSuccess)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Message);
		}
	}
}