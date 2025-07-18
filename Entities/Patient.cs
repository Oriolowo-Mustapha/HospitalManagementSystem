﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementSystem.Entities
{
	public class Patient : BaseEntity
	{

		[Required]
		public DateTime DateOfBirth { get; set; }

		[StringLength(11)]
		public string Phone { get; set; }

		public string InsuranceProvider { get; set; }

		public decimal InsuranceDiscount { get; set; }

		[ForeignKey("User")]
		public Guid? UserId { get; set; }

		public User User { get; set; }

		public List<Appointment> Appointments { get; set; }
		public List<Billing> Billings { get; set; }
	}
}
