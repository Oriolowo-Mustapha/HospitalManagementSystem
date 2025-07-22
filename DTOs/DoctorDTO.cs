using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.DTOs
{
	public class DoctorDTO
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
