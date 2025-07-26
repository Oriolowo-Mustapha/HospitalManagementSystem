using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InsuranceController : ControllerBase
	{
		private readonly IInsuranceService _insuranceService;

		public InsuranceController(IInsuranceService insuranceService)
		{
			_insuranceService = insuranceService;
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AddInsurance([FromBody] AddInsuranceDto dto)
		{
			var result = await _insuranceService.AddInsuranceAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _insuranceService.GetAllAsync();
			return Ok(result);
		}

		[HttpGet("{name}")]
		public async Task<IActionResult> GetByName(string name)
		{
			var result = await _insuranceService.GetByNameAsync(name);
			return result.IsSuccess ? Ok(result) : NotFound(result);
		}
	}
}
