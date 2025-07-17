using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.Entities
{
	public class BaseEntity
	{
		[Key]
		public Guid Id { get; set; } = NewId.Next().ToGuid();
	}
}
