using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Contract.Services
{
    public interface IBillingService
    {
        Task<ServiceResponse<Billing>> CreateBillForAppointmentAsync(BillDTO dto);
        Task<ServiceResponse<Billing>> GetBillByAppointmentIdAsync(Guid appointmentId);
        Task<ServiceResponse<List<Billing>>> GetAllBillsAsync();
        Task<ServiceResponse<bool>> MarkBillAsPaidAsync(Guid billId);
    }
}
