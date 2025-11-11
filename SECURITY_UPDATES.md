# 🔐 Cập nhật bảo mật Authentication

## Ngày cập nhật: 11/11/2025

### ✅ Các thay đổi đã thực hiện

#### 1. **Hash Password với BCrypt** 
- ✅ Cài đặt package `BCrypt.Net-Next`
- ✅ Password được hash trước khi lưu vào database
- ✅ Xác thực login dùng `BCrypt.Verify()` thay vì so sánh plaintext
- ✅ **Giữ nguyên** tên property `Password` trong model `Users.cs`

**Files đã sửa:**
- `src/Mappers/UserMapper.cs` - Hash password khi tạo user mới
- `src/Services/Implements/UserService.cs` - Hash password khi update user
- `src/Services/Implements/AuthService.cs` - Verify password với BCrypt

#### 2. **Tách JWT Token Generation ra service riêng**
- ✅ Tạo `IJwtTokenService` interface
- ✅ Tạo `JwtTokenService` implementation
- ✅ Follow Single Responsibility Principle
- ✅ Dễ maintain, test và mở rộng sau này

**Files mới:**
- `src/Services/Interfaces/IJwtTokenService.cs`
- `src/Services/Implements/JwtTokenService.cs`

#### 3. **Cải thiện JWT Token**
- ✅ Sử dụng `DateTime.UtcNow` thay vì `DateTime.Now` (tránh vấn đề timezone)
- ✅ Thêm claim `Jti` (JWT ID) - unique token identifier
- ✅ Thêm claim `Sub` (Subject) - chứa User ID
- ✅ Thêm claim `Iat` (Issued At) - thời điểm phát token
- ✅ Validate JWT configuration khi startup
- ✅ Logging cho các sự kiện authentication

**Files đã sửa:**
- `src/Services/Implements/AuthService.cs` - Refactor để dùng JwtTokenService
- `Program.cs` - Đăng ký JwtTokenService trong DI container

---

## 📝 Cấu trúc Claims trong JWT Token

Token hiện tại chứa các claims sau:

```json
{
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "student01",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Student",
  "jti": "unique-guid-here",
  "sub": "123",  // User ID
  "iat": "1699660800"
}
```

---

## 🔄 Migration dữ liệu cũ (nếu cần)

**Nếu database đã có users với plaintext password:**

### Option 1: Reset tất cả passwords (khuyến nghị cho development)
```sql
-- Đặt lại password mặc định cho tất cả users (ví dụ: "Password@123")
UPDATE "Users" 
SET "Password" = '$2a$11$example-bcrypt-hash-here';
```

### Option 2: Yêu cầu users đổi password lần đầu login
- Thêm flag `RequirePasswordChange` vào model Users
- Khi login thành công lần đầu → bắt buộc đổi password

---

## ⚠️ Lưu ý quan trọng

### 1. **Không thể recover password gốc**
- Sau khi hash, KHÔNG thể xem lại password gốc
- Chức năng "Quên mật khẩu" phải reset password, không thể "gửi lại password cũ"

### 2. **Testing**
Khi test API, lưu ý:
- **Tạo user mới**: Password sẽ tự động được hash
- **Login**: Gửi password plaintext, server sẽ verify với hash
- **Ví dụ**:
  ```json
  // POST /api/user/create-user
  {
    "userName": "testuser",
    "password": "MyPassword123",  // Gửi plaintext
    "role": "Student"
  }
  
  // POST /api/auth/login
  {
    "userName": "testuser",
    "password": "MyPassword123"   // Gửi plaintext
  }
  ```

### 3. **JWT Secret Key**
- Hiện tại lưu trong `appsettings.json` → OK cho development
- **Production**: Nên chuyển sang Environment Variables hoặc Azure Key Vault
- Key nên dài ít nhất 32 ký tự

---

## 🎯 Các cải tiến tiếp theo (Optional)

### Phase 2 - Medium Priority:
- [ ] Implement Account Lockout (khóa tài khoản sau N lần login sai)
- [ ] Add Rate Limiting cho endpoint `/api/auth/login`
- [ ] Password strength validation (min 8 ký tự, chữ hoa, số, ký tự đặc biệt)
- [ ] Audit logging (log mọi login attempt)

### Phase 3 - Advanced:
- [ ] Refresh Token pattern (token dài hạn để refresh access token)
- [ ] Token Blacklist/Revocation (logout thật sự)
- [ ] Two-Factor Authentication (2FA)
- [ ] Password reset qua email

---

## 🧪 Test Cases cần kiểm tra

- [x] ✅ Build project thành công
- [ ] Tạo user mới → password được hash
- [ ] Login với password đúng → trả token
- [ ] Login với password sai → trả Unauthorized
- [ ] Token chứa đầy đủ claims (Name, Role, Jti, Sub)
- [ ] Token expire đúng thời gian
- [ ] Các endpoint có `[Authorize]` hoạt động đúng

---

## 📞 Hỗ trợ

Nếu có vấn đề:
1. Check logs trong console khi run project
2. Verify JWT configuration trong `appsettings.json`
3. Test với Postman/Swagger
4. Review code trong các files đã sửa ở trên

---

**Tóm tắt**: Hệ thống authentication giờ đã **AN TOÀN HỠN** với password hashing và JWT token được tách riêng, dễ quản lý! 🎉
