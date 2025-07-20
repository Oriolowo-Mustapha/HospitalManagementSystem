using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.Interface.Repository
{
	public interface IAppointmentRepository
	{
		Task<Appointment> GetByIdAsync(Guid id);
		Task<List<Appointment>> GetAllAsync();
		Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId);
		Task<List<Appointment>> GetByPatientIdAsync(Guid patientId);
		Task<List<Appointment>> GetByScheduleIdAsync(Guid scheduleId);
		Task<List<Appointment>> GetByAppointmentStatus(AppointmentStatus appointmentStatus);
		Task<Appointment> CreateAsync(Appointment appointment);
		Task<Appointment> UpdateAsync(Appointment appointment);
		Task<bool> DeleteAsync(Guid id);
		Task<bool> IsAppointmentSlotAvailableAsync(Guid scheduleId, DateTime appointmentDateTime, Guid? excludeAppointmentId = null);
	}
}
