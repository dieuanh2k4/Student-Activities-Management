# 📚 Student Activities Management System

## 🎯 **Tổng Quan**
Hệ thống quản lý hoạt động sinh viên được xây dựng bằng ASP.NET Core 8.0 với Clean Architecture, bao gồm quản lý học vụ, sự kiện, câu lạc bộ và người dùng.

## 🚀 **Tính Năng Chính**
- ✅ **Academic Management**: Quản lý Khoa, Lớp học, Học kỳ
- ✅ **User Management**: Quản lý người dùng (Admin, Student, Organizer)
- ✅ **Events Management**: Quản lý sự kiện và hoạt động
- ✅ **Clubs Management**: Quản lý câu lạc bộ sinh viên
- ✅ **Database Integration**: PostgreSQL với Entity Framework Core
- ✅ **API Documentation**: Swagger/OpenAPI
- ✅ **Clean Architecture**: Tách biệt rõ ràng các layer

## 🛠️ **Technology Stack**
- **Backend**: ASP.NET Core 8.0 Web API
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Architecture**: Clean Architecture Pattern
- **Documentation**: Swagger UI
- **Development**: VS Code, C# Dev Kit

## ⚙️ **Cài Đặt và Chạy**

### **1. Prerequisites**
```bash
- .NET 8.0 SDK
- PostgreSQL Database
- Git
- VS Code (recommended)
```

### **2. Clone Repository**
```bash
git clone https://github.com/dieuanh2k4/Student-Activities-Management.git
cd Student-Activities-Management
```

### **3. Configuration**
```bash
# Copy và chỉnh sửa file cấu hình
cp appsettings.Example.json appsettings.json

# Cập nhật connection string trong appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StudentActivities;Username=your_username;Password=your_password"
  }
}
```

### **4. Database Setup**
```bash
# Restore packages
dotnet restore

# Update database
dotnet ef database update

# Hoặc tạo migration mới (nếu cần)
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### **5. Run Application**
```bash
# Development mode
dotnet run

# hoặc
dotnet watch run
```

### **6. Access Application**
- **API Server**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **API Documentation**: Available at Swagger endpoint

## 📊 **API Endpoints**

### **Academic Management**
- `GET/POST/PUT/DELETE /api/faculties` - Quản lý khoa
- `GET/POST/PUT/DELETE /api/academicclasses` - Quản lý lớp học
- `GET/POST/PUT/DELETE /api/semesters` - Quản lý học kỳ

### **User Management**
- `GET/POST/PUT/DELETE /api/user` - Quản lý người dùng

### **Events & Clubs**
- `GET/POST/PUT/DELETE /api/events` - Quản lý sự kiện
- `GET/POST/PUT/DELETE /api/clubs` - Quản lý câu lạc bộ

## 📁 **Project Structure**
```
src/
├── Controllers/      # API Controllers
├── Services/         # Business Logic
├── Models/          # Database Entities
├── Dtos/            # Data Transfer Objects
├── Mappers/         # Entity-DTO Mapping
├── Data/            # Database Context
├── Exceptions/      # Custom Exceptions
└── Constant/        # Application Constants
```

## 🔐 **Security Notes**
- File `appsettings.json` chứa thông tin nhạy cảm và đã được exclude khỏi Git
- Copy từ `appsettings.Example.json` và cập nhật với thông tin thật của bạn
- Không commit database credentials hoặc JWT secrets

## 🤝 **Contributing**
1. Fork the repository
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

## 📝 **Documentation**
- [Project Overview](PROJECT_OVERVIEW.md)
- [Academic Management Implementation Guide](IMPLEMENTATION_GUIDE_ACADEMIC_MANAGEMENT.md)
- [Git/GitHub Guide](GIT_GITHUB_GUIDE.md)

## 📞 **Support**
- **Issues**: [GitHub Issues](https://github.com/dieuanh2k4/Student-Activities-Management/issues)
- **Repository**: [GitHub Repository](https://github.com/dieuanh2k4/Student-Activities-Management)