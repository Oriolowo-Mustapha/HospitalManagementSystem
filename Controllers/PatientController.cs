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

        [HttpPost]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientRequestDto requestDto)
        {
            try
            {
                var patient = await _patientService.RegisterPatientAsync(requestDto);
                return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetPatientByEmail(string email)
        {
            var patient = await _patientService.GetPatientByEmailAsync(email);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] PatientDTO patientDto)
        {
            try
            {
                await _patientService.UpdatePatientAsync(id, patientDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}