using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IPatientService _patientService;

		public PatientController(IPatientService patientService)
		{
			_patientService = patientService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPatients()
		{
			var response = await _patientService.GetAllPatientsAsync();
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPatientById(Guid id)
		{
			var response = await _patientService.GetPatientByIdAsync(id);
			if (!response.IsSuccess)
			{
				return NotFound(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpGet("email/{email}")]
		public async Task<IActionResult> GetPatientByEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return BadRequest("Email is required.");
			}

			var response = await _patientService.GetPatientByEmailAsync(email);
			if (!response.IsSuccess)
			{
				return NotFound(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientRequestModel patientDto)
		{
			if (patientDto == null)
			{
				return BadRequest("Patient data is required.");
			}

			var response = await _patientService.UpdatePatientAsync(id, patientDto);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePatient(Guid id)
		{
			var response = await _patientService.DeletePatientAsync(id);
			if (!response.IsSuccess)
			{
				return BadRequest(response.Message);
			}
			return Ok(response.Data);
		}
	}
}