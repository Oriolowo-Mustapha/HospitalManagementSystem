using HospitalManagementSystem.Contract.Repository;
using HospitalManagementSystem.Contract.Services;
using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Enum;
using HospitalManagementSystem.Repository;

namespace HospitalManagementSystem.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(IAppointmentRepository repo, IDoctorRepository doctorRepository)
        {
            _repo = repo;
            _doctorRepository = doctorRepository;
        }

        public async Task<ServiceResponse<AppointmentDTO>> CreateAsync(AppointmentDTO dto)
        {
            var appointmentTime = dto.AppointmentDate;
            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Doctor not found"
                };
            }

            
            var schedules = doctor.Schedules;
            var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == appointmentTime.DayOfWeek);

            if (daySchedule == null)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Doctor is not available on the selected day"
                };
            }

           
            var appointmentTimeOnly = appointmentTime.TimeOfDay;
            if (appointmentTimeOnly < daySchedule.StartTime || appointmentTimeOnly > daySchedule.EndTime)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = $"Doctor is only available between {daySchedule.StartTime} and {daySchedule.EndTime}"
                };
            }

           
            var existingAppointments = await _repo.GetByDoctorIdAsync(dto.DoctorId);
            var hasConflict = existingAppointments.Any(a =>
                a.AppointmentDate.Date == appointmentTime.Date &&
                a.AppointmentDate.TimeOfDay == appointmentTimeOnly &&
                a.Status != AppointmentStatus.Cancelled
            );

            if (hasConflict)
            {
                return new ServiceResponse<AppointmentDTO>
                {
                    IsSuccess = false,
                    Message = "Doctor already has an appointment at that time"
                };
            }

            // Create appointment
            var appointment = new Appointment
            {
                AppointmentDate = dto.AppointmentDate,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                Notes = dto.Notes,
                Status = AppointmentStatus.Pending
            };

            var created = await _repo.CreateAsync(appointment);
            return new ServiceResponse<AppointmentDTO>
            {
                IsSuccess = true,
                Data = MapToDTO(created),
                Message = "Appointment created successfully"
            };
        }


        public async Task<ServiceResponse<List<AppointmentDTO>>> GetAllAsync()
        {
            var appointments = await _repo.GetAllAsync();
            return new ServiceResponse<List<AppointmentDTO>>
            {
                IsSuccess = true,
                Data = appointments.Select(MapToDTO).ToList()
            };
        }

        public async Task<ServiceResponse<AppointmentDTO>> GetByIdAsync(Guid id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null)
                return new ServiceResponse<AppointmentDTO> { IsSuccess = false, Message = "Not found" };

            return new ServiceResponse<AppointmentDTO>
            {
                IsSuccess = true,
                Data = MapToDTO(appointment)
            };
        }

        public async Task<ServiceResponse<List<AppointmentDTO>>> GetByPatientIdAsync(Guid patientId)
        {
            var appointments = await _repo.GetByPatientIdAsync(patientId);
            return new ServiceResponse<List<AppointmentDTO>>
            {
                IsSuccess = true,
                Data = appointments.Select(MapToDTO).ToList()
            };
        }

        public async Task<ServiceResponse<List<AppointmentDTO>>> GetByDoctorIdAsync(Guid doctorId)
        {
            var appointments = await _repo.GetByDoctorIdAsync(doctorId);
            return new ServiceResponse<List<AppointmentDTO>>
            {
                IsSuccess = true,
                Data = appointments.Select(MapToDTO).ToList()
            };
        }

        public async Task<ServiceResponse<AppointmentDTO>> UpdateAsync(Guid id, AppointmentDTO dto)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null)
                return new ServiceResponse<AppointmentDTO> { IsSuccess = false, Message = "Appointment not found" };

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.Status = dto.Status;
            appointment.Notes = dto.Notes;

            var updated = await _repo.UpdateAsync(appointment);
            return new ServiceResponse<AppointmentDTO>
            {
                IsSuccess = true,
                Data = MapToDTO(updated),
                Message = "Appointment updated"
            };
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(Guid id)
        {
            var success = await _repo.DeleteAsync(id);
            return new ServiceResponse<bool> { IsSuccess = success, Data = success };
        }

        private AppointmentDTO MapToDTO(Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status,
                Notes = appointment.Notes,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.User?.FirstName + " " + appointment.Doctor?.User?.LastName,
                PatientName = appointment.Patient?.User?.FirstName + " " + appointment.Patient?.User?.LastName
            };
        }
    }

}
