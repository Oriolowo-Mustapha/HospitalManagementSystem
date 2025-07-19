using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IUserService
	{
		Task<ServiceResponse<AuthResponseModel>> SignUpAsync(RegisterPatientRequestDto model);
		Task<ServiceResponse<AuthResponseModel>> AddDoctorAsync(AddDoctorRequestDto model);
		Task<ServiceResponse<AuthResponseModel>> LoginAsync(LoginRequestDto model);
	}
}
