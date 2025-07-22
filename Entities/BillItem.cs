namespace HospitalManagementSystem.Entities
{
    public class BillItem : BaseEntity
    {
        public Guid BillingId { get; set; } 

        public Billing Bill { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

}
