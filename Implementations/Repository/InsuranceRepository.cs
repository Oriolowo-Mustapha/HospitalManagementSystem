using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

public class InsuranceRepository : IInsuranceRepository
{
	private readonly HSMDbContext _context;

	public InsuranceRepository(HSMDbContext context)
	{
		_context = context;
	}

	public async Task<bool> ExistsByNameAsync(string name)
	{
		return await _context.Insurances.AnyAsync(i => i.Name.ToLower() == name.ToLower());
	}

	public async Task<Insurance> GetByNameAsync(string name)
	{
		return await _context.Insurances.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());
	}

	public async Task<List<Insurance>> GetAllAsync()
	{
		return await _context.Insurances.ToListAsync();
	}

	public async Task AddAsync(Insurance insurance)
	{
		await _context.Insurances.AddAsync(insurance);
		await _context.SaveChangesAsync();
	}
}
