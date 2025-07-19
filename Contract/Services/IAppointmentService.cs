using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Contract.Services
{
    public interface IAppointmentService
    {
        Task<ServiceResponse<AppointmentDTO>> CreateAsync(AppointmentDTO dto);
        Task<ServiceResponse<List<AppointmentDTO>>> GetAllAsync();
        Task<ServiceResponse<AppointmentDTO>> GetByIdAsync(Guid id);
        Task<ServiceResponse<List<AppointmentDTO>>> GetByPatientIdAsync(Guid patientId);
        Task<ServiceResponse<List<AppointmentDTO>>> GetByDoctorIdAsync(Guid doctorId);
        Task<ServiceResponse<AppointmentDTO>> UpdateAsync(Guid id, AppointmentDTO dto);
        Task<ServiceResponse<bool>> DeleteAsync(Guid id);
    }

}
