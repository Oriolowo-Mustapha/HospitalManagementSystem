using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateBill([FromBody] BillingDto dto)
        {
            var result = await _billingService.CreateBillForAppointmentAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointment(Guid appointmentId)
        {
            var result = await _billingService.GetBillByAppointmentIdAsync(appointmentId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _billingService.GetAllBillsAsync();
            return Ok(result);
        }

        [HttpPut("{billId}/pay")]
        public async Task<IActionResult> Pay(Guid billId)
        {
            var result = await _billingService.MarkBillAsPaidAsync(billId);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }

}
