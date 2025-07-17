using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Billing : BaseEntity
	{

		[ForeignKey("Patient")]
		public Guid PatientId { get; set; }

		[Required]
		public decimal TotalAmount { get; set; }

		public decimal DiscountApplied { get; set; }

		public decimal FinalAmount { get; set; }

		public bool IsPaid { get; set; }

		public DateTime GeneratedDate { get; set; }

		public Patient Patient { get; set; }
		public List<MedicalService> Services { get; set; }
	}
}
