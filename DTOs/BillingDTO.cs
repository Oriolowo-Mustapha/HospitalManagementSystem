using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
    //public class BillingDTO
    //{
    //	public Guid Id { get; set; }
    //	public Guid PatientId { get; set; }
    //	public decimal TotalAmount { get; set; }
    //	public decimal DiscountApplied { get; set; }
    //	public decimal FinalAmount { get; set; }
    //	public bool IsPaid { get; set; }
    //	public DateTime GeneratedDate { get; set; }
    //}

    //public class BillingRequestDto
    //{
    //	[Required]
    //	public Guid PatientId { get; set; }

    //	[Required]
    //	public List<Guid> ServiceIds { get; set; }
    //}

    //public class BillingReportDto
    //{
    //	public DateTime Date { get; set; }
    //	public decimal TotalBilled { get; set; }
    //	public int TotalAppointments { get; set; }
    //}

    public class BillingDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public List<BillingItemDto> Items { get; set; }
        public string BillingStatus { get; set; }
    }

    public class BillingItemDto
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }


}
