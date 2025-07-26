using HospitalManagementSystem.Enum;
using System.Text.Json.Serialization;

namespace HospitalManagementSystem.DTOs
{
	public class DoctorDTO
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Specialty { get; set; }
		public string Availability { get; set; }
		public string Email { get; set; }

		[JsonIgnore]
		public string Password { get; set; }
	}

	public class DoctorResponseModel
	{
		public string FullName { get; set; }
		public string Specialty { get; set; }
		public string Email { get; set; }
		public string Availability { get; set; }
		public string Phone { get; set; }

	}


	public class UpdateDoctorDTO
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Specialty { get; set; }
		public DoctorAvailability Availability { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
