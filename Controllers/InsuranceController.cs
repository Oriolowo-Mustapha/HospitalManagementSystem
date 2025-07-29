using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InsuranceController(IInsuranceService insuranceService) : ControllerBase
	{

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AddInsurance([FromBody] AddInsuranceDto dto)
		{
			var result = await insuranceService.AddInsuranceAsync(dto);
			return result.IsSuccess ? Ok(result) : Ok(result.Message);
		}

		[Authorize]
		[HttpGet]

		public async Task<IActionResult> GetAll()
		{
			var result = await insuranceService.GetAllAsync();
			return Ok(result);
		}

		[Authorize]
		[HttpGet("{name}")]
		public async Task<IActionResult> GetByName(string name)
		{
			var result = await insuranceService.GetByNameAsync(name);
			return result.IsSuccess ? Ok(result) : Ok(result.Message);
		}
	}
}
