using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class ScheduleService : IScheduleService
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly IDoctorRepository _doctorRepository;
		public ScheduleService(IScheduleRepository scheduleRepository, IDoctorRepository doctorRepository)
		{
			_scheduleRepository = scheduleRepository;
			_doctorRepository = doctorRepository;
		}
		public async Task<ServiceResponse<ScheduleDTO>> CreateScheduleAsync(ScheduleDTO scheduleDto, Guid doctorId)
		{
			if (!await _scheduleRepository.ValidateScheduleAsync(doctorId, scheduleDto))
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Schedule overlaps with existing schedule or is invalid."
				};
			}

			var schedule = new Schedule
			{
				DoctorId = doctorId,
				Date = scheduleDto.Date,
				StartTime = scheduleDto.StartTime,
				EndTime = scheduleDto.EndTime,
				DailyAppointmentLimit = scheduleDto.DailyAppointmentLimit
			};

			var createdSchedule = await _scheduleRepository.CreateAsync(schedule);

			var scheduledDto = new ScheduleDTO
			{
				Id = createdSchedule.Id,
				DoctorId = createdSchedule.DoctorId,
				Date = createdSchedule.Date,
				StartTime = createdSchedule.StartTime,
				EndTime = createdSchedule.EndTime,
				DailyAppointmentLimit = createdSchedule.DailyAppointmentLimit
			};

			return new ServiceResponse<ScheduleDTO>
			{
				Data = scheduledDto,
				IsSuccess = true,
				Message = "Schedule Added Successfully"
			};
		}

		public async Task<ServiceResponse<bool>> DeleteScheduleAsync(Guid id)
		{
			var schedule = await _scheduleRepository.GetByIdAsync(id);
			if (schedule == null)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = "INvalid Schedule"
				};
			}

			var delete = await _scheduleRepository.DeleteAsync(id);
			if (delete == false)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Data = false,
					Message = "Schedule Failed To Delete"
				};
			}
			return new ServiceResponse<bool>
			{
				IsSuccess = true,
				Data = true,
				Message = "Schedule Deleted Successfully"
			};
		}

		public async Task<ServiceResponse<ScheduleDTO>> GetScheduleByIdAsync(Guid id)
		{
			var schedule = await _scheduleRepository.GetByIdAsync(id);
			if (schedule == null)
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Data = null,
					Message = "Invalid Schedule Id"
				};
			}

			var GetSchedule = new ScheduleDTO
			{
				Id = schedule.Id,
				DoctorId = schedule.DoctorId,
				Date = schedule.Date,
				StartTime = schedule.StartTime,
				EndTime = schedule.EndTime,
				DailyAppointmentLimit = schedule.DailyAppointmentLimit
			};

			return new ServiceResponse<ScheduleDTO>
			{
				Data = GetSchedule,
				IsSuccess = true,
				Message = ""
			};
		}

		public async Task<ServiceResponse<List<ScheduleDTO>>> GetSchedulesByDoctorIdAsync(Guid doctorId)
		{
			try
			{
				var schedules = await _scheduleRepository.GetByDoctorIdAsync(doctorId);

				if (schedules == null)
				{
					return new ServiceResponse<List<ScheduleDTO>>
					{
						IsSuccess = false,
						Message = "Schedule Unavailable.\nPls TryAgain."
					};
				}

				var GetSchedule = new List<ScheduleDTO>();

				foreach (var schedule in schedules)
				{
					GetSchedule.Add(new ScheduleDTO
					{
						Date = schedule.Date,
						StartTime = schedule.StartTime,
						EndTime = schedule.EndTime,
						DailyAppointmentLimit = schedule.DailyAppointmentLimit
					});
				}

				return new ServiceResponse<List<ScheduleDTO>>
				{
					Data = GetSchedule,
					IsSuccess = true,
					Message = "Schedule Successfully Retrieved."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<ScheduleDTO>>
				{
					IsSuccess = false,
					Message = "Failed to get all doctor's schedule."
				};
			}
		}

		public async Task<ServiceResponse<ScheduleDTO>> UpdateScheduleAsync(Guid doctorId, ScheduleDTO scheduleDto)
		{
			var existingDoctor = await _doctorRepository.GetByIdAsync(doctorId);
			if (existingDoctor == null)
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Doctor not found"
				};
			}

			var existingSchedule = await _scheduleRepository.GetByCurrentScheduleDoctorIdAsync(doctorId);
			if (existingSchedule == null)
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = $"Schedule on {existingSchedule.Date} Is Unavailable"
				};
			}

			if (!await _scheduleRepository.ValidateScheduleAsync(doctorId, scheduleDto, existingSchedule.Id))
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Updated schedule overlaps with existing schedule or is invalid."
				};

			}

			var schedule = new Schedule
			{
				Id = existingDoctor.Id,
				DoctorId = existingSchedule.DoctorId,
				Date = scheduleDto.Date,
				StartTime = scheduleDto.StartTime,
				EndTime = scheduleDto.EndTime,
				DailyAppointmentLimit = scheduleDto.DailyAppointmentLimit
			};

			var updatedSchedule = await _scheduleRepository.UpdateAsync(schedule);

			var updateScheduleDto = new ScheduleDTO
			{
				Id = updatedSchedule.Id,
				DoctorId = updatedSchedule.DoctorId,
				Date = updatedSchedule.Date,
				StartTime = updatedSchedule.StartTime,
				EndTime = updatedSchedule.EndTime,
				DailyAppointmentLimit = updatedSchedule.DailyAppointmentLimit
			};

			return new ServiceResponse<ScheduleDTO>
			{
				Data = updateScheduleDto,
				IsSuccess = true,
				Message = "Schedule Updated Succesfully"
			};
		}

		public async Task<ServiceResponse<bool>> ValidateScheduleAsync(Guid doctorId, ScheduleDTO scheduleDto)
		{
			var validate = await _scheduleRepository.ValidateScheduleAsync(doctorId, scheduleDto);
			if (validate == false)
			{
				return new ServiceResponse<bool>
				{
					Data = false,
					IsSuccess = false,
					Message = "Unable To Validate"
				};
			}
			return new ServiceResponse<bool>
			{
				Data = true,
				IsSuccess = true,
				Message = "Validation Done Successfully."
			};
		}
	}
}