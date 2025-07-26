using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IDoctorService
	{
		Task<ServiceResponse<DoctorDTO>> GetDoctorByIdAsync(Guid id);
		Task<ServiceResponse<List<DoctorResponseModel>>> GetBySpecialtyAsync(string specialty);
		Task<ServiceResponse<List<DoctorResponseModel>>> GetByAvailability(string availability);
		Task<ServiceResponse<List<DoctorResponseModel>>> GetAllDoctorsAsync();
		Task<ServiceResponse<DoctorDTO>> UpdateDoctorAsync(Guid id, UpdateDoctorDTO doctorDto);
		Task<ServiceResponse<bool>> DeleteDoctorAsync(Guid id);

	}
}

