using HospitalManagementSystem.DTOs;
using HospitalManagementSystem.Entities;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagementSystem.Implementations.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPatientRepository _patientRepository;
		private readonly IDoctorRepository _doctorRepository;
		private readonly ILogger<UserService> _logger;
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;


		public UserService(IUserRepository userRepository, IPatientRepository patient, ILogger<UserService> logger, IConfiguration configuration,
			HttpClient httpClient, IDoctorRepository doctorRepository)
		{
			_userRepository = userRepository;
			_patientRepository = patient;
			_logger = logger;
			_configuration = configuration;
			_httpClient = httpClient;
			_doctorRepository = doctorRepository;
		}

		public async Task<ServiceResponse<AuthResponseModel>> SignUpAsync(RegisterPatientRequestDto model)
		{
			try
			{
				var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
				if (existingUser != null)
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "User already exists with this email"
					};
				}

				// Create user
				var user = new User
				{
					Id = Guid.NewGuid(),
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Username = model.Username,
					PasswordHash = HashPassword(model.Password),
					Role = model.Role,
					IsActive = true,
					CreatedAt = DateTime.UtcNow
				};

				var createdUser = await _userRepository.CreateUserAsync(user);

				if (createdUser == null)
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "Failed to create user"
					};
				}
				if (model.Role == "Patient")
				{
					var patient = new Patient
					{
						Id = Guid.NewGuid(),
						Phone = model.Phone,
						InsuranceProvider = model.InsuranceProvider,
						InsuranceDiscount = model.InsuranceDiscount,
						DateOfBirth = model.DateOfBirth,
						UserId = createdUser.Id,
					};

					await _patientRepository.CreatePatientAsync(patient);
				}

				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = true,
					Data = new AuthResponseModel
					{
						UserId = createdUser.Id,
						Email = createdUser.Email,
						Username = createdUser.Username,
						FirstName = model.FirstName,
						LastName = model.LastName,
						Token = GenerateJwtToken(createdUser)
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during sign up");
				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = false,
					Message = "An error occurred during sign up"
				};
			}
		}


		public async Task<ServiceResponse<AuthResponseModel>> LoginAsync(LoginRequestDto model)
		{
			try
			{
				var user = await _userRepository.GetUserByUsernameAsync(model.Username);
				if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "Invalid email or password"
					};
				}

				if (!user.IsActive)
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "Account is deactivated"
					};
				}

				// Update last login
				user.LastLogin = DateTime.UtcNow;
				await _userRepository.UpdateUserAsync(user);

				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = true,
					Data = new AuthResponseModel
					{
						UserId = user.Id,
						Email = user.Email,
						Username = user.Username,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Token = GenerateJwtToken(user)
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during login");
				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = false,
					Message = "An error occurred during login"
				};
			}
		}

		private string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		private bool VerifyPassword(string password, string hashedPassword)
		{
			if (string.IsNullOrEmpty(hashedPassword))
				return false;

			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}

		private string GenerateJwtToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
			new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
			new Claim("username", user.Username ?? string.Empty),

		};

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddDays(30),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<ServiceResponse<AuthResponseModel>> AddDoctorAsync(AddDoctorRequestDto model)
		{
			try
			{
				var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
				if (existingUser != null)
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "User already exists with this email"
					};
				}

				// Create user
				var user = new User
				{
					Id = Guid.NewGuid(),
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Username = model.Username,
					PasswordHash = HashPassword(model.Password),
					Role = model.Role,
					IsActive = true,
					CreatedAt = DateTime.UtcNow
				};

				var createdUser = await _userRepository.CreateUserAsync(user);

				if (createdUser == null)
				{
					return new ServiceResponse<AuthResponseModel>
					{
						IsSuccess = false,
						Message = "Failed to create doctor"
					};
				}
				if (model.Role == "Doctor")
				{
					var doctor = new Doctor
					{
						Id = Guid.NewGuid(),
						Phone = model.Phone,
						Specialty = model.Specialty,
						Availability = model.Availability,
						UserId = createdUser.Id,
					};

					await _doctorRepository.CreateAsync(doctor);
				}

				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = true,
					Data = new AuthResponseModel
					{
						UserId = createdUser.Id,
						Email = createdUser.Email,
						Username = createdUser.Username,
						FirstName = model.FirstName,
						LastName = model.LastName,
						Token = GenerateJwtToken(createdUser)
					}
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during sign up");
				return new ServiceResponse<AuthResponseModel>
				{
					IsSuccess = false,
					Message = "An error occurred during sign up"
				};
			}
		}
	}
}
