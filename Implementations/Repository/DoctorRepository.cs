using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Implementations.Repository
{
	public class DoctorRepository : IDoctorRepository
	{
		private readonly HSMDbContext _context;

		public DoctorRepository(HSMDbContext context)
		{
			_context = context;
		}

		public async Task<Doctor> CreateAsync(Doctor doctor)
		{
			await _context.Doctors.AddAsync(doctor);
			await _context.SaveChangesAsync();
			return doctor;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var doctor = await _context.Doctors
				.Include(d => d.User)
				.FirstOrDefaultAsync(d => d.Id == id || d.UserId == id);

			if (doctor == null)
			{
				return false;
			}

			_context.Users.Remove(doctor.User);

			await _context.SaveChangesAsync();
			return true;
		}



		public async Task<List<Doctor>> GetAllAsync()
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Include(d => d.Schedules)
				.Include(d => d.Appointments)
				.ToListAsync();
		}

		public async Task<List<Doctor>> GetByAvailability(DoctorAvailability availability)
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Include(d => d.Schedules)
				.Include(d => d.Appointments)
				.Where(d => d.Availability == availability)
				.ToListAsync();
		}

		public async Task<Doctor> GetByIdAsync(Guid id)
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Include(d => d.Schedules)
				.Include(d => d.Appointments)
				.FirstOrDefaultAsync(d => d.UserId == id || d.Id == id);
		}
		public async Task<List<Doctor>> GetBySpecialtyAsync(string specialty)
		{
			return await _context.Doctors
				.Include(d => d.User)
				.Include(d => d.Schedules)
				.Include(d => d.Appointments)
				.Where(d => d.Specialty != null && d.Specialty.ToLower() == specialty.ToLower())
				.ToListAsync();
		}
		public async Task<Doctor> UpdateAsync(Doctor doctor)
		{
			_context.Doctors.Update(doctor);
			await _context.SaveChangesAsync();
			return doctor;
		}

		public async Task<bool> UpdateScheduleAsync(Guid doctorId, List<Schedule> schedules)
		{
			throw new NotImplementedException();
		}
	}
}
