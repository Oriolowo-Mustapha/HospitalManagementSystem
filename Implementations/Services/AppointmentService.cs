using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class AppointmentService(IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository, IDoctorRepository doctorRepository) : IAppointmentService
	{

		public async Task<ServiceResponse<AppointmentDTO>> ApproveAppointmentAsync(Guid id)
		{
			var GetAppointment = await appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Approved;
			var UpdateAppointment = await appointmentRepository.UpdateAsync(GetAppointment);

			if (UpdateAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Failed To Approved Appointment"
				};
			}
			return new ServiceResponse<AppointmentDTO>
			{
				IsSuccess = true,
				Message = "Appointment Approved Successfully"
			};
		}

		public async Task<ServiceResponse<AppointmentDTO>> CancelAppointmentAsync(Guid id)
		{
			var GetAppointment = await appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Cancelled;
			var UpdateAppointment = await appointmentRepository.UpdateAsync(GetAppointment);

			if (UpdateAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Failed To Cancel Appointment"
				};
			}
			return new ServiceResponse<AppointmentDTO>
			{
				IsSuccess = true,
				Message = "Appointment Cancelled Successfully"
			};
		}

		public async Task<ServiceResponse<AppointmentDTO>> CreateAppointmentAsync(AppointmentRequestDto requestDto)
		{
			var availableDoctors = await doctorRepository.GetByAvailability(DoctorAvailability.Available);

			foreach (var doctor in availableDoctors)
			{
				var schedule = await scheduleRepository.GetValidScheduleForAppointmentAsync(doctor.Id, requestDto.AppointmentDateTime);

				if (schedule == null) continue;

				var appointmentCount = await scheduleRepository.GetAppointmentCountForScheduleAsync(schedule.Id);
				if (appointmentCount >= schedule.DailyAppointmentLimit) continue;

				var isSlotAvailable = await appointmentRepository.IsAppointmentSlotAvailableAsync(schedule.Id, requestDto.AppointmentDateTime);
				if (!isSlotAvailable) continue;

				// All conditions passed — create appointment
				var appointment = new Appointment
				{
					PatientId = requestDto.PatientId,
					DoctorId = doctor.Id,
					ScheduleId = schedule.Id,
					AppointmentDateTime = requestDto.AppointmentDateTime
				};

				var createdAppointment = await appointmentRepository.CreateAsync(appointment);

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
					Message = "Appointment scheduled successfully with an available doctor."
				};
			}

			// If no doctor was suitable
			return new ServiceResponse<AppointmentDTO>
			{
				IsSuccess = false,
				Message = "No available doctor could be found for the selected time."
			};
		}


		public async Task<ServiceResponse<bool>> DeleteAppointmentAsync(Guid id)
		{
			var appointment = await appointmentRepository.GetByIdAsync(id);
			if (appointment == null)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = "Appointment not found"
				};
			}
			var delete = await appointmentRepository.DeleteAsync(id);
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

		public async Task<ServiceResponse<AppointmentDTO>> DisapproveAppointmentAsync(Guid id)
		{
			var GetAppointment = await appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Disspproved;
			var UpdateAppointment = await appointmentRepository.UpdateAsync(GetAppointment);

			if (UpdateAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Failed To Disapprove"
				};
			}
			return new ServiceResponse<AppointmentDTO>
			{
				IsSuccess = true,
				Message = "Disapproved Successfully"
			};
		}

		public async Task<ServiceResponse<List<AppointmentDTO>>> GetAllAppointmentsAsync()
		{
			try
			{
				var appointments = await appointmentRepository.GetAllAsync();

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
					Message = "Failed to get all appointment."
				};
			}
		}

		public async Task<ServiceResponse<AppointmentDTO>> GetAppointmentByIdAsync(Guid id)
		{
			try
			{
				var appointment = await appointmentRepository.GetByIdAsync(id);

				if (appointment == null)
				{
					return new ServiceResponse<AppointmentDTO>
					{
						IsSuccess = false,
						Message = "Appointment Unavailable."
					};
				}

				var GetAppointment = new AppointmentDTO
				{
					Id = appointment.Id,
					PatientId = appointment.PatientId,
					DoctorId = appointment.DoctorId,
					AppointmentDateTime = appointment.AppointmentDateTime,
					Notes = appointment.Notes
				};

				return new ServiceResponse<AppointmentDTO>
				{
					Data = GetAppointment,
					IsSuccess = true,
					Message = "Appointment Successfully Retrieved."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Failed to get appointment."
				};
			}
		}

		public async Task<ServiceResponse<List<AppointmentDTO>>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
		{
			try
			{
				var appointments = await appointmentRepository.GetByDoctorIdAsync(doctorId);

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
				var appointments = await appointmentRepository.GetByPatientIdAsync(patientId);

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
					Message = "Failed to get all patient's appointment."
				};
			}
		}



		public async Task<ServiceResponse<AppointmentDTO>> RescheduleAppointmentAsync(Guid id, AppointmentUpdateDto updateDto)
		{
			var existingAppointment = await appointmentRepository.GetByIdAsync(id);
			if (existingAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment slot doesn't exists"
				};
			}

			var schedule = await scheduleRepository.GetValidScheduleForAppointmentAsync(updateDto.DoctorId, updateDto.AppointmentDateTime);
			if (schedule == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "No valid schedule found for the requested appointment time."
				};
			}

			var appointmentCount = await scheduleRepository.GetAppointmentCountForScheduleAsync(schedule.Id);
			if (appointmentCount >= schedule.DailyAppointmentLimit && schedule.Id != existingAppointment.ScheduleId)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Daily appointment limit reached for this schedule."
				};
			}

			if (!await appointmentRepository.IsAppointmentSlotAvailableAsync(schedule.Id, updateDto.AppointmentDateTime, id))
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
			};

			var updatedAppointment = await appointmentRepository.UpdateAsync(appointment);

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

		public async Task<ServiceResponse<AppointmentDTO>> UpdateAppointmentNote(Guid id, UploadAppointmentNoteRequestDto uploadAppointmentNote)
		{
			var GetAppointment = await appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment Not Found."
				};
			}

			GetAppointment.Notes = uploadAppointmentNote.Note;
			var updateAppointment = await appointmentRepository.UpdateAsync(GetAppointment);
			if (updateAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment Note Unable Uploaded."
				};
			}

			return new ServiceResponse<AppointmentDTO>
			{
				IsSuccess = true,
				Message = "Appoinment Note Uploaded."
			};
		}
	}
}
