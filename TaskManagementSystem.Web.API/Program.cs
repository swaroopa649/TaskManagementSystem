using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Respository;
using TaskManagementSystem.Web.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controller Services
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// 2. Configure Database Context
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured.");

var jwtIssuer = jwtSettings["Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is not configured.");

var jwtAudience = jwtSettings["Audience"]
    ?? throw new InvalidOperationException("JWT Audience is not configured.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),

            ClockSkew = TimeSpan.Zero // Removes default 5-minute clock drift margin
        };
    });

builder.Services.AddAuthorization();

// 4. Register Repositories & Services
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
builder.Services.AddScoped<PasswordService>();

// 5. Configure Swagger / OpenAPI with JWT Security
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TMS Web API",
        Description = "API for Task Management System",
        Contact = new OpenApiContact
        {
            Name = "TMS Team",
            Email = "support@TMS.com"
        }
    });

    // Define Bearer Token Security Scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1Ni...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Apply Security Scheme Globally to Swagger UI
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 6. HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
// Order is important: UseAuthentication MUST come before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();