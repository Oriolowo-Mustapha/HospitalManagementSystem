namespace HospitalManagementSystem.DTOs
{
	public class AuthResponseModel
	{
		public Guid UserId { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Token { get; set; }

	}

	public class AuthSignUpResponseModel
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
