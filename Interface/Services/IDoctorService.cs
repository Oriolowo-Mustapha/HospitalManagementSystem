using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IDoctorService
	{
		Task<ServiceResponse<DoctorDTO>> GetDoctorByIdAsync(Guid id);
		Task<ServiceResponse<List<DoctorDTO>>> GetBySpecialtyAsync(string specialty);
		Task<ServiceResponse<List<DoctorDTO>>> GetByAvailability(DoctorAvailability availability);
		Task<ServiceResponse<List<DoctorResponseModel>>> GetAllDoctorsAsync();
		Task<ServiceResponse<DoctorDTO>> UpdateDoctorAsync(Guid id, DoctorDTO doctorDto);
		Task<ServiceResponse<bool>> DeleteDoctorAsync(Guid id);
	}
}
