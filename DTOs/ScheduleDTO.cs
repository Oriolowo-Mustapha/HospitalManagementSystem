using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.DTOs
{
	public class ScheduleDTO
	{
		[Required]
		public DateTime Date { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }

		public int DailyAppointmentLimit { get; set; }
	}
}
