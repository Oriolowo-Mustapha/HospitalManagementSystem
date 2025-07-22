using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

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

				var patientDto = new PatientDTO
				{
					Id = patient.Id,
					FirstName = patient.User.FirstName,
					LastName = patient.User.LastName,
					Phone = patient.Phone,
					Email = patient.User.Email,
					InsuranceProvider = patient.InsuranceProvider,
					InsuranceDiscount = patient.InsuranceDiscount
				};

				return new ServiceResponse<PatientDTO>
				{
					Data = patientDto,
					IsSuccess = true,
					Message = "Patient retrieved successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<PatientDTO>
				{
					IsSuccess = false,
					Message = $"Failed to retrieve patient: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<List<PatientDTO>>> GetAllPatientsAsync()
		{
			try
			{
				var patients = await _patientRepository.GetAllPatientsAsync();
				var patientDtos = patients.Select(patient => new PatientDTO
				{
					Id = patient.Id,
					FirstName = patient.User.FirstName,
					LastName = patient.User.LastName,
					Phone = patient.Phone,
					Email = patient.User.Email,
					InsuranceProvider = patient.InsuranceProvider,
					InsuranceDiscount = patient.InsuranceDiscount
				}).ToList();

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

				var patientDto = new PatientDTO
				{
					Id = patient.Id,
					FirstName = patient.User.FirstName,
					LastName = patient.User.LastName,
					Phone = patient.Phone,
					Email = patient.User.Email,
					InsuranceProvider = patient.InsuranceProvider,
					InsuranceDiscount = patient.InsuranceDiscount
				};

				return new ServiceResponse<PatientDTO>
				{
					Data = patientDto,
					IsSuccess = true,
					Message = "Patient retrieved successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<PatientDTO>
				{
					IsSuccess = false,
					Message = $"Failed to retrieve patient: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<bool>> UpdatePatientAsync(Guid id, PatientDTO patientDto)
		{
			try
			{
				var patient = await _patientRepository.GetPatientByIdAsync(id);
				if (patient == null)
				{
					return new ServiceResponse<bool>
					{
						IsSuccess = false,
						Message = "Patient not found."
					};
				}

				// Check for email uniqueness if changed
				if (patient.User.Email != patientDto.Email)
				{
					var existingPatient = await _patientRepository.GetPatientByEmailAsync(patientDto.Email);
					if (existingPatient != null && existingPatient.Id != id)
					{
						return new ServiceResponse<bool>
						{
							IsSuccess = false,
							Message = "A patient with this email already exists."
						};
					}
				}

				patient.Phone = patientDto.Phone;
				patient.User.Email = patientDto.Email;
				patient.User.FirstName = patientDto.FirstName;
				patient.User.LastName = patientDto.LastName;
				patient.InsuranceProvider = patientDto.InsuranceProvider;
				patient.InsuranceDiscount = patientDto.InsuranceDiscount;

				await _patientRepository.UpdatePatientAsync(patient);

				return new ServiceResponse<bool>
				{
					Data = true,
					IsSuccess = true,
					Message = "Patient updated successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = $"Failed to update patient: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<bool>> DeletePatientAsync(Guid id)
		{
			try
			{
				var patient = await _patientRepository.GetPatientByIdAsync(id);
				if (patient == null)
				{
					return new ServiceResponse<bool>
					{
						IsSuccess = false,
						Message = "Patient not found."
					};
				}

				await _patientRepository.DeletePatientAsync(id);

				return new ServiceResponse<bool>
				{
					Data = true,
					IsSuccess = true,
					Message = "Patient deleted successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = $"Failed to delete patient: {ex.Message}"
				};
			}
		}
	}
}