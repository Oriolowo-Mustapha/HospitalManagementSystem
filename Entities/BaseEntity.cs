using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Entities
{
	public class BaseEntity
	{
		[Key]
		public Guid Id { get; set; } = NewId.Next().ToGuid();
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime LastLogin { get; set; }= DateTime.UtcNow;
    }
}
