namespace HospitalManagementSystem.Interface.Repository
{
	public interface ISpecialtyRepository
	{
		Task<bool> ExistsByNameAsync(string name);
		Task<List<string>> GetAllSpecialtiesAsync();
	}
}
