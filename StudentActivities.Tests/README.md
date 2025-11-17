# Student Activities - Unit Tests

## ✅ Test Project đã được setup

### 📦 Cấu trúc:

```
StudentActivities.Tests/
├── StudentActivities.Tests.csproj  (xUnit test project)
├── Controllers/                     (Test files sẽ ở đây)
└── README.md
```

### 🔧 Packages đã cài đặt:

- ✅ **xUnit** - Testing framework
- ✅ **Moq** - Mocking framework
- ✅ **FluentAssertions** - Readable assertions
- ✅ **Microsoft.AspNetCore.Mvc.Testing** - Integration testing

---

## 🚀 Cách sử dụng

### 1. Tạo test file mới

Tạo file trong `StudentActivities.Tests/Controllers/`:

```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StudentActivities.src.Controllers;
using StudentActivities.src.Services.Interfaces;

namespace StudentActivities.Tests.Controllers
{
    public class YourControllerTests
    {
        [Fact]
        public void TestMethod_Scenario_ExpectedResult()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
```

### 2. Chạy tests

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với chi tiết
dotnet test --logger "console;verbosity=detailed"

# Chạy tests với code coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📝 Template tests đã chuẩn bị

Tôi đã tạo sẵn 3 test files mẫu trong thư mục `Controllers/`:

1. **AuthControllerTests.cs** - Test authentication logic

   - Login với valid credentials
   - Login với invalid credentials
   - Null/empty input validation

2. **EventsControllerTests.cs** - Test CRUD operations

   - GetAll events
   - GetById valid/invalid
   - Update event
   - Delete event

3. **CheckinControllerTests.cs** - Test check-in logic
   - Checkin với valid data
   - Duplicate checkin prevention
   - Get checkins by event/student

**⚠️ Lưu ý:** Các test files này sẽ cần điều chỉnh theo DTOs và interfaces thực tế của bạn.

---

## 🔨 Jenkinsfile Integration

Jenkinsfile đã có stage Test:

```groovy
stage('Test') {
    steps {
        bat 'dotnet test --no-build --configuration Release'
    }
}
```

**Khi chưa có tests:**

- Jenkins sẽ skip stage này (0 tests discovered)
- Build vẫn PASS

**Khi đã có tests:**

- Jenkins tự động run tất cả tests
- Nếu có test FAIL → Build FAIL → Deploy bị block

---

## 📚 Best Practices

### 1. **Test Naming Convention**

```
MethodName_Scenario_ExpectedBehavior()
```

Ví dụ:

- `Login_WithValidCredentials_ReturnsOkWithToken()`
- `GetById_WithInvalidId_ReturnsNotFound()`

### 2. **AAA Pattern**

```csharp
[Fact]
public void Method_Scenario_Expected()
{
    // Arrange - Setup test data & mocks
    var mock = new Mock<IService>();

    // Act - Execute the method
    var result = controller.Method();

    // Assert - Verify expectations
    result.Should().BeOfType<OkObjectResult>();
}
```

### 3. **Mock Dependencies**

```csharp
var mockService = new Mock<IAuthService>();
mockService
    .Setup(s => s.LoginAsync(It.IsAny<LoginRequestDto>()))
    .ReturnsAsync(expectedResponse);
```

### 4. **Readable Assertions**

```csharp
// FluentAssertions style
result.Should().BeOfType<OkObjectResult>();
result.Should().NotBeNull();
list.Should().HaveCount(2);
user.Name.Should().Be("Expected Name");
```

---

## 🎯 Những gì NÊN test (Priority)

### ✅ **HIGH Priority:**

- Authentication & Authorization logic
- Business rules & validations
- Critical API endpoints (checkin, scoring)
- Data transformations

### ⚠️ **MEDIUM Priority:**

- CRUD operations
- Error handling
- Edge cases

### ❌ **LOW Priority (có thể skip):**

- Simple DTOs/Models
- Mappers
- Configuration classes

---

## 📊 Code Coverage

Sau khi có tests, bạn có thể check coverage:

```bash
# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"

# Install report generator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generate HTML report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

# Open report
start coveragereport/index.html
```

---

## 🆘 Troubleshooting

### Lỗi: "The type or namespace name 'Xunit' could not be found"

**Giải pháp:**

```bash
cd StudentActivities.Tests
dotnet restore
dotnet build
```

### Lỗi: "Mock<> could not be found"

**Giải pháp:**

```bash
dotnet add package Moq
```

### Tests không chạy trong Jenkins

**Kiểm tra:**

1. Test project có reference đến main project?
2. Stage Test trong Jenkinsfile có đúng path?
3. Build main project trước khi run tests?

---

## 📖 Tài liệu tham khảo

- [xUnit Documentation](https://xunit.net/)
- [Moq Quickstart](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [.NET Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

---

## ✨ Next Steps

1. **Bây giờ (Optional):** Uncomment và fix các test files mẫu
2. **Khi cần:** Viết tests cho business logic quan trọng nhất
3. **Sau này:** Tăng code coverage dần dần
4. **Production:** Đảm bảo critical paths đều có tests

**Không bắt buộc phải có 100% coverage ngay!** Bắt đầu với những phần quan trọng nhất.
