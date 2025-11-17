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
using Minio;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Cấu hình UTC cho toàn bộ ứng dụng
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Activities API", Version = "v1" });

    // Cấu hình JWT cho Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddControllers();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

// Core Services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Academic Management Services
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IAcademicClassService, AcademicClassService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();

// Repository and Auth Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IResgistrationService, ResgistrationService>();

// Checkin Service
builder.Services.AddScoped<ICheckinService, CheckinService>();

// Storage Service
builder.Services.AddScoped<IStorageService, MinioStorageService>();
// Report Service
builder.Services.AddScoped<IReportService, ReportService>();

// Background Services
// Tạm thời tắt để tránh lỗi khi chạy migrations
// Training Score Service
builder.Services.AddScoped<ITrainingScoreService, TrainingScoreService>();

// Background Services
// Đã sửa lỗi DateTime UTC - bật lại Background Service
// builder.Services.AddHostedService<StudentActivities.src.BackgroundServices.EventReminderService>();

// JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "default-secret-key-minimum-32-characters-long");
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

// đăng ký Minio client
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new MinioClient()
        .WithEndpoint(configuration["Minio:Endpoint"])
        .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
        .WithSSL(configuration.GetValue<bool>("Minio:UseSsl"))
        .Build();
});

var app = builder.Build();

// Tự động chạy migrations khi ứng dụng khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi chạy migrations");
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

// Remove HTTPS redirection to avoid warning for development
// app.UseHttpsRedirection();

app.MapControllers();

// Tự động mở Swagger UI khi chạy ứng dụng trong Development
if (app.Environment.IsDevelopment())
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    try
    {
        var url = "http://localhost:5172/swagger";
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch (Exception ex)
    {
        logger.LogWarning($"Không thể tự động mở browser: {ex.Message}");
    }
}

app.Run();

