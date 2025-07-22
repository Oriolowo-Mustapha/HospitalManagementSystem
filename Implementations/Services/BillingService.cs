using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class BillingService : IBillingService
	{
		private readonly IBillRepository _billRepository;
		private readonly IAppointmentRepository _appointmentRepository;

		public BillingService(IBillRepository billRepository, IAppointmentRepository appointmentRepository)
		{
			_billRepository = billRepository;
			_appointmentRepository = appointmentRepository;
		}

		public async Task<ServiceResponse<Billing>> CreateBillForAppointmentAsync(BillDTO dto)
		{
			var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId);
			if (appointment == null || appointment.AppointmentStatus != AppointmentStatus.Completed)
			{
				return new ServiceResponse<Billing>
				{
					IsSuccess = false,
					Message = "Invalid or Incomplete appointment"
				};
			}

			var existing = await _billRepository.GetByAppointmentIdAsync(dto.AppointmentId);
			if (existing != null)
			{
				return new ServiceResponse<Billing>
				{
					IsSuccess = false,
					Message = "Bill already exists for this appointment"
				};
			}

			var bill = new Billing
			{
				AppointmentId = dto.AppointmentId,
				PatientId = appointment.PatientId,
				Status = BillingStatus.Pending,
				BilledOn = DateTime.UtcNow,
				Items = dto.Items.Select(i => new BillItem
				{
					Description = i.Description,
					Amount = i.Amount
				}).ToList()
			};

			var result = await _billRepository.CreateAsync(bill);
			return new ServiceResponse<Billing>
			{
				Data = result,
				IsSuccess = true,
				Message = "Bill created successfully"
			};
		}

		public async Task<ServiceResponse<Billing>> GetBillByAppointmentIdAsync(Guid appointmentId)
		{
			var bill = await _billRepository.GetByAppointmentIdAsync(appointmentId);
			if (bill == null)
			{
				return new ServiceResponse<Billing> { IsSuccess = false, Message = "Bill not found" };
			}

			return new ServiceResponse<Billing> { Data = bill, IsSuccess = true };
		}

		public async Task<ServiceResponse<List<Billing>>> GetAllBillsAsync()
		{
			var bills = await _billRepository.GetAllAsync();
			return new ServiceResponse<List<Billing>> { Data = bills, IsSuccess = true };
		}

		public async Task<ServiceResponse<bool>> MarkBillAsPaidAsync(Guid billId)
		{
			var success = await _billRepository.MarkAsPaidAsync(billId);
			return new ServiceResponse<bool> { Data = success, IsSuccess = success };
		}
	}

}
