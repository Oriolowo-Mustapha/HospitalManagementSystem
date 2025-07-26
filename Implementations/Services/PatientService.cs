using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class PatientService : IPatientService
	{
		private readonly IPatientRepository _patientRepository;
		private readonly IUserRepository _userRepository;

		public PatientService(IPatientRepository patientRepository, IUserRepository userRepository)
		{
			_patientRepository = patientRepository;
			_userRepository = userRepository;
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
					Username = patient.User.Username,
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
					Message = $"Error occurred: {ex.Message}"
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
					Username = patient.User.Username,
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
					Username = patient.User.Username,
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
					Message = $"Error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<bool>> UpdatePatientAsync(Guid id, UpdatePatientRequestModel patientDto)
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

				var user = await _userRepository.GetUserByIdAsync((Guid)patient.UserId);

				user.FirstName = patientDto.FirstName;
				user.LastName = patientDto.LastName;
				user.Email = patientDto.Email;
				user.Username = patientDto.Username;

				if (!string.IsNullOrWhiteSpace(patientDto.Password) &&
					!BCrypt.Net.BCrypt.Verify(patientDto.Password, user.PasswordHash))
				{
					user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(patientDto.Password);
				}

				await _userRepository.UpdateUserAsync(user);

				patient.Phone = patientDto.Phone;
				patient.InsuranceProvider = patientDto.InsuranceProvider;
				patient.InsuranceDiscount = patientDto.InsuranceDiscount;

				await _patientRepository.UpdatePatientAsync(patient);

				return new ServiceResponse<bool>
				{
					IsSuccess = true,
					Message = "Patient updated successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = $"Update failed: {ex.Message}"
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

				await _userRepository.DeleteUserAsync((Guid)patient.UserId);
				await _patientRepository.DeletePatientAsync(id);

				return new ServiceResponse<bool>
				{
					IsSuccess = true,
					Message = "Patient deleted successfully."
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = $"Deletion failed: {ex.Message}"
				};
			}
		}
	}
}
