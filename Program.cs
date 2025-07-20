using HospitalManagementSystem.Data;
using HospitalManagementSystem.Implementations.Repository;
using HospitalManagementSystem.Implementations.Services;
using HospitalManagementSystem.Interface.Repository;
using HospitalManagementSystem.Interface.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HSMDbContext>(options =>
	options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IAppointmentRepository , AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService >();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IBillingService, BillingService>();

// Register the DbContext with MySQL
builder.Services.AddDbContext<HSMDbContext>(options =>
	options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
