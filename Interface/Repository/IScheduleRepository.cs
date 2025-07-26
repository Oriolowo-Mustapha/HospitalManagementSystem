using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Interface.Repository
{
	public interface IScheduleRepository
	{
		Task<Schedule> GetByIdAsync(Guid id);
		Task<List<Schedule>> GetByDoctorIdAsync(Guid doctorId);
		Task<Schedule> GetByCurrentScheduleDoctorIdAsync(Guid doctorId);
		Task<Schedule> CreateAsync(Schedule schedule);
		Task<Schedule> UpdateAsync(Schedule schedule);
		Task<bool> DeleteAsync(Guid id);
		Task<Schedule> GetValidScheduleForAppointmentAsync(Guid doctorId, DateTime appointmentDateTime);
		Task<int> GetAppointmentCountForScheduleAsync(Guid scheduleId);
		Task<bool> ValidateScheduleAsync(Guid doctorId, createScheduleRequestModel scheduleDto, Guid? excludeScheduleId = null);
	}
}
