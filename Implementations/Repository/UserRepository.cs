using HospitalManagementSystem.Data;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Implementations.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly HSMDbContext _hSMDbContext;

		public UserRepository(HSMDbContext hSMDbContext)
		{
			_hSMDbContext = hSMDbContext;
		}

		public async Task<User> CreateUserAsync(User user)
		{
			var createdUser = _hSMDbContext.Users.Add(user);
			await _hSMDbContext.SaveChangesAsync();
			return createdUser.Entity;
		}

		public Task DeleteUserAsync(Guid userId)
		{
			var user = _hSMDbContext.Users.Find(userId);
			if (user == null)
			{
				throw new KeyNotFoundException("User not found");

			}
			else
			{
				_hSMDbContext.Users.Remove(user);
				return _hSMDbContext.SaveChangesAsync();
			}

		}
		public async Task<User> GetUserByEmailAsync(string email)
		{
			return await _hSMDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
		}

		public async Task<User> GetUserByFirstNameAsync(string FirstName)
		{
			return await _hSMDbContext.Users.FirstOrDefaultAsync(u => u.FirstName == FirstName);
		}

		public Task<User> GetUserByIdAsync(Guid userId)
		{
			var user = _hSMDbContext.Users.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				throw new KeyNotFoundException("User not found");
			}
			return Task.FromResult(user);
		}

		public async Task<User> GetUserByLastNameAsync(string LastName)
		{
			return await _hSMDbContext.Users.FirstOrDefaultAsync(u => u.LastName == LastName);
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _hSMDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
		}

		public Task<User> UpdateUserAsync(User user)
		{
			var existingUser = _hSMDbContext.Users.Find(user.Id);
			if (existingUser == null)
			{
				throw new KeyNotFoundException("User not found");
			}
			//existingUser.Username = user.Username;
			//existingUser.PasswordHash = user.PasswordHash;
			//existingUser.Role = user.Role;
			_hSMDbContext.Users.Update(existingUser);
			_hSMDbContext.SaveChanges();
			return Task.FromResult(existingUser);
		}
	}
}
