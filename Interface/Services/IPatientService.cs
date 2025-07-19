using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IPatientService
	{
		Task<PatientDTO> RegisterPatientAsync(RegisterPatientRequestDto requestDto);
		Task<PatientDTO> GetPatientByIdAsync(Guid id);
		Task<IEnumerable<PatientDTO>> GetAllPatientsAsync();
		Task<PatientDTO> GetPatientByEmailAsync(string email);
		Task UpdatePatientAsync(Guid id, PatientDTO patientDto);
		Task DeletePatientAsync(Guid id);
	}
}