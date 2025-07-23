using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Interface.Services
{
    public interface IBillingService
    {
        Task<ServiceResponse<Billing>> CreateBillForAppointmentAsync(BillingDto dto);
        Task<ServiceResponse<BillingDto>> GetBillByAppointmentIdAsync(Guid appointmentId);
          Task<ServiceResponse<List<BillingDto>>> GetAllBillsAsync();
        Task<ServiceResponse<bool>> MarkBillAsPaidAsync(Guid billId);
    }
}
