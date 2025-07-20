using HospitalManagementSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class AppointmentDTO
	{
		public Guid Id { get; set; }
		public Guid PatientId { get; set; }
		public Guid DoctorId { get; set; }
		public DateTime AppointmentDateTime { get; set; }
		public AppointmentStatus AppointmentStatus { get; set; }
		public string Notes { get; set; }
	}
	public class AppointmentRequestDto
	{
		[Required]
		public Guid PatientId { get; set; }

		[Required]
		public Guid DoctorId { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }
		public AppointmentStatus Status { get; set; }
	}
	public class AppointmentUpdateDto
	{
		[Required]
		public Guid PatientId { get; set; }
		[Required]
		public Guid DoctorId { get; set; }
		[Required]
		public DateTime AppointmentDateTime { get; set; }
		public AppointmentStatus Status { get; set; }
		public string Notes { get; set; }
	}
}
