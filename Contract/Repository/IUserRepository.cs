using HospitalManagementSystem.Entities;

namespace HospitalManagementSystem.Contract.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);        
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid userId);
        Task <User> GetUserByEmailAsync(string email);
    }
}
