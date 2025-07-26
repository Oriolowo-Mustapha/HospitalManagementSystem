using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Entities
{
	public class Insurance
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Range(0, 100)]
		public decimal DiscountPercentage { get; set; }

		public List<Patient> Patients { get; set; }
	}

}
