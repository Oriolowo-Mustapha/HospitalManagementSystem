using HospitalManagementSystem.DTOs;

namespace HospitalManagementSystem.Interface.Services
{
	public interface IAppointmentService
	{
		Task<ServiceResponse<AppointmentDTO>> GetAppointmentByIdAsync(Guid id);
		Task<ServiceResponse<List<AppointmentDTO>>> GetAllAppointmentsAsync();
		Task<ServiceResponse<List<AppointmentDTO>>> GetAppointmentsByDoctorIdAsync(Guid doctorId);
		Task<ServiceResponse<List<AppointmentDTO>>> GetAppointmentsByPatientIdAsync(Guid patientId);
		Task<ServiceResponse<AppointmentDTO>> CreateAppointmentAsync(AppointmentRequestDto requestDto);
		Task<ServiceResponse<AppointmentDTO>> RescheduleAppointmentAsync(Guid id, AppointmentUpdateDto updateDto);
		Task<ServiceResponse<AppointmentDTO>> CancelAppointmentAsync(Guid id);
		Task<ServiceResponse<AppointmentDTO>> DisapproveAppointmentAsync(Guid id);
		Task<ServiceResponse<AppointmentDTO>> ApproveAppointmentAsync(Guid id);
		Task<ServiceResponse<bool>> DeleteAppointmentAsync(Guid id);
	}
}
