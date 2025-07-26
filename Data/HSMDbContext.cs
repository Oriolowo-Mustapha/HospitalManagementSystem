using HospitalManagementSystem.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Data
{
	public class HSMDbContext : DbContext
	{
		public HSMDbContext(DbContextOptions<HSMDbContext> options) : base(options)
		{

		}
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Appointment> Appointments { get; set; }
		public DbSet<BillItem> BillItems { get; set; }
		public DbSet<Billing> Billings { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Schedule> Schedules { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<User>()
				.HasOne(u => u.Doctor)
				.WithOne(d => d.User)
				.HasForeignKey<Doctor>(d => d.UserId);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Patient)
				.WithOne(p => p.User)
				.HasForeignKey<Patient>(p => p.UserId);

			modelBuilder.Entity<Schedule>()
				.HasMany(s => s.Appointments)
				.WithOne(a => a.Schedule)
				.HasForeignKey(a => a.ScheduleId);

			modelBuilder.Entity<Doctor>()
				.HasMany(d => d.Schedules)
				.WithOne(s => s.Doctor)
				.HasForeignKey(s => s.DoctorId);

			modelBuilder.Entity<Doctor>()
				.HasMany(d => d.Appointments)
				.WithOne(a => a.Doctor)
				.HasForeignKey(a => a.DoctorId);

			modelBuilder.Entity<Patient>()
				.HasMany(p => p.Appointments)
				.WithOne(a => a.Patient)
				.HasForeignKey(a => a.PatientId);

			modelBuilder.Entity<Patient>()
				.HasMany(p => p.Billings)
				.WithOne(b => b.Patient)
				.HasForeignKey(b => b.PatientId);

			modelBuilder.Entity<Appointment>()
				.HasMany(a => a.Services)
				.WithOne(s => s.Appointment)
				.HasForeignKey(s => s.AppointmentId);

			//modelBuilder.Entity<Billing>()
			//	.HasMany(b => b.Services)
			//	.WithOne(ms => ms.Billings)
			//	.HasForeignKey(b => b.BillingId);

			modelBuilder.Entity<Billing>()
				.HasMany(b => b.Items)
				.WithOne(i => i.Bill)
				.HasForeignKey(i => i.BillingId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<User>().HasData(
				new User
				{
					Id = NewId.Next().ToGuid(),
					Username = "admin",
					PasswordHash = HashPassword("Admin123"),
					Email = "admin@gmail.com",
					FirstName = "Admin",
					LastName = "Admin",
					IsActive = true,
					CreatedAt = DateTime.UtcNow,
					Role = "Admin",
				}
			);
		}
		private string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

	}

}


