namespace HospitalManagementSystem.DTOs
{
	public class PatientDTO
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Username { get; set; }
		public string InsuranceProvider { get; set; }
		public decimal InsuranceDiscount { get; set; }
	}


	public class UpdatePatientDTO
	{
		public Guid Id { get; set; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string InsuranceProvider { get; set; }
		public decimal InsuranceDiscount { get; set; }
	}


	public class GetAllPatients
	{

		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }

	}

	public class UpdatePatientRequestModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string InsuranceProvider { get; set; }
		public decimal InsuranceDiscount { get; set; }
	}

}
