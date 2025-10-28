using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using StudentActivities.src.Data;
using StudentActivities.src.Services.Implements;
using StudentActivities.src.Services.Interfaces;
using StudentActivities.src.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserService, UserService>();

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();

