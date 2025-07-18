using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Contract.Services
{
    public interface IUserService
    {
         Task<ServiceResponse<AuthResponseModel>> SignUpAsync(RegisterPatientRequestDto model);
         Task<ServiceResponse<AuthResponseModel>> LoginAsync(LoginRequestDto model);
    }
}
