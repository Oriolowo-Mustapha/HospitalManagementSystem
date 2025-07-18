using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services
{
    public interface IPatientService
    {
        Task<PatientDTO> RegisterPatientAsync(RegisterPatientRequestDto requestDto);
        Task<PatientDTO> GetPatientByIdAsync(Guid id);
        Task<IEnumerable<PatientDTO>> GetAllPatientsAsync();
        Task<PatientDTO> GetPatientByEmailAsync(string email);
        Task UpdatePatientAsync(Guid id, PatientDTO patientDto);
        Task DeletePatientAsync(Guid id);
    }
}