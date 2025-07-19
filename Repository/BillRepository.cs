using HospitalManagementSystem.Contract.Repository;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Repository
{
    public class BillRepository : IBillRepository
    {
        private readonly HSMDbContext _context;

        public BillRepository(HSMDbContext context)
        {
            _context = context;
        }

        public async Task<Billing> CreateAsync(Billing bill)
        {
            await _context.Billings.AddAsync(bill);
            await _context.SaveChangesAsync();
            return bill;
        }

        public async Task<Billing> GetByAppointmentIdAsync(Guid appointmentId)
        {
            return await _context.Billings  
                .Include(b => b.Items)
                .Include(b => b.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(b => b.AppointmentId == appointmentId);
        }

        public async Task<Billing> GetByIdAsync(Guid id)
        {
            return await _context.Billings
                .Include(b => b.Items)
                .Include(b => b.Appointment)
                .ThenInclude(a => a.Patient)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Billing>> GetAllAsync()
        {
            return await _context.Billings
                .Include(b => b.Items)
                .Include(b => b.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync();
        }

        public async Task<bool> MarkAsPaidAsync(Guid billId)
        {
            var bill = await _context.Billings.FindAsync(billId);
            if (bill == null) return false;

            bill.Status = BillingStatus.Paid;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
