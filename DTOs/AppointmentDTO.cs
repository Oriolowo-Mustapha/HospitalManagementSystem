using HospitalManagementSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class AppointmentDTO
	{
		public Guid Id { get; set; }
		public DateTime AppointmentDate => AppointmentDateTime.Date;

		// Change from enum to string
		public string Status { get; set; }

		public Guid PatientId { get; set; }
		public Guid DoctorId { get; set; }

		public string Notes { get; set; }

		public DateTime AppointmentDateTime { get; set; }

		public string DoctorName { get; set; }
		public string PatientName { get; set; }
	}



	public class AppointmentRequestDto
	{
		[Required]
		public Guid PatientId { get; set; }

		[Required]
		public Guid DoctorId { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }
		public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
	}

	public class UploadAppointmentNoteRequestDto
	{
		[Required]
		public string Note { get; set; }
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
	}
}
