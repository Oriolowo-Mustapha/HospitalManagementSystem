using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Interface.Repository
{
    public interface IBillRepository
    {
        Task<Billing> CreateAsync(Billing bill);
        Task<Billing> GetByAppointmentIdAsync(Guid appointmentId);
        Task<Billing> GetByIdAsync(Guid id);
        Task<List<Billing>> GetAllAsync();
        Task<bool> MarkAsPaidAsync(Guid billId);
        Task<Billing> GetBillWithPatientByAppointmentIdAsync(Guid appointmentId);
    }
}
