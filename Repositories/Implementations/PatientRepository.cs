using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HSMDbContext _context;

        public PatientRepository(HSMDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            patient.Id = Guid.NewGuid();

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task<Patient> GetPatientByIdAsync(Guid id)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Billings)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Billings)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Patient> GetPatientByEmailAsync(string email)
        {
            return await _context.Patients
                .Include(p => p.Appointments)
                .Include(p => p.Billings)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}