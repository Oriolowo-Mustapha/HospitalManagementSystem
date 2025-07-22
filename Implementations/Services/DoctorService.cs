using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;

namespace HospitalManagementSystem.Implementations.Services
{
	public class DoctorService : IDoctorService
	{
		private readonly IDoctorRepository _doctorRepository;
		public DoctorService(IDoctorRepository doctorRepository)
		{
			_doctorRepository = doctorRepository;
		}

		public async Task<ServiceResponse<bool>> DeleteDoctorAsync(Guid id)
		{
			try
			{
				var UserExits = await _doctorRepository.GetByIdAsync(id);
				if (UserExits == null)
				{
					return new ServiceResponse<bool>
					{
						IsSuccess = false,
						Message = "Doctor not found"
					};
				}
				var result = await _doctorRepository.DeleteAsync(id);
				if (result == true)
				{
					return new ServiceResponse<bool>
					{
						IsSuccess = true,
						Message = "Doctor deleted successfully"
					};
				}
				else
				{
					return new ServiceResponse<bool>
					{
						IsSuccess = false,
						Message = "Failed to delete doctor"
					};
				}
			}
			catch (Exception ex)
			{
				return new ServiceResponse<bool>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<List<DoctorDTO>>> GetAllDoctorsAsync()
		{
			try
			{
				var doctors = await _doctorRepository.GetAllAsync();
				var doctorDtos = new List<DoctorDTO>();

				foreach (var doctor in doctors)
				{
					doctorDtos.Add(new DoctorDTO
					{
						Id = doctor.Id,
						FirstName = doctor.User.FirstName,
						LastName = doctor.User.LastName,
						Phone = doctor.Phone,
						Email = doctor.User.Email,
						Specialty = doctor.Specialty,
						Availability = doctor.Availability,
					});
				}
				return new ServiceResponse<List<DoctorDTO>>
				{
					Data = doctorDtos,
					IsSuccess = true,
					Message = "Doctors retrieved successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<DoctorDTO>>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<List<DoctorDTO>>> GetByAvailability(DoctorAvailability availability)
		{
			try
			{
				var doctors = await _doctorRepository.GetByAvailability(availability);
				var doctorDtos = new List<DoctorDTO>();

				foreach (var doctor in doctors)
				{
					doctorDtos.Add(new DoctorDTO
					{
						Id = doctor.Id,
						FirstName = doctor.User.FirstName,
						LastName = doctor.User.LastName,
						Phone = doctor.Phone,
						Specialty = doctor.Specialty,
						Availability = doctor.Availability,

					});
				}
				return new ServiceResponse<List<DoctorDTO>>
				{
					Data = doctorDtos,
					IsSuccess = true,
					Message = "Doctors retrieved successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<DoctorDTO>>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<List<DoctorDTO>>> GetBySpecialtyAsync(string specialty)
		{
			try
			{
				var doctors = await _doctorRepository.GetBySpecialtyAsync(specialty);
				var doctorDtos = new List<DoctorDTO>();

				foreach (var doctor in doctors)
				{
					doctorDtos.Add(new DoctorDTO
					{
						Id = doctor.Id,
						FirstName = doctor.User.FirstName,
						LastName = doctor.User.LastName,
						Phone = doctor.Phone,
						Specialty = doctor.Specialty,
						Availability = doctor.Availability,

					});
				}
				return new ServiceResponse<List<DoctorDTO>>
				{
					Data = doctorDtos,
					IsSuccess = true,
					Message = "Doctors retrieved successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<DoctorDTO>>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<DoctorDTO>> GetDoctorByIdAsync(Guid id)
		{
			try
			{
				var doctor = await _doctorRepository.GetByIdAsync(id);
				if (doctor == null)
				{
					return new ServiceResponse<DoctorDTO>
					{
						IsSuccess = false,
						Message = "Doctor not found"
					};
				}
				var doctorDto = new DoctorDTO
				{
					Id = doctor.Id,
					FirstName = doctor.User.FirstName,
					LastName = doctor.User.LastName,
					Phone = doctor.Phone,
					Specialty = doctor.Specialty,
					Availability = doctor.Availability,
				};
				return new ServiceResponse<DoctorDTO>
				{
					Data = doctorDto,
					IsSuccess = true,
					Message = "Doctor retrieved successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<DoctorDTO>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

		public async Task<ServiceResponse<DoctorDTO>> UpdateDoctorAsync(Guid id, DoctorDTO doctorDto)
		{
			try
			{
				var existingDoctor = await _doctorRepository.GetByIdAsync(id);
				if (existingDoctor == null)
				{
					return new ServiceResponse<DoctorDTO>
					{
						IsSuccess = false,
						Message = "Doctor not found"
					};
				}


				existingDoctor.Phone = doctorDto.Phone;
				existingDoctor.Specialty = doctorDto.Specialty;
				existingDoctor.Availability = doctorDto.Availability;
				if (!string.IsNullOrWhiteSpace(doctorDto.Password))
				{
					existingDoctor.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(doctorDto.Password);
				}
				existingDoctor.User.FirstName = doctorDto.FirstName;
				existingDoctor.User.LastName = doctorDto.LastName;
				existingDoctor.User.Email = doctorDto.Email;



				var updatedDoctor = await _doctorRepository.UpdateAsync(existingDoctor);

				var resultDto = new DoctorDTO
				{
					Id = updatedDoctor.Id,
					FirstName = updatedDoctor.User.FirstName,
					LastName = updatedDoctor.User.LastName,
					Email = updatedDoctor.User.Email,
					Phone = updatedDoctor.Phone,
					Specialty = updatedDoctor.Specialty,
					Availability = updatedDoctor.Availability
				};

				return new ServiceResponse<DoctorDTO>
				{
					Data = resultDto,
					IsSuccess = true,
					Message = "Doctor updated successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<DoctorDTO>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}
	}
}
