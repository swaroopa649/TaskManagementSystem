using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Respository;
using TaskManagementSystem.Web.API.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Add services to the container
// =====================================================

// Controllers
builder.Services.AddControllers();

// =====================================================
// Database
// =====================================================

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// =====================================================
// Repositories
// =====================================================

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// =====================================================
// Services
// =====================================================

builder.Services.AddScoped<IRoleService, RoleService>();

// =====================================================
// Swagger
// =====================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// =====================================================
// Configure HTTP request pipeline
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();