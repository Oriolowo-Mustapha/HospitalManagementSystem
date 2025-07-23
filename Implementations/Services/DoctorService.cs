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
                var doctor = await _doctorRepository.GetByIdAsync(id);
                if (doctor == null)
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Doctor not found"
                    };
                }

                var result = await _doctorRepository.DeleteAsync(id);

                if (result)
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = true,
                        Data = true,
                        Message = "Doctor and associated user deleted successfully"
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Failed to delete doctor and user"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse<List<GetAllDoctors>>> GetAllDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepository.GetAllAsync();

                if (doctors == null || !doctors.Any())
                {
                    return new ServiceResponse<List<GetAllDoctors>>
                    {
                        IsSuccess = false,
                        Message = "No doctors found"
                    };
                }

                var doctorDtos = new List<GetAllDoctors>();

                foreach (var doctor in doctors)
                {
                    doctorDtos.Add(new GetAllDoctors
                    {
                        FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                        Email = doctor.User.Email,
                        Specialty = doctor.Specialty,
                    });
                }

                return new ServiceResponse<List<GetAllDoctors>>
                {
                    Data = doctorDtos,
                    IsSuccess = true,
                    Message = "Doctors retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetAllDoctors>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse<List<GetAllDoctors>>> GetByAvailability(DoctorAvailability availability)
		{
			try
			{
				var doctors = await _doctorRepository.GetByAvailability(availability);
				var doctorDtos = new List<GetAllDoctors>();

				foreach (var doctor in doctors)
				{
					doctorDtos.Add(new GetAllDoctors
                    {
                        FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                        Email = doctor.User.Email,
                        Specialty = doctor.Specialty,

                    });
				}
				return new ServiceResponse<List<GetAllDoctors>>
				{
					Data = doctorDtos,
					IsSuccess = true,
					Message = "Doctors retrieved successfully"
				};
			}
			catch (Exception ex)
			{
				return new ServiceResponse<List<GetAllDoctors>>
				{
					IsSuccess = false,
					Message = $"An error occurred: {ex.Message}"
				};
			}
		}

        public async Task<ServiceResponse<List<GetAllDoctors>>> GetBySpecialtyAsync(string specialty)
        {
            try
            {
                var doctors = await _doctorRepository.GetBySpecialtyAsync(specialty);

                if (doctors == null || !doctors.Any())
                {
                    return new ServiceResponse<List<GetAllDoctors>>
                    {
                        IsSuccess = false,
                        Data = null,
                        Message = $"No doctors found with specialty '{specialty}'."
                    };
                }

                var doctorDtos = new List<GetAllDoctors>();

                foreach (var doctor in doctors)
                {
                    doctorDtos.Add(new GetAllDoctors
                    {
                        FullName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                        Email = doctor.User.Email,
                        Specialty = doctor.Specialty,

                    });
                }

                return new ServiceResponse<List<GetAllDoctors>>
                {
                    Data = doctorDtos,
                    IsSuccess = true,
                    Message = "Doctors retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetAllDoctors>>
                {
                    IsSuccess = false,
                    Data = null,
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
					Availability = doctor.Availability.ToString(),
					Email = doctor.User.Email
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

		public async Task<ServiceResponse<DoctorDTO>> UpdateDoctorAsync(Guid id, UpdateDoctorDTO doctorDto)
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
					Availability = updatedDoctor.Availability.ToString()
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
