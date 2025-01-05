using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersMicroservice.core.Application;
using UsersMicroservice.core.Infrastructure;
using UsersMicroservice.src.auth.application.repositories;
using UsersMicroservice.src.auth.infrastructure.repositories;
using UsersMicroservice.src.department.application.commands.create_department.types;
using UsersMicroservice.src.department.application.repositories;
using UsersMicroservice.src.department.infrastructure.repositories;
using UsersMicroservice.src.department.infrastructure.validators;
using UsersMicroservice.src.user.application.commands.create_user.types;
using UsersMicroservice.src.user.application.repositories;
using UsersMicroservice.src.user.infrastructure.dto;
using UsersMicroservice.src.user.infrastructure.repositories;
using UsersMicroservice.src.user.infrastructure.validators;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MongoDBConfig>();
builder.Services.AddTransient<IValidator<CreateDepartmentCommand>, CreateDepartmentCommandValidator>();
builder.Services.AddScoped<IDepartmentRepository, MongoDepartmentRepository>();
builder.Services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
builder.Services.AddTransient<IValidator<UpdateUserDto>, UpdateUserByIdValidator>();
builder.Services.AddScoped<IUserRepository, MongoUserRepository>();
builder.Services.AddScoped<ICredentialsRepository, MongoCredentialsRepository>();
builder.Services.AddScoped<ICryptoService, BcryptService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")!)),
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreationalUser", policy => policy.RequireClaim("UserRole", ["admin", "provider"]));
    options.AddPolicy("AdminUser", policy => policy.RequireClaim("UserRole", "admin"));
});
builder.Services.AddScoped<ITokenAuthenticationService, JwtService>();

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SMTP"));
builder.Services.AddTransient<EmailSenderService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
