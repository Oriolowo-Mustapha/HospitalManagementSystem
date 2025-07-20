namespace HospitalManagementSystem.Entities
{
	public class BillItem : BaseEntity
	{
		public Billing Bill { get; set; }

		public string Description { get; set; }
		public decimal Amount { get; set; }
	}
}
