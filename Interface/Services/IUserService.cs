using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IUserService
	{
		Task<ServiceResponse<AuthSignUpResponseModel>> SignUpAsync(RegisterPatientRequestDto model);
		Task<ServiceResponse<AuthSignUpResponseModel>> AddDoctorAsync(AddDoctorRequestDto model);
		Task<ServiceResponse<AuthResponseModel>> LoginAsync(LoginRequestDto model);
	}
}
