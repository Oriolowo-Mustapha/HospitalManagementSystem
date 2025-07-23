using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Implementations.Repository;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;
using System.Numerics;

namespace HospitalManagementSystem.Implementations.Services
{
	public class PatientService : IPatientService
	{
		private readonly IPatientRepository _patientRepository;

		public PatientService(IPatientRepository patientRepository)
		{
			_patientRepository = patientRepository;
		}



		public async Task<ServiceResponse<PatientDTO>> GetPatientByIdAsync(Guid id)
		{
			try
			{
				var patient = await _patientRepository.GetPatientByIdAsync(id);
				if (patient == null)
				{
					return new ServiceResponse<PatientDTO>
					{
						IsSuccess = false,
						Message = "Patient not found."
					};
				}

			return new PatientDTO
			{
				Id = patient.Id,
				Phone = patient.Phone,
				InsuranceProvider = patient.InsuranceProvider,
				InsuranceDiscount = patient.InsuranceDiscount,
                Name = $"{patient.User.FirstName} {patient.User.LastName}",
				Email = patient.User.Email
            };
        }

		public async Task<IEnumerable<GetAllPatients>> GetAllPatientsAsync()
		{
			var patients = await _patientRepository.GetAllPatientsAsync();
			var patientDtos = new List<GetAllPatients>();

			foreach (var patient in patients)
			{
				patientDtos.Add(new GetAllPatients
                {					
					Phone = patient.Phone,					
                     Name = $"{patient.User.FirstName} {patient.User.LastName}",
                    Email = patient.User.Email
                });
			}

				return new ServiceResponse<List<PatientDTO>>
				{
					Data = patientDtos,
					IsSuccess = true,
					Message = patientDtos.Any() ? "Patients retrieved successfully." : "No patients found."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<PatientDTO>>
				{
					IsSuccess = false,
					Message = $"Failed to retrieve patients: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<PatientDTO>> GetPatientByEmailAsync(string email)
		{
			try
			{
				var patient = await _patientRepository.GetPatientByEmailAsync(email);
				if (patient == null)
				{
					return new ServiceResponse<PatientDTO>
					{
						IsSuccess = false,
						Message = "Patient not found."
					};
				}

			return new PatientDTO
			{
				Id = patient.Id,
				Phone = patient.Phone,
                Name = $"{patient.User.FirstName} {patient.User.LastName}",
                Email = patient.User.Email,
                InsuranceProvider = patient.InsuranceProvider,
				InsuranceDiscount = patient.InsuranceDiscount
			};
		}


			public async Task UpdatePatientAsync(Guid id, UpdatePatientDTO patientDto)
			{
				var patient = await _patientRepository.GetPatientByIdAsync(id);
				if (patient == null)
				{
					throw new Exception("Patient not found");
				}

				
				patient.Phone = patientDto.Phone;
				patient.InsuranceProvider = patientDto.InsuranceProvider;
				patient.InsuranceDiscount = patientDto.InsuranceDiscount;

				
				if (patient.User != null)
				{
					patient.User.FirstName = patientDto.firstname;
					patient.User.LastName = patientDto.lastname;
					patient.User.Email = patientDto.Email;
					patient.User.Username = patientDto.Username;


                if (!string.IsNullOrWhiteSpace(patientDto.Password) &&
                        !BCrypt.Net.BCrypt.Verify(patientDto.Password, patient.User.PasswordHash))
                {
                    patient.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patientDto.Password);
                }


                await _patientRepository.UpdatePatientAsync(patient);
			}
        }



        public async Task DeletePatientAsync(Guid id)
		{
			await _patientRepository.DeletePatientAsync(id);
		}
	}
}