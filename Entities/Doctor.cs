using HospitalManagementSystem.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Doctor : BaseEntity
	{

		[Required]
		[StringLength(50)]
		public string Specialty { get; set; }

		[Required]
		[StringLength(11)]
		public string Phone { get; set; }

		public DoctorAvailability Availability { get; internal set; }

		[ForeignKey("User")]
		public Guid UserId { get; set; }

		public User User { get; set; }
		public List<Schedule> Schedules { get; set; }
		public List<Appointment> Appointments { get; set; }
	}
}
