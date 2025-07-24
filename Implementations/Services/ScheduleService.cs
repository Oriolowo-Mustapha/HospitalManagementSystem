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
		public async Task<ServiceResponse<ScheduleDTO>> CreateScheduleAsync(createScheduleRequestModel scheduleDto, Guid doctorId)
		{
			var doctor = await _doctorRepository.GetByIdAsync(doctorId);
			if (doctor == null)
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Doctor not found"
				};
			}
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
				Message = "Schedule Retrieved Successfully"
			};
		}
		public async Task<ServiceResponse<List<ScheduleDTO>>> GetSchedulesByDoctorIdAsync(Guid doctorId)
		{
			try
			{
				var schedules = await _scheduleRepository.GetByDoctorIdAsync(doctorId);

				// Check for both null and empty list
				if (schedules == null || !schedules.Any())
				{
					return new ServiceResponse<List<ScheduleDTO>>
					{
						IsSuccess = false,
						Message = "No schedules available for this doctor."
					};
				}

				var scheduleDtos = schedules.Select(schedule => new ScheduleDTO
				{
					Date = schedule.Date,
					StartTime = schedule.StartTime,
					Id = schedule.Id,
					DoctorId = schedule.DoctorId,
					EndTime = schedule.EndTime,
					DailyAppointmentLimit = schedule.DailyAppointmentLimit
				}).ToList();

				return new ServiceResponse<List<ScheduleDTO>>
				{
					Data = scheduleDtos,
					IsSuccess = true,
					Message = "Doctor Schedules retrieved successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<ScheduleDTO>>
				{
					IsSuccess = false,
					Message = $"Error retrieving schedules: {ex.Message}"
				};
			}
		}


		public async Task<ServiceResponse<ScheduleDTO>> UpdateScheduleAsync(Guid Id, Guid doctorId, createScheduleRequestModel scheduleDto)
		{
			var GetScheduleById = await _scheduleRepository.GetByIdAsync(Id);
			if (GetScheduleById == null)
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Schedule not found,Try Adding A Schedule Before Updating it."
				};
			}


			if (!await _scheduleRepository.ValidateScheduleAsync(GetScheduleById.DoctorId, scheduleDto, Id))
			{
				return new ServiceResponse<ScheduleDTO>
				{
					IsSuccess = false,
					Message = "Updated schedule overlaps with existing schedule or is invalid."
				};

			}

			var schedule = new Schedule
			{
				Id = Id,
				DoctorId = GetScheduleById.DoctorId,
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

		public async Task<ServiceResponse<bool>> ValidateScheduleAsync(Guid doctorId, createScheduleRequestModel scheduleDto)
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