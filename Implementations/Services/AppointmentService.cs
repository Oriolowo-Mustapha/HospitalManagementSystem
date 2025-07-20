using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IAppointmentRepository _appointmentRepository;
		private readonly IScheduleRepository _scheduleRepository;

		public AppointmentService(IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository)
		{
			_appointmentRepository = appointmentRepository;
			_scheduleRepository = scheduleRepository;
		}

		public Task<ServiceResponse<AppointmentDTO>> ApproveAppointmentAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<AppointmentDTO>> CancelAppointmentAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResponse<AppointmentDTO>> CreateAppointmentAsync(AppointmentRequestDto requestDto)
		{
			var schedule = await _scheduleRepository.GetValidScheduleForAppointmentAsync(requestDto.DoctorId, requestDto.AppointmentDateTime);
			if (schedule == null)
			{
				throw new InvalidOperationException("No valid schedule found for the requested appointment time.");
			}

			var appointmentCount = await _scheduleRepository.GetAppointmentCountForScheduleAsync(schedule.Id);
			if (appointmentCount >= schedule.DailyAppointmentLimit)
			{
				throw new InvalidOperationException("Daily appointment limit reached for this schedule.");
			}

			if (!await _appointmentRepository.IsAppointmentSlotAvailableAsync(schedule.Id, requestDto.AppointmentDateTime))
			{
				throw new InvalidOperationException("Appointment slot is already booked.");
			}

			var appointment = new Appointment
			{
				PatientId = requestDto.PatientId,
				DoctorId = requestDto.DoctorId,
				ScheduleId = schedule.Id,
				AppointmentDateTime = requestDto.AppointmentDateTime
			};

			var createdAppointment = await _appointmentRepository.CreateAsync(appointment);

			var updatedAppointment = new AppointmentDTO
			{
				Id = createdAppointment.Id,
				PatientId = createdAppointment.PatientId,
				DoctorId = createdAppointment.DoctorId,
				AppointmentDateTime = createdAppointment.AppointmentDateTime,
				Notes = createdAppointment.Notes
			};

			return new ServiceResponse<AppointmentDTO>
			{
				Data = updatedAppointment,
				IsSuccess = true,
				Message = "Appointment Scheduled Successfully"
			};
		}

		public async Task<ServiceResponse<bool>> DeleteAppointmentAsync(Guid id)
		{
			var appointment = await _appointmentRepository.GetByIdAsync(id);
			if (appointment == null)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = "Appointment not found"
				};
			}
			var delete = await _appointmentRepository.DeleteAsync(id);
			if (delete == null)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = "Appointment Unable To Delete"
				};
			}
			return new ServiceResponse<bool>
			{
				IsSuccess = true,
				Message = "Appointment Deleted Succesfully"
			};
		}

		public Task<ServiceResponse<AppointmentDTO>> DisapproveAppointmentAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<List<AppointmentDTO>>> GetAllAppointmentsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<AppointmentDTO>> GetAppointmentByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResponse<List<AppointmentDTO>>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
		{
			try
			{
				var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId);

				if (appointments == null)
				{
					return new ServiceResponse<List<AppointmentDTO>>
					{
						IsSuccess = false,
						Message = "Appointment Unavailable.\nPls TryAgain."
					};
				}

				var GetAppointment = new List<AppointmentDTO>();

				foreach (var appointment in appointments)
				{
					GetAppointment.Add(new AppointmentDTO
					{
						Id = appointment.Id,
						PatientId = appointment.PatientId,
						DoctorId = appointment.DoctorId,
						AppointmentDateTime = appointment.AppointmentDateTime,
						Notes = appointment.Notes
					});
				}

				return new ServiceResponse<List<AppointmentDTO>>
				{
					Data = GetAppointment,
					IsSuccess = true,
					Message = "Appointment Successfully Retrieved."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<AppointmentDTO>>
				{
					IsSuccess = false,
					Message = "Failed to get all doctor's appointment."
				};
			}
		}

		public async Task<ServiceResponse<List<AppointmentDTO>>> GetAppointmentsByPatientIdAsync(Guid patientId)
		{
			try
			{
				var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);

				if (appointments == null)
				{
					return new ServiceResponse<List<AppointmentDTO>>
					{
						IsSuccess = false,
						Message = "Appointment Unavailable.\nPls TryAgain."
					};
				}

				var GetAppointment = new List<AppointmentDTO>();

				foreach (var appointment in appointments)
				{
					GetAppointment.Add(new AppointmentDTO
					{
						Id = appointment.Id,
						PatientId = appointment.PatientId,
						DoctorId = appointment.DoctorId,
						AppointmentDateTime = appointment.AppointmentDateTime,
						Notes = appointment.Notes
					});
				}

				return new ServiceResponse<List<AppointmentDTO>>
				{
					Data = GetAppointment,
					IsSuccess = true,
					Message = "Appointment Successfully Retrieved."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<AppointmentDTO>>
				{
					IsSuccess = false,
					Message = "Failed to get all patient's appintment."
				};
			}
		}



		public async Task<ServiceResponse<AppointmentDTO>> RescheduleAppointmentAsync(Guid id, AppointmentUpdateDto updateDto)
		{
			var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
			if (existingAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment slot doesn't exists"
				};
			}

			var schedule = await _scheduleRepository.GetValidScheduleForAppointmentAsync(updateDto.DoctorId, updateDto.AppointmentDateTime);
			if (schedule == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "No valid schedule found for the requested appointment time."
				};
			}

			var appointmentCount = await _scheduleRepository.GetAppointmentCountForScheduleAsync(schedule.Id);
			if (appointmentCount >= schedule.DailyAppointmentLimit && schedule.Id != existingAppointment.ScheduleId)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Daily appointment limit reached for this schedule."
				};
			}

			if (!await _appointmentRepository.IsAppointmentSlotAvailableAsync(schedule.Id, updateDto.AppointmentDateTime, id))
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment slot is already booked."
				};
			}

			var appointment = new Appointment
			{
				Id = id,
				PatientId = updateDto.PatientId,
				DoctorId = updateDto.DoctorId,
				ScheduleId = schedule.Id,
				AppointmentDateTime = updateDto.AppointmentDateTime,
				Notes = updateDto.Notes
			};

			var updatedAppointment = await _appointmentRepository.UpdateAsync(appointment);

			var Update = new AppointmentDTO
			{
				Id = updatedAppointment.Id,
				PatientId = updatedAppointment.PatientId,
				DoctorId = updatedAppointment.DoctorId,
				AppointmentDateTime = updatedAppointment.AppointmentDateTime,
				Notes = updatedAppointment.Notes
			};

			return new ServiceResponse<AppointmentDTO>
			{
				Data = Update,
				IsSuccess = true,
				Message = "Appointment Rescheduled Successfuly"
			};
		}
	}
}
