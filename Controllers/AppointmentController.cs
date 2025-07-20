using HospitalManagementSystem.Contract.Services;
using HospitalManagementSystem.DTOs;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Create([FromBody] AppointmentDTO dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           return Ok(await _service.GetAllAsync());
        }
          

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(Guid patientId){
            return Ok(await _service.GetByPatientIdAsync(patientId));
        }
           

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(Guid doctorId) {
          return  Ok(await _service.GetByDoctorIdAsync(doctorId));
        }
           

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }

}
