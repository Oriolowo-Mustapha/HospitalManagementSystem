using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IPatientService
	{
		Task<ServiceResponse<PatientDTO>> GetPatientByIdAsync(Guid id);
		Task<ServiceResponse<List<PatientDTO>>> GetAllPatientsAsync();
		Task<ServiceResponse<PatientDTO>> GetPatientByEmailAsync(string email);
		Task<ServiceResponse<bool>> UpdatePatientAsync(Guid id, PatientDTO patientDto);
		Task<ServiceResponse<bool>> DeletePatientAsync(Guid id);
	}
}