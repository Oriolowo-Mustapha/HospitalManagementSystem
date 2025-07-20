using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Implementations.Repository
{
	public class AppointmentRepository : IAppointmentRepository
	{
		private readonly HSMDbContext _context;
		public AppointmentRepository(HSMDbContext context)
		{
			_context = context;
		}

		public async Task<Appointment> CreateAsync(Appointment appointment)
		{
			await _context.Appointments.AddAsync(appointment);
			await _context.SaveChangesAsync();
			return appointment;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment == null)
			{
				return false;
			}
			_context.Appointments.Remove(appointment);
			return true;
		}

		public async Task<List<Appointment>> GetAllAsync()
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.ToListAsync();
		}

		public async Task<List<Appointment>> GetByAppointmentStatus(AppointmentStatus appointmentStatus)
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.Where(a => a.AppointmentStatus == appointmentStatus)
				.ToListAsync();
		}

		public async Task<List<Appointment>> GetByDoctorIdAsync(Guid doctorId)
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.Where(a => a.DoctorId == doctorId)
				.ToListAsync();
		}

		public async Task<Appointment> GetByIdAsync(Guid id)
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task<List<Appointment>> GetByPatientIdAsync(Guid patientId)
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.Where(a => a.PatientId == patientId)
				.ToListAsync();
		}

		public async Task<List<Appointment>> GetByScheduleIdAsync(Guid scheduleId)
		{
			return await _context.Appointments
				.Include(a => a.Patient)
				.Include(a => a.Doctor)
				.Include(a => a.Schedule)
				.Include(a => a.Services)
				.Where(a => a.ScheduleId == scheduleId)
				.ToListAsync();
		}

		public async Task<bool> IsAppointmentSlotAvailableAsync(Guid scheduleId, DateTime appointmentDateTime, Guid? excludeAppointmentId = null)
		{
			var exists = await _context.Appointments.AnyAsync(a =>
				a.ScheduleId == scheduleId &&
				a.AppointmentDateTime == appointmentDateTime &&
				(!excludeAppointmentId.HasValue || a.Id != excludeAppointmentId.Value)
			);

			return !exists;
		}

		public async Task<Appointment> UpdateAsync(Appointment appointment)
		{
			var existingAppointment = await _context.Appointments
			   .FirstOrDefaultAsync(a => a.Id == appointment.Id);

			if (existingAppointment == null)
			{
				return null;
			}

			existingAppointment.PatientId = appointment.PatientId;
			existingAppointment.DoctorId = appointment.DoctorId;
			existingAppointment.ScheduleId = appointment.ScheduleId;
			existingAppointment.AppointmentDateTime = appointment.AppointmentDateTime;
			existingAppointment.AppointmentStatus = appointment.AppointmentStatus;
			existingAppointment.Notes = appointment.Notes;
			existingAppointment.UpdatedAt = appointment.UpdatedAt;
			await _context.SaveChangesAsync();
			return existingAppointment;
		}
	}
}
