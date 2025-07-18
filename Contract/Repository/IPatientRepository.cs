using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Contract.Repository
{
    public interface IPatientRepository
    {
        Task<Patient> CreatePatientAsync(Patient patient);
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByEmailAsync(string email);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(Guid id);
    }
}
