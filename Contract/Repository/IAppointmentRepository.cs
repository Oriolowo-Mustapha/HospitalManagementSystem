using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Contract.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(Guid id);
        Task<List<Appointment>> GetByPatientIdAsync(Guid patientId);
        Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId);
        Task<Appointment> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(Guid id);
    }

}
