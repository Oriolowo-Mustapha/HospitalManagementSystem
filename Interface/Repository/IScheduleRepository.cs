using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Interface.Repository
{
	public interface IScheduleRepository
	{
		Task<Schedule> GetByIdAsync(Guid id);
		Task<List<Schedule>> GetByDoctorIdAsync(Guid doctorId);
		Task<Schedule> CreateAsync(Schedule schedule);
		Task<Schedule> UpdateAsync(Schedule schedule);
		Task<bool> DeleteAsync(Guid id);
		Task<Schedule> GetValidScheduleForAppointmentAsync(Guid doctorId, DateTime appointmentDateTime);
		Task<int> GetAppointmentCountForScheduleAsync(Guid scheduleId);
	}
}
