using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<PatientDTO> RegisterPatientAsync(RegisterPatientRequestDto requestDto)
        {
            var patient = new Patient
            {
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                Phone = requestDto.Phone,
                InsuranceProvider = requestDto.InsuranceProvider,
                InsuranceDiscount = requestDto.InsuranceDiscount,
                DateOfBirth = requestDto.DateOfBirth
            };

            var createdPatient = await _patientRepository.CreatePatientAsync(patient);

            return new PatientDTO
            {
                Id = createdPatient.Id,
                Name = $"{createdPatient.FirstName} {createdPatient.LastName}",
                Email = createdPatient.Email,
                Phone = createdPatient.Phone,
                InsuranceProvider = createdPatient.InsuranceProvider,
                InsuranceDiscount = createdPatient.InsuranceDiscount
            };
        }

        public async Task<PatientDTO> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return null;
            }

            return new PatientDTO
            {
                Id = patient.Id,
                Name = $"{patient.FirstName} {patient.LastName}",
                Email = patient.Email,
                Phone = patient.Phone,
                InsuranceProvider = patient.InsuranceProvider,
                InsuranceDiscount = patient.InsuranceDiscount
            };
        }

        public async Task<IEnumerable<PatientDTO>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();
            var patientDtos = new List<PatientDTO>();

            foreach (var patient in patients)
            {
                patientDtos.Add(new PatientDTO
                {
                    Id = patient.Id,
                    Name = $"{patient.FirstName} {patient.LastName}",
                    Email = patient.Email,
                    Phone = patient.Phone,
                    InsuranceProvider = patient.InsuranceProvider,
                    InsuranceDiscount = patient.InsuranceDiscount
                });
            }

            return patientDtos;
        }

        public async Task<PatientDTO> GetPatientByEmailAsync(string email)
        {
            var patient = await _patientRepository.GetPatientByEmailAsync(email);
            if (patient == null)
            {
                return null;
            }

            return new PatientDTO
            {
                Id = patient.Id,
                Name = $"{patient.FirstName} {patient.LastName}",
                Email = patient.Email,
                Phone = patient.Phone,
                InsuranceProvider = patient.InsuranceProvider,
                InsuranceDiscount = patient.InsuranceDiscount
            };
        }

        public async Task UpdatePatientAsync(Guid id, PatientDTO patientDto)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                throw new Exception("Patient not found");
            }

            patient.FirstName = patientDto.Name.Split(' ')[0]; // Assuming first word is FirstName
            patient.LastName = patientDto.Name.Contains(' ') ? patientDto.Name.Substring(patientDto.Name.IndexOf(' ') + 1) : "";
            patient.Email = patientDto.Email;
            patient.Phone = patientDto.Phone;
            patient.InsuranceProvider = patientDto.InsuranceProvider;
            patient.InsuranceDiscount = patientDto.InsuranceDiscount;

            await _patientRepository.UpdatePatientAsync(patient);
        }

        public async Task DeletePatientAsync(Guid id)
        {
            await _patientRepository.DeletePatientAsync(id);
        }
    }
}