using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class InsuranceDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal DiscountPercentage { get; set; }
		public string Description { get; set; }
	}

	public class AddInsuranceDto
	{
		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		[Required]
		[Range(0, 100)]
		public decimal DiscountPercentage { get; set; }
	}
}
