using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Repositories.Implements;
using StudentActivities.src.Repositories.Interfaces;
using StudentActivities.src.Services.Implements;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Core Services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IUserService, UserService>();

// Academic Management Services
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IAcademicClassService, AcademicClassService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

// Repository and Auth Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStudentService, StudentService>();

// JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Cấu hình Cloudinary
builder.Services.Configure<CloudinarySetting>(
    builder.Configuration.GetSection("CloudinarySettings")
);

// Cấu hình Sanitizer
builder.Services.AddSingleton<IHtmlSanitizer>(provider =>
{
    var sanitizer = new HtmlSanitizer();

    // xóa hết các thẻ mặc định
    sanitizer.AllowedTags.Clear();

    // Cấu hình các thuộc tính được phép
    sanitizer.AllowedAttributes.Add("href");
    sanitizer.AllowedAttributes.Add("target");

    // chỉ cho phép các schema an toàn 
    sanitizer.AllowedSchemes.Add("http");
    sanitizer.AllowedSchemes.Add("https");
    sanitizer.AllowedSchemes.Add("mailto");

    return sanitizer;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Force development environment to show Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

// Remove HTTPS redirection to avoid warning for development
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();

