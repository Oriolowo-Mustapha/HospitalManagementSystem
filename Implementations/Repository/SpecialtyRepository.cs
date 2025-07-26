using HospitalManagementSystem.Data;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Implementations.Repository
{
	public class SpecialtyRepository : ISpecialtyRepository
	{
		private readonly HSMDbContext _context;

		public SpecialtyRepository(HSMDbContext context)
		{
			_context = context;
		}

		public async Task<bool> ExistsByNameAsync(string name)
		{
			return await _context.Specialties.AnyAsync(s => s.Name.ToLower() == name.ToLower());
		}

		public async Task<List<string>> GetAllSpecialtiesAsync()
		{
			return await _context.Specialties.Select(s => s.Name).ToListAsync();
		}
	}
}
