using HospitalManagementSystem.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Appointment : BaseEntity
	{

		[ForeignKey("Patient")]
		public Guid PatientId { get; set; }

		[ForeignKey("Doctor")]
		public Guid DoctorId { get; set; }
		[ForeignKey("Schedule")]
		public Guid ScheduleId { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }
		public AppointmentStatus AppointmentStatus { get; set; }

		public string Notes { get; set; }

		public Patient Patient { get; set; }
		public Doctor Doctor { get; set; }
		public Schedule Schedule { get; set; }
		public List<MedicalService> Services { get; set; }
	}
}
