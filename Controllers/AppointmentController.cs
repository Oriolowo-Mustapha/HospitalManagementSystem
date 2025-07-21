using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
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
			var result = await _service.CreateAppointmentAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _service.GetAllAppointmentsAsync());
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var result = await _service.GetAppointmentByIdAsync(id);
			return result.IsSuccess ? Ok(result) : NotFound(result);
		}

		[HttpGet("patient/{patientId}")]
		public async Task<IActionResult> GetByPatient(Guid patientId)
		{
			return Ok(await _service.GetAppointmentsByPatientIdAsync(patientId));
		}


		[HttpGet("doctor/{doctorId}")]
		public async Task<IActionResult> GetByDoctor(Guid doctorId)
		{
			return Ok(await _service.GetAppointmentsByDoctorIdAsync(doctorId));
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentUpdateDto dto)
		{
			var result = await _service.RescheduleAppointmentAsync(id, dto);
			return result.IsSuccess ? Ok(result) : NotFound(result);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var result = await _service.DeleteAppointmentAsync(id);
			return result.IsSuccess ? Ok(result) : NotFound(result);
		}
	}

}
