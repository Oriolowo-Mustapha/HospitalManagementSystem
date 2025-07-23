using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Implementations.Repository;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class AppointmentService : IAppointmentService
	{
		private readonly IAppointmentRepository _appointmentRepository;
		private readonly IScheduleRepository _scheduleRepository;
        private readonly IDoctorRepository _doctorRepository;

		public AppointmentService(IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository,IDoctorRepository doctorRepository)
		{
			_appointmentRepository = appointmentRepository;
			_scheduleRepository = scheduleRepository;
            _doctorRepository = doctorRepository;
		}

		public async Task<ServiceResponse<AppointmentDTO>> ApproveAppointmentAsync(Guid id)
		{
			var GetAppointment = await _appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Approved;
			var UpdateAppointment = await _appointmentRepository.UpdateAsync(GetAppointment);

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
			var GetAppointment = await _appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Cancelled;
			var UpdateAppointment = await _appointmentRepository.UpdateAsync(GetAppointment);

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
            // ✅ Ensure doctor exists and is a Medical Doctor
            var doctor = await _doctorRepository.GetByIdAsync(requestDto.DoctorId);
            if (doctor == null || doctor.Specialty != "Medical Doctor")
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Appointments can only be initially scheduled with a Medical Doctor."
                };
            }

            // ✅ Check valid schedule
            var schedule = await _scheduleRepository.GetValidScheduleForAppointmentAsync(requestDto.DoctorId, requestDto.AppointmentDateTime);
            if (schedule == null)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "No valid schedule found for the requested appointment time."
                };
            }

            var appointmentCount = await _scheduleRepository.GetAppointmentCountForScheduleAsync(schedule.Id);
            if (appointmentCount >= schedule.DailyAppointmentLimit)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Daily appointment limit reached for this schedule."
                };
            }

            if (!await _appointmentRepository.IsAppointmentSlotAvailableAsync(schedule.Id, requestDto.AppointmentDateTime))
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Appointment slot is already booked."
                };
            }

            var appointment = new Appointment
            {
                PatientId = requestDto.PatientId,
                DoctorId = requestDto.DoctorId,
                ScheduleId = schedule.Id,
                AppointmentDateTime = requestDto.AppointmentDateTime,
                AppointmentStatus = AppointmentStatus.Scheduled
            };

            var createdAppointment = await _appointmentRepository.CreateAsync(appointment);

            var updatedAppointment = new AppointmentDTO
            {
                Id = createdAppointment.Id,
                PatientId = createdAppointment.PatientId,
                DoctorId = createdAppointment.DoctorId,
                Status = createdAppointment.AppointmentStatus.ToString(),
                DoctorName = $"{createdAppointment.Doctor?.User?.FirstName ?? ""} {createdAppointment.Doctor?.User?.LastName ?? ""}".Trim(),
                PatientName = $"{createdAppointment.Patient?.User?.FirstName ?? ""} {createdAppointment.Patient?.User?.LastName ?? ""}".Trim(),
                AppointmentDateTime = createdAppointment.AppointmentDateTime,
                Notes = createdAppointment.Notes ?? ""
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

		public async Task<ServiceResponse<AppointmentDTO>> DisapproveAppointmentAsync(Guid id)
		{
			var GetAppointment = await _appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Invalid Appointment Id."
				};
			}

			GetAppointment.AppointmentStatus = Enum.AppointmentStatus.Disspproved;
			var UpdateAppointment = await _appointmentRepository.UpdateAsync(GetAppointment);

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
                var appointments = await _appointmentRepository.GetAllAsync();

                if (appointments == null || !appointments.Any())
                {
                    return new ServiceResponse<List<AppointmentDTO>>
                    {
                        IsSuccess = false,
                        Message = "No appointments found."
                    };
                }

                var appointmentDTOs = appointments.Select(a => new AppointmentDTO
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    Status = a.AppointmentStatus.ToString(),
                    DoctorName = $"{a.Doctor?.User?.FirstName ?? ""} {a.Doctor?.User?.LastName ?? ""}".Trim(),
                    PatientName = $"{a.Patient?.User?.FirstName ?? ""} {a.Patient?.User?.LastName ?? ""}".Trim(),
                    AppointmentDateTime = a.AppointmentDateTime,
                    Notes = a.Notes ?? ""
                }).ToList();

                return new ServiceResponse<List<AppointmentDTO>>
                {
                    Data = appointmentDTOs,
                    IsSuccess = true,
                    Message = "Appointments retrieved successfully."
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<AppointmentDTO>>
                {
                    IsSuccess = false,
                    Message = "Failed to get all appointments."
                };
            }
        }


        public async Task<ServiceResponse<AppointmentDTO>> GetAppointmentByIdAsync(Guid id)
		{
			try
			{
				var appointment = await _appointmentRepository.GetByIdAsync(id);

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
                    Notes = appointment.Notes ?? "",
                    DoctorName = $"{appointment.Doctor?.User?.FirstName} {appointment.Doctor?.User?.LastName}",
                    PatientName = $"{appointment.Patient?.User?.FirstName} {appointment.Patient?.User?.LastName}",
                    Status = appointment.AppointmentStatus.ToString()
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
                        Notes = appointment.Notes ?? "",
                        Status = appointment.AppointmentStatus.ToString(),
                        DoctorName = $"{appointment.Doctor?.User?.FirstName} {appointment.Doctor?.User?.LastName}",
                        PatientName = $"{appointment.Patient?.User?.FirstName} {appointment.Patient?.User?.LastName}"
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

                if (appointments == null || !appointments.Any())
                {
                    return new ServiceResponse<List<AppointmentDTO>>
                    {
                        IsSuccess = false,
                        Message = "No appointments found for this patient."
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
                        Notes = appointment.Notes ?? "",
                        Status = appointment.AppointmentStatus.ToString(),
                        DoctorName = $"{appointment.Doctor?.User?.FirstName} {appointment.Doctor?.User?.LastName}",
                        PatientName = $"{appointment.Patient?.User?.FirstName} {appointment.Patient?.User?.LastName}"
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
            var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
            if (existingAppointment == null)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Appointment slot doesn't exist."
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

            // Update the existing appointment
            existingAppointment.PatientId = updateDto.PatientId;
            existingAppointment.DoctorId = updateDto.DoctorId;
            existingAppointment.ScheduleId = schedule.Id;
            existingAppointment.AppointmentDateTime = updateDto.AppointmentDateTime;
            existingAppointment.Notes ="";
            existingAppointment.AppointmentStatus = AppointmentStatus.Rescheduled;
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            var updatedAppointment = await _appointmentRepository.UpdateAsync(existingAppointment);

            // Re-fetch including user navigation properties
            var populatedAppointment = await _appointmentRepository.GetByIdAsync(updatedAppointment.Id);

            var dto = new AppointmentDTO
            {
                Id = populatedAppointment.Id,
                PatientId = populatedAppointment.PatientId,
                DoctorId = populatedAppointment.DoctorId,
                AppointmentDateTime = populatedAppointment.AppointmentDateTime,
                Notes = populatedAppointment.Notes ?? "",
                Status = populatedAppointment.AppointmentStatus.ToString(),
                DoctorName = $"{populatedAppointment.Doctor?.User?.FirstName} {populatedAppointment.Doctor?.User?.LastName}",
                PatientName = $"{populatedAppointment.Patient?.User?.FirstName} {populatedAppointment.Patient?.User?.LastName}"
            };

            return new ServiceResponse<AppointmentDTO>
            {
                Data = dto,
                IsSuccess = true,
                Message = "Appointment rescheduled successfully."
            };
        }


        public async Task<ServiceResponse<AppointmentDTO>> UpdateAppointmentNote(Guid id, UploadAppointmentNoteRequestDto uploadAppointmentNote)
		{
			var GetAppointment = await _appointmentRepository.GetByIdAsync(id);
			if (GetAppointment == null)
			{
				return new ServiceResponse<AppointmentDTO>
				{
					IsSuccess = false,
					Message = "Appointment Not Found."
				};
			}

			GetAppointment.Notes = uploadAppointmentNote.Note;
			var updateAppointment = await _appointmentRepository.UpdateAsync(GetAppointment);
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
