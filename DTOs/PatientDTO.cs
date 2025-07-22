namespace HospitalManagementSystem.DTOs
{
	public class PatientDTO
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string InsuranceProvider { get; set; }
		public decimal InsuranceDiscount { get; set; }
	}
}
