using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class MedicalService : BaseEntity
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[Required]
		public decimal Cost { get; set; }

		[ForeignKey("Appointment")]
		public Guid AppointmentId { get; set; }

		public Appointment Appointment { get; set; }

		public List<Billing> Billings { get; set; }
	}
}
