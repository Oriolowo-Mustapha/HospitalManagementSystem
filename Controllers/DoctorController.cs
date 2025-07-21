using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DoctorController : ControllerBase
	{
		private readonly IDoctorService _doctorService;
		private readonly IUserService _userService;

		public DoctorController(IDoctorService doctorService, IUserService userService)
		{
			_doctorService = doctorService;
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var response = await _doctorService.GetAllDoctorsAsync();
			return response.IsSuccess ? Ok(response) : StatusCode(500, response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			var response = await _doctorService.GetDoctorByIdAsync(id);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateDoctor([FromBody] AddDoctorRequestDto dto)
		{
			var result = await _userService.AddDoctorAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}


		[HttpGet("availability/{availability}")]
		public async Task<IActionResult> GetByAvailability(DoctorAvailability availability)
		{
			var response = await _doctorService.GetByAvailability(availability);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpGet("specialty/{specialty}")]
		public async Task<IActionResult> GetBySpecialty(string specialty)
		{
			var response = await _doctorService.GetBySpecialtyAsync(specialty);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, [FromBody] DoctorDTO doctorDto)
		{
			var response = await _doctorService.UpdateDoctorAsync(id, doctorDto);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var response = await _doctorService.DeleteDoctorAsync(id);
			return response.IsSuccess ? Ok(response) : NotFound(response);
		}
	}
}
