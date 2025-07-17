using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Entities
{
	public class User : BaseEntity
	{

		[Required]
		[StringLength(50)]
		public string Username { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		[Required]
		[StringLength(50)]
		public string Role { get; set; }

		public Doctor Doctor { get; set; }
		public Patient Patient { get; set; }
	}
}
