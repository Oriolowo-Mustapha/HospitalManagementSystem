using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Patient : BaseEntity
	{
		public DateTime DateOfBirth { get; set; }

		[StringLength(11)]
		public string Phone { get; set; }

		[ForeignKey("Insurance")]
		public int? InsuranceId { get; set; }
		public Insurance Insurance { get; set; }

		[NotMapped]
		public decimal InsuranceDiscount => Insurance?.DiscountPercentage ?? 0;

		[ForeignKey("User")]
		public Guid? UserId { get; set; }
		public User User { get; set; }

		public List<Appointment> Appointments { get; set; }
		public List<Billing> Billings { get; set; }
	}

}
