using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IScheduleService
	{
		Task<ServiceResponse<ScheduleDTO>> GetScheduleByIdAsync(Guid id);
		Task<ServiceResponse<List<ScheduleDTO>>> GetSchedulesByDoctorIdAsync(Guid doctorId);
		Task<ServiceResponse<ScheduleDTO>> CreateScheduleAsync(createScheduleRequestModel scheduleDto, Guid doctorId);
		Task<ServiceResponse<ScheduleDTO>> UpdateScheduleAsync(Guid doctorId, createScheduleRequestModel scheduleDto);
		Task<ServiceResponse<bool>> DeleteScheduleAsync(Guid id);
		Task<ServiceResponse<bool>> ValidateScheduleAsync(Guid doctorId, createScheduleRequestModel scheduleDto);
	}
}
