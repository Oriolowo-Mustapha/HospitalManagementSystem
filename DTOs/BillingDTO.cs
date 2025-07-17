using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class BillingDTO
	{
		public Guid Id { get; set; }
		public Guid PatientId { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal DiscountApplied { get; set; }
		public decimal FinalAmount { get; set; }
		public bool IsPaid { get; set; }
		public DateTime GeneratedDate { get; set; }
	}

	public class BillingRequestDto
	{
		[Required]
		public Guid PatientId { get; set; }

		[Required]
		public List<Guid> ServiceIds { get; set; }
	}

	public class BillingReportDto
	{
		public DateTime Date { get; set; }
		public decimal TotalBilled { get; set; }
		public int TotalAppointments { get; set; }
	}
}
