using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IPatientService
	{
		Task<PatientDTO> RegisterPatientAsync(RegisterPatientRequestDto requestDto);
		Task<PatientDTO> GetPatientByIdAsync(Guid id);
		Task<IEnumerable<GetAllPatients>> GetAllPatientsAsync();
		Task<PatientDTO> GetPatientByEmailAsync(string email);
		Task UpdatePatientAsync(Guid id, UpdatePatientDTO patientDto);
		Task DeletePatientAsync(Guid id);
	}
}