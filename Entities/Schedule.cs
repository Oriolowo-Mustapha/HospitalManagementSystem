using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Schedule : BaseEntity
	{
		[ForeignKey("Doctor")]
		public Guid DoctorId { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }

		public int DailyAppointmentLimit { get; set; }

		public Doctor Doctor { get; set; }
	}
}
