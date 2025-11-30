# 🏷️ Hướng dẫn cấu hình Hostname cho Student Activities System

## 📖 Tổng quan

Sử dụng **hostname** thay vì **IP address** mang lại nhiều lợi ích:

- ✅ Không cần cập nhật cấu hình khi IP thay đổi
- ✅ Dễ nhớ và quản lý hơn
- ✅ Phù hợp với môi trường có DHCP
- ✅ Professional hơn trong môi trường production

## 🎯 Kiến trúc

```
┌─────────────────────────────────────────────────────────┐
│                    Your Network                         │
│                                                         │
│  ┌──────────────┐      ┌──────────────┐               │
│  │   Jenkins    │─────▶│  Server      │               │
│  │   Machine    │      │  (Registry)  │               │
│  │              │      │              │               │
│  │ Uses:        │      │ Hostname:    │               │
│  │ hostname:    │      │ MY-SERVER    │               │
│  │ 5443         │      │              │               │
│  └──────────────┘      └──────────────┘               │
│                              │                          │
│                              │                          │
│  ┌──────────────────────────▼─────────────┐           │
│  │        DNS Server / Hosts File         │           │
│  │  Maps: MY-SERVER → 192.168.102.3       │           │
│  └────────────────────────────────────────┘           │
└─────────────────────────────────────────────────────────┘
```

## 🚀 Setup nhanh

### Bước 1: Chạy script setup (với quyền Admin)

```powershell
# Tự động phát hiện hostname
.\setup-hostname.ps1

# Hoặc chỉ định hostname cụ thể
.\setup-hostname.ps1 -Hostname "MY-SERVER"
```

### Bước 2: Cấu hình hostname resolution

#### Option A: Sử dụng hosts file (đơn giản, cho mạng nhỏ)

```powershell
# Trên MỖI máy trong mạng, chạy với quyền Admin:
.\update-hosts-file.ps1 -Hostname "MY-SERVER" -IPAddress "192.168.102.3"
```

#### Option B: Sử dụng DNS Server (khuyến nghị cho mạng lớn)

Xem phần [Cấu hình DNS Server](#cấu-hình-dns-server) bên dưới

### Bước 3: Cấu hình Jenkins

```
1. Vào Jenkins → Manage Jenkins → Configure System
2. Tìm "Global properties" → Check "Environment variables"
3. Thêm biến:
   Name:  DEPLOY_HOSTNAME
   Value: MY-SERVER (hostname của server)
4. Save
```

### Bước 4: Restart services

```powershell
# Restart Docker
Restart-Service docker

# Restart Docker Registry
docker restart registry
```

### Bước 5: Verify

```powershell
# Test hostname resolution
ping MY-SERVER

# Test Docker Registry
docker login MY-SERVER:5443
```

## 📁 Files đã được cập nhật

### 1. Jenkinsfile

```groovy
environment {
    // Tự động sử dụng hostname từ biến môi trường
    DEPLOY_SERVER = "${env.DEPLOY_HOSTNAME ?: env.COMPUTERNAME ?: 'localhost'}"
    REGISTRY_URL = "${DEPLOY_SERVER}:5443"
}
```

**Ưu tiên:**

1. `DEPLOY_HOSTNAME` - Biến môi trường trong Jenkins
2. `COMPUTERNAME` - Hostname của Jenkins agent
3. `localhost` - Fallback

### 2. appsettings.json

```json
{
  "Minio": {
    "PublicEndpoint": "${HOSTNAME:localhost}:9000"
  }
}
```

Sẽ đọc từ biến môi trường `HOSTNAME` trong container

### 3. .env files

```env
# StudentActivies/.env
HOSTNAME=MY-SERVER

# .env.production
HOSTNAME=MY-SERVER
```

## 🔧 Cấu hình chi tiết

### Cấu hình DNS Server

#### Trên Windows Server:

1. **Install DNS Server Role**

```powershell
Install-WindowsFeature -Name DNS -IncludeManagementTools
```

2. **Create Forward Lookup Zone**

```powershell
# Open DNS Manager
dnsmgmt.msc

# Create new zone:
# - Zone type: Primary zone
# - Zone name: yourdomain.local (hoặc tên khác)
# - Dynamic updates: Allow
```

3. **Add Host Record**

```
# In DNS Manager:
# Right-click zone → New Host (A or AAAA)
# - Name: MY-SERVER
# - IP Address: 192.168.102.3
# - Create PTR record: Yes
```

4. **Configure DHCP to use this DNS**

```powershell
# In DHCP Manager, set DNS server option
Set-DhcpServerv4OptionValue -DnsServer 192.168.102.3
```

#### Trên Linux (dnsmasq):

```bash
# Install dnsmasq
sudo apt-get install dnsmasq

# Edit /etc/dnsmasq.conf
echo "address=/MY-SERVER/192.168.102.3" | sudo tee -a /etc/dnsmasq.conf

# Restart service
sudo systemctl restart dnsmasq
```

#### Trên Router (nếu hỗ trợ):

1. Đăng nhập vào router web interface
2. Tìm **DNS Settings** hoặc **Static DNS**
3. Thêm entry: `MY-SERVER → 192.168.102.3`
4. Save và reboot router

### Cấu hình hosts file trên các máy

**Trên mỗi máy Windows:**

```powershell
# Với quyền Administrator
.\update-hosts-file.ps1 -Hostname "MY-SERVER" -IPAddress "192.168.102.3"
```

**Hoặc chỉnh sửa thủ công:**

```powershell
# Mở Notepad với quyền Admin
notepad C:\Windows\System32\drivers\etc\hosts

# Thêm dòng:
192.168.102.3    MY-SERVER
```

**Trên Linux/Mac:**

```bash
sudo nano /etc/hosts

# Thêm:
192.168.102.3    MY-SERVER
```

### Cấu hình Docker daemon.json

File này đã được script `setup-hostname.ps1` tự động cập nhật:

```json
{
  "insecure-registries": ["MY-SERVER:5443"]
}
```

**Vị trí file:**

- Windows: `C:\ProgramData\docker\config\daemon.json`
- Linux: `/etc/docker/daemon.json`

**Sau khi sửa, restart Docker:**

```powershell
# Windows
Restart-Service docker

# Linux
sudo systemctl restart docker
```

## 📋 Checklist Setup

### Trên Server (chạy Docker Registry + Application):

- [ ] Chạy `setup-hostname.ps1` với quyền Admin
- [ ] Verify daemon.json có hostname đúng
- [ ] Restart Docker service
- [ ] Restart Docker Registry container
- [ ] Test: `docker login <HOSTNAME>:5443`

### Trên Jenkins Server:

- [ ] Cấu hình biến môi trường `DEPLOY_HOSTNAME`
- [ ] Hoặc đảm bảo `COMPUTERNAME` trả về hostname đúng
- [ ] Update Docker daemon.json với hostname
- [ ] Restart Docker service
- [ ] Test: `docker login <HOSTNAME>:5443`
- [ ] Trigger một Jenkins build test

### Trên tất cả các máy khác:

- [ ] Update hosts file HOẶC cấu hình DNS
- [ ] Test: `ping <HOSTNAME>`
- [ ] Test: `nslookup <HOSTNAME>`
- [ ] Flush DNS cache: `ipconfig /flushdns`

## 🔍 Troubleshooting

### ❌ Lỗi: "Could not resolve hostname"

**Nguyên nhân:** Hostname chưa được cấu hình trong DNS/hosts file

**Giải pháp:**

```powershell
# Kiểm tra hostname resolution
nslookup MY-SERVER

# Nếu thất bại, update hosts file
.\update-hosts-file.ps1 -Hostname "MY-SERVER" -IPAddress "192.168.102.3"

# Flush DNS cache
ipconfig /flushdns

# Test lại
ping MY-SERVER
```

### ❌ Lỗi: "Cannot connect to Docker Registry"

**Kiểm tra:**

```powershell
# 1. Hostname có resolve không?
ping MY-SERVER

# 2. Port 5443 có mở không?
Test-NetConnection MY-SERVER -Port 5443

# 3. daemon.json có đúng không?
Get-Content C:\ProgramData\docker\config\daemon.json
# Phải có: "insecure-registries": ["MY-SERVER:5443"]

# 4. Docker service đã restart chưa?
Restart-Service docker

# 5. Registry container có chạy không?
docker ps | Select-String registry
```

### ❌ Jenkins build thất bại

**Kiểm tra:**

```powershell
# 1. Biến môi trường có đúng không?
# Trong Jenkins console output, tìm:
echo "DEPLOY_SERVER: ${DEPLOY_SERVER}"

# 2. Jenkins agent có resolve hostname không?
# SSH vào Jenkins agent:
ping MY-SERVER
nslookup MY-SERVER

# 3. daemon.json trên Jenkins agent
# Kiểm tra và update nếu cần
```

### ⚠️ Hostname resolve sai IP

**Nguyên nhân:** DNS cache hoặc multiple entries

**Giải pháp:**

```powershell
# 1. Flush DNS cache
ipconfig /flushdns

# 2. Kiểm tra hosts file
Get-Content C:\Windows\System32\drivers\etc\hosts | Select-String "MY-SERVER"

# 3. Xóa các entry trùng lặp
notepad C:\Windows\System32\drivers\etc\hosts

# 4. Test lại
nslookup MY-SERVER
```

## 🎓 Best Practices

### 1. Naming Convention

```
Sử dụng hostname có ý nghĩa:
✅ GOOD: SERVER-PROD, SERVER-DEV, SERVER-TEST
❌ BAD: SERVER1, MYPC, DESKTOP-X7H3K

Tránh:
- Hostname quá dài (>15 ký tự cho NetBIOS)
- Ký tự đặc biệt (chỉ dùng chữ, số, gạch ngang)
- Hostname trùng với các service khác
```

### 2. Documentation

```markdown
Tạo bảng mapping trong docs:
| Hostname | IP Address | Role |
|---------------|----------------|-----------------------|
| SERVER-PROD | 192.168.102.3 | Production Server |
| SERVER-DEV | 192.168.102.4 | Development Server |
| JENKINS-CI | 192.168.102.5 | Jenkins CI/CD |
```

### 3. DNS vs Hosts File

**Sử dụng DNS khi:**

- ✅ Có nhiều máy trong mạng (>5 máy)
- ✅ IP thường xuyên thay đổi
- ✅ Có Windows Server hoặc Linux server
- ✅ Muốn quản lý tập trung

**Sử dụng Hosts File khi:**

- ✅ Mạng nhỏ (<5 máy)
- ✅ IP ổn định
- ✅ Không có DNS server
- ✅ Setup nhanh, đơn giản

### 4. Security

```
1. Đặt hostname không tiết lộ thông tin nhạy cảm
2. Sử dụng internal domain (.local, .internal)
3. Không expose hostname ra internet
4. Regular audit DNS/hosts entries
```

## 🔄 Khi hostname thay đổi

Nếu cần đổi hostname:

```powershell
# 1. Update hostname trên Windows
Rename-Computer -NewName "NEW-HOSTNAME" -Restart

# 2. Sau khi reboot, chạy setup lại
.\setup-hostname.ps1

# 3. Update hosts file trên tất cả máy
.\update-hosts-file.ps1 -Hostname "NEW-HOSTNAME" -IPAddress "<IP>"

# 4. Update Jenkins environment variable
# Trong Jenkins UI: DEPLOY_HOSTNAME = NEW-HOSTNAME

# 5. Restart Docker & Registry
Restart-Service docker
docker restart registry
```

## 📊 Kiểm tra cấu hình

### Script kiểm tra toàn bộ

```powershell
Write-Host "=== Hostname Configuration Check ===" -ForegroundColor Cyan

# 1. Hostname
Write-Host "`n[1] Computer Hostname"
Write-Host "  COMPUTERNAME: $env:COMPUTERNAME"
Write-Host "  DNS Name: $([System.Net.Dns]::GetHostName())"

# 2. Hosts file
Write-Host "`n[2] Hosts File Entries"
Get-Content C:\Windows\System32\drivers\etc\hosts |
    Where-Object { $_ -notmatch "^\s*#" -and $_ -match "\w" } |
    ForEach-Object { Write-Host "  $_" }

# 3. Docker daemon
Write-Host "`n[3] Docker daemon.json"
if (Test-Path "C:\ProgramData\docker\config\daemon.json") {
    Get-Content "C:\ProgramData\docker\config\daemon.json"
} else {
    Write-Host "  Not configured" -ForegroundColor Yellow
}

# 4. Environment variables
Write-Host "`n[4] Environment Variables"
Write-Host "  HOSTNAME: $env:HOSTNAME"
Write-Host "  DEPLOY_HOSTNAME: $env:DEPLOY_HOSTNAME"

# 5. Network test
Write-Host "`n[5] Network Tests"
$hostname = $env:COMPUTERNAME
Write-Host "  Testing: $hostname"
Test-NetConnection $hostname -WarningAction SilentlyContinue |
    Select-Object ComputerName, PingSucceeded |
    Format-Table
```

## 📞 Quick Commands

```powershell
# Lấy hostname hiện tại
.\get-hostname.ps1

# Setup hostname cho hệ thống
.\setup-hostname.ps1

# Update hosts file
.\update-hosts-file.ps1 -Hostname "MY-SERVER" -IPAddress "192.168.102.3"

# Test hostname resolution
nslookup MY-SERVER
ping MY-SERVER

# Test Docker Registry
docker login MY-SERVER:5443

# Flush DNS cache
ipconfig /flushdns

# View hosts file
notepad C:\Windows\System32\drivers\etc\hosts

# View daemon.json
notepad C:\ProgramData\docker\config\daemon.json
```

## 🎯 Summary

**Ưu điểm của hostname:**

- ✅ Không lo IP thay đổi
- ✅ Dễ nhớ, dễ quản lý
- ✅ Professional
- ✅ Scale tốt hơn

**Lưu ý:**

- ⚠️ Cần cấu hình DNS hoặc hosts file trên tất cả máy
- ⚠️ Hostname phải unique trong mạng
- ⚠️ Cần document mapping hostname ↔ IP

**Workflow:**

1. Setup hostname trên server
2. Cấu hình DNS/hosts trên tất cả máy
3. Update Jenkins environment variable
4. Test kỹ trước khi deploy production
