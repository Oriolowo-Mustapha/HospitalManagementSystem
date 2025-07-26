using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class InsuranceService : IInsuranceService
	{
		private readonly IInsuranceRepository _insuranceRepo;

		public InsuranceService(IInsuranceRepository insuranceRepo)
		{
			_insuranceRepo = insuranceRepo;
		}

		public async Task<ServiceResponse<bool>> AddInsuranceAsync(AddInsuranceDto dto)
		{
			if (await _insuranceRepo.ExistsByNameAsync(dto.Name))
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = "Insurance provider already exists."
				};
			}

			var insurance = new Insurance
			{
				Name = dto.Name,
				Description = dto.Description,
				DiscountPercentage = dto.DiscountPercentage
			};

			await _insuranceRepo.AddAsync(insurance);

			return new ServiceResponse<bool>
			{
				IsSuccess = true,
				Message = "Insurance provider registered successfully.",
				Data = true
			};
		}

		public async Task<ServiceResponse<List<InsuranceDTO>>> GetAllAsync()
		{
			var insurances = await _insuranceRepo.GetAllAsync();

			var result = insurances.Select(i => new InsuranceDTO
			{
				Id = i.Id,
				Name = i.Name,
				Description = i.Description,
				DiscountPercentage = i.DiscountPercentage
			}).ToList();

			return new ServiceResponse<List<InsuranceDTO>>
			{
				Data = result,
				IsSuccess = true,
				Message = "Insurance providers retrieved successfully."
			};
		}

		public async Task<ServiceResponse<InsuranceDTO>> GetByNameAsync(string name)
		{
			var insurance = await _insuranceRepo.GetByNameAsync(name);
			if (insurance == null)
			{
				return new ServiceResponse<InsuranceDTO>
				{
					IsSuccess = false,
					Message = "Insurance not found"
				};
			}

			return new ServiceResponse<InsuranceDTO>
			{
				IsSuccess = true,
				Message = "Insurance found",
				Data = new InsuranceDTO
				{
					Id = insurance.Id,
					Name = insurance.Name,
					Description = insurance.Description,
					DiscountPercentage = insurance.DiscountPercentage
				}
			};
		}
	}
}
