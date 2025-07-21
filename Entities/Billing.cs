using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.Entities
{
	public class Billing : BaseEntity
	{

		public Guid AppointmentId { get; set; }
		public Appointment Appointment { get; set; }

		public Guid PatientId { get; set; }
		public Patient Patient { get; set; }

		public ICollection<BillItem> Items { get; set; } = new List<BillItem>();

		public decimal TotalAmount => Items?.Sum(x => x.Amount) ?? 0;

		public BillingStatus Status { get; set; } = BillingStatus.Pending;

		public DateTime BilledOn { get; set; } = DateTime.UtcNow;
	}


}
