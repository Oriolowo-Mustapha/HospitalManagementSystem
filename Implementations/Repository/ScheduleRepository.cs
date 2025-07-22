using HospitalManagementSystem.Data;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Implementations.Repository
{
	public class ScheduleRepository : IScheduleRepository
	{
		private readonly HSMDbContext _context;
		public ScheduleRepository(HSMDbContext context)
		{
			_context = context;
		}
		public async Task<Schedule> CreateAsync(Schedule schedule)
		{
			_context.Schedules.Add(schedule);
			await _context.SaveChangesAsync();
			return schedule;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var schedule = await _context.Schedules.FindAsync(id);
			if (schedule == null)
			{
				return false;
			}

			_context.Schedules.Remove(schedule);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<int> GetAppointmentCountForScheduleAsync(Guid scheduleId)
		{
			return await _context.Appointments
				.CountAsync(a => a.ScheduleId == scheduleId);
		}

		public async Task<Schedule> GetByCurrentScheduleDoctorIdAsync(Guid doctorId)
		{
			return await _context.Schedules
			   .Include(s => s.Doctor)
			   .Include(s => s.Appointments)
			   .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.Date == DateTime.Now);
		}

		public async Task<List<Schedule>> GetByDoctorIdAsync(Guid doctorId)
		{
			return await _context.Schedules
				.Include(s => s.Doctor)
				.Include(s => s.Appointments)
				.Where(s => s.DoctorId == doctorId)
				.ToListAsync();
		}

		public async Task<Schedule> GetByIdAsync(Guid id)
		{
			return await _context.Schedules
			   .Include(s => s.Doctor)
			   .Include(s => s.Appointments)
			   .FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<Schedule> GetValidScheduleForAppointmentAsync(Guid doctorId, DateTime appointmentDateTime)
		{
			return await _context.Schedules
				.Where(s => s.DoctorId == doctorId &&
						   s.Date.Date == appointmentDateTime.Date &&
						   s.StartTime <= appointmentDateTime.TimeOfDay &&
						   s.EndTime >= appointmentDateTime.TimeOfDay)
				.FirstOrDefaultAsync();
		}

		public async Task<Schedule> UpdateAsync(Schedule schedule)
		{
			var existingSchedule = await _context.Schedules
				.FirstOrDefaultAsync(s => s.Id == schedule.Id);

			if (existingSchedule == null)
			{
				return null;
			}

			existingSchedule.Date = schedule.Date;
			existingSchedule.StartTime = schedule.StartTime;
			existingSchedule.EndTime = schedule.EndTime;
			existingSchedule.DailyAppointmentLimit = schedule.DailyAppointmentLimit;
			existingSchedule.UpdatedAt = schedule.UpdatedAt;

			await _context.SaveChangesAsync();
			return existingSchedule;
		}

		public async Task<bool> ValidateScheduleAsync(Guid doctorId, ScheduleDTO scheduleDto, Guid? excludeScheduleId = null)
		{
			if (scheduleDto.StartTime >= scheduleDto.EndTime)
			{
				return false;
			}

			var overlappingSchedules = await _context.Schedules
				.Where(s => s.DoctorId == doctorId &&
						   s.Date.Date == scheduleDto.Date.Date &&
						   s.Id != (excludeScheduleId ?? Guid.Empty) &&
						   (s.StartTime < scheduleDto.EndTime && s.EndTime > scheduleDto.StartTime))
				.AnyAsync();

			return !overlappingSchedules;
		}
	}
}
