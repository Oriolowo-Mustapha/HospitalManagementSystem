using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;

namespace HospitalManagementSystem.Interface.Repository
{
	public interface IDoctorRepository
	{
		Task<Doctor> GetByIdAsync(Guid id);
		Task<List<Doctor>> GetBySpecialtyAsync(string specialty);
		Task<List<Doctor>> GetByAvailability(DoctorAvailability availability);
		Task<List<Doctor>> GetAllAsync();
		Task<Doctor> CreateAsync(Doctor doctor);
		Task<Doctor> UpdateAsync(Doctor doctor);
		Task<bool> DeleteAsync(Guid id);
	}
}
