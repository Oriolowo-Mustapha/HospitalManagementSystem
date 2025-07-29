using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientController(IPatientService patientService) : ControllerBase
	{
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllPatients()
		{
			var response = await patientService.GetAllPatientsAsync();
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetPatientById(Guid id)
		{
			var response = await patientService.GetPatientByIdAsync(id);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("by-email")]
		public async Task<IActionResult> GetPatientByEmail([FromQuery] string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return Ok("Email is required.");
			}

			var response = await patientService.GetPatientByEmailAsync(email);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}

			return Ok(response);
		}
		[Authorize(Roles = "Admin")]
		[HttpGet("search")]
		public async Task<IActionResult> SearchPatients([FromQuery] string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return Ok("Name is required.");

			var response = await patientService.SearchPatientsByNameAsync(name);

			if (!response.IsSuccess)
				return Ok(response.Message);

			return Ok(response);
		}


		[Authorize(Roles = "Patient")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientRequestModel patientDto)
		{
			try
			{
				var response = await patientService.UpdatePatientAsync(id, patientDto);
				if (!response.IsSuccess)
				{
					return Ok(response.Message);
				}
				return Ok(response);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePatient(Guid id)
		{
			var response = await patientService.DeletePatientAsync(id);
			if (!response.IsSuccess)
			{
				return Ok(response.Message);
			}
			return Ok(response);
		}
	}
}