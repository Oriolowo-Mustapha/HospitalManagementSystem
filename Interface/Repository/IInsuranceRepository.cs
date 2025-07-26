using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Interface.Repository
{
	public interface IInsuranceRepository
	{
		Task<bool> ExistsByNameAsync(string name);
		Task<Insurance> GetByNameAsync(string name);
		Task<List<Insurance>> GetAllAsync();
		Task AddAsync(Insurance insurance);
	}
}
