using HospitalManagementSystem.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
		public DbSet<MedicalService> MedicalServices { get; set; }
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

			modelBuilder.Entity<MedicalService>()
				.HasMany(ms => ms.Billings)
				.WithMany(b => b.Services);

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
		private static string HashPassword(string rawData)
		{
			using var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
			var builder = new StringBuilder();
			foreach (var b in bytes)
				builder.Append(b.ToString("x2"));
			return builder.ToString();
		}

	}

}


