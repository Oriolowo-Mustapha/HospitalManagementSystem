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

		[ForeignKey("Billing")]
		public Guid BillingId { get; set; }

		public Appointment Appointment { get; set; }

		public Billing Billings { get; set; }
	}
}
