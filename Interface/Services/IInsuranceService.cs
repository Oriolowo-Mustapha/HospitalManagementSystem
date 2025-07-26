using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IInsuranceService
	{
		Task<ServiceResponse<bool>> AddInsuranceAsync(AddInsuranceDto dto);
		Task<ServiceResponse<List<InsuranceDTO>>> GetAllAsync();
		Task<ServiceResponse<InsuranceDTO>> GetByNameAsync(string name);
	}
}
