using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Contract.Repository
{
    public interface IBillRepository
    {
        Task<Billing> CreateAsync(Billing bill);
        Task<Billing> GetByAppointmentIdAsync(Guid appointmentId);
        Task<Billing> GetByIdAsync(Guid id);
        Task<List<Billing>> GetAllAsync();
        Task<bool> MarkAsPaidAsync(Guid billId);
    }
}
