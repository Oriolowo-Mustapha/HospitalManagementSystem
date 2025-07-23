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

		public async Task<ServiceResponse<Billing>> CreateBillForAppointmentAsync(BillingDto dto)
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

        public async Task<ServiceResponse<BillingDto>> GetBillByAppointmentIdAsync(Guid appointmentId)
        {
            var bill = await _billRepository.GetBillWithPatientByAppointmentIdAsync(appointmentId);

            if (bill == null)
            {
                return new ServiceResponse<BillingDto>
                {
                    IsSuccess = false,
                    Message = "Bill not found"
                };
            }

            var dto = new BillingDto
            {
                Id = bill.Id,
                AppointmentId = bill.AppointmentId,
                PatientFirstName = bill.Appointment.Patient.User.FirstName,
                PatientLastName = bill.Appointment.Patient.User.LastName,
                Items = bill.Items.Select(i => new BillingItemDto
                {
                    Description = i.Description,
                    Amount = i.Amount
                }).ToList()
            };

            return new ServiceResponse<BillingDto>
            {
                IsSuccess = true,
                Data = dto
            };
        }

        public async Task<ServiceResponse<List<BillingDto>>> GetAllBillsAsync()
        {
            var bills = await _billRepository.GetAllAsync();

            var billDtos = bills.Select(b => new BillingDto
            {
                Id = b.Id,
                AppointmentId = b.AppointmentId,
                PatientFirstName = b.Appointment?.Patient?.User?.FirstName,
                PatientLastName = b.Appointment?.Patient?.User?.LastName,

                Items = b.Items.Select(i => new BillingItemDto
                {
                    Description = i.Description,
                    Amount = i.Amount
                }).ToList()

            }).ToList();

            return new ServiceResponse<List<BillingDto>>
            {
                Data = billDtos,
                IsSuccess = true
            };
        }

        public async Task<ServiceResponse<bool>> MarkBillAsPaidAsync(Guid billId)
		{
			var success = await _billRepository.MarkAsPaidAsync(billId);
			return new ServiceResponse<bool> { Data = success, IsSuccess = success };
		}
	}

}
