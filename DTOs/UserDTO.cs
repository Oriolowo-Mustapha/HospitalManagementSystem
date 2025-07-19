using HospitalManagementSystem.Enum;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class UserDTO
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Role { get; set; }
	}
	public class RegisterPatientRequestDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string Role { get; set; } = "Patient";

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[StringLength(11, ErrorMessage = "Phone number must be 11 digits long.")]
		public string Phone { get; set; }

		public DateTime DateOfBirth { get; set; }
		public string InsuranceProvider { get; set; }
		public decimal InsuranceDiscount { get; set; }
	}

	public class AddDoctorRequestDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; } = "Doctor123";

		[Required]
		public string Role { get; set; } = "Doctor";

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[StringLength(50)]
		public string Specialty { get; set; }

		public DoctorAvailability Availability { get; set; } = DoctorAvailability.Available;

		[Required]
		public string Email { get; set; }
		[Required]
		[StringLength(11, ErrorMessage = "Phone number must be 11 digits long")]
		public string Phone { get; set; }
	}


	public class LoginRequestDto
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
