using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.Contract.Services
{
    public interface IDoctorService
    {
        Task<ServiceResponse<DoctorDTO>> GetDoctorByIdAsync(Guid id);
        Task<ServiceResponse<List<DoctorDTO>>> GetBySpecialtyAsync(string specialty);
        Task<ServiceResponse<List<DoctorDTO>>> GetByAvailability(DoctorAvailability availability);
        Task<ServiceResponse<List<DoctorDTO>>> GetAllDoctorsAsync();
        Task<ServiceResponse<DoctorDTO>> UpdateDoctorAsync(Guid id, DoctorDTO doctorDto);
        Task<ServiceResponse<bool>> DeleteDoctorAsync(Guid id);
        Task<ServiceResponse<bool>> UpdateDoctorScheduleAsync(Guid doctorId, List<ScheduleDTO> schedules);
        Task<ServiceResponse<DoctorDTO>> CreateDoctorAsync(DoctorDTO doctorDto);
    }
}
