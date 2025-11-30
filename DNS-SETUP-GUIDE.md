# 🌐 Hướng dẫn Setup DNS Server cho Student Activities

## 📊 Thông tin hệ thống của bạn

- **OS**: Windows 10 Home
- **Hostname**: DESKTOP-QEDCEJ1
- **IP**: 192.168.102.3

---

## ⚠️ LƯU Ý QUAN TRỌNG

Windows 10 Home **KHÔNG** hỗ trợ DNS Server role (chỉ có Windows Server hoặc Pro).

**Bạn có 3 lựa chọn:**

---

## 🎯 OPTION 1: Sử dụng Router DNS (KHUYẾN NGHỊ - ĐƠN GIẢN NHẤT)

### Ưu điểm:

✅ Không cần cài đặt gì thêm  
✅ Tất cả máy trong mạng tự động dùng  
✅ Phù hợp với router hiện đại (TP-Link, Asus, Netgear, etc.)

### Các bước thực hiện:

#### Bước 1: Xác định IP Gateway (Router)

```powershell
# Chạy lệnh này để tìm IP router
ipconfig | Select-String "Default Gateway"
# Thường là: 192.168.1.1 hoặc 192.168.0.1 hoặc 192.168.102.1
```

#### Bước 2: Đăng nhập Router

1. Mở trình duyệt, truy cập: `http://192.168.102.1` (hoặc IP gateway của bạn)
2. Login với tài khoản admin
   - Username thường là: `admin`
   - Password: kiểm tra dưới đáy router hoặc `admin`/`password`

#### Bước 3: Tìm DNS Settings

**Với Router TP-Link:**

```
Advanced → Network → DHCP Server → Address Reservation
hoặc
Advanced → Network → DNS → Static DNS
```

**Với Router Asus:**

```
LAN → DHCP Server → DNS and WINS Server Setting
hoặc
WAN → DNS Settings
```

**Với Router D-Link:**

```
Setup → Network Settings → Add DHCP Reservation
```

#### Bước 4: Thêm DNS Entry

**Nếu router hỗ trợ "Static DNS" hoặc "Local DNS":**

- Hostname: `DESKTOP-QEDCEJ1`
- IP Address: `192.168.102.3`
- Save/Apply

**Nếu không có, dùng DHCP Reservation + hosts file:**

- Chỉ cần đặt IP tĩnh cho server
- Các máy khác dùng hosts file (Option 3)

#### Bước 5: Cấu hình DHCP DNS

Đảm bảo DHCP Server của router đang phát:

- Primary DNS: IP của router (192.168.102.1)
- Hoặc Primary DNS: 8.8.8.8, Secondary DNS: IP router

#### Bước 6: Test

```powershell
# Trên các máy client, chạy:
ipconfig /release
ipconfig /renew
ipconfig /flushdns

# Test hostname
ping DESKTOP-QEDCEJ1
nslookup DESKTOP-QEDCEJ1
```

---

## 🐧 OPTION 2: Setup DNS Server với dnsmasq trên WSL (KHUYẾN NGHỊ CHO DEV)

### Ưu điểm:

✅ Lightweight, dễ cấu hình  
✅ Chạy trên Windows 10 Home qua WSL  
✅ Professional, tốt cho môi trường dev

### Yêu cầu:

- Đã cài WSL (Windows Subsystem for Linux)

### Các bước thực hiện:

#### Bước 1: Cài đặt WSL (nếu chưa có)

```powershell
# Chạy với quyền Administrator
wsl --install -d Ubuntu

# Sau khi cài xong, restart máy
```

#### Bước 2: Cài dnsmasq trong WSL

```bash
# Mở WSL Ubuntu
wsl

# Update và cài dnsmasq
sudo apt update
sudo apt install dnsmasq -y
```

#### Bước 3: Cấu hình dnsmasq

```bash
# Backup config cũ
sudo cp /etc/dnsmasq.conf /etc/dnsmasq.conf.backup

# Tạo config mới
sudo nano /etc/dnsmasq.conf
```

Thêm nội dung sau:

```ini
# Lắng nghe trên tất cả interfaces
interface=eth0
listen-address=0.0.0.0

# DNS upstream (forward các query không phải local)
server=8.8.8.8
server=8.8.4.4

# Local domain
domain=local
local=/local/

# DNS records cho Student Activities
address=/DESKTOP-QEDCEJ1/192.168.102.3
address=/DESKTOP-QEDCEJ1.local/192.168.102.3

# Cache settings
cache-size=1000

# Log queries (optional, để debug)
# log-queries
# log-facility=/var/log/dnsmasq.log
```

Save: `Ctrl+O`, `Enter`, `Ctrl+X`

#### Bước 4: Khởi động dnsmasq

```bash
# Restart dnsmasq
sudo systemctl restart dnsmasq

# Enable auto-start
sudo systemctl enable dnsmasq

# Kiểm tra status
sudo systemctl status dnsmasq
```

#### Bước 5: Cấu hình Windows Firewall

```powershell
# Cho phép DNS port (53) qua firewall
New-NetFirewallRule -DisplayName "DNS Server (UDP)" -Direction Inbound -LocalPort 53 -Protocol UDP -Action Allow
New-NetFirewallRule -DisplayName "DNS Server (TCP)" -Direction Inbound -LocalPort 53 -Protocol TCP -Action Allow
```

#### Bước 6: Lấy IP của WSL

```bash
# Trong WSL, chạy:
ip addr show eth0 | grep inet
# Ghi nhớ IP này, ví dụ: 172.28.196.237
```

#### Bước 7: Cấu hình Client Machines

Trên mỗi máy client (Jenkins, Dev machines):

**Windows:**

```
1. Settings → Network & Internet → Change adapter options
2. Right-click adapter → Properties
3. Select "Internet Protocol Version 4 (TCP/IPv4)" → Properties
4. Use the following DNS server addresses:
   - Preferred DNS: 172.28.196.237 (IP của WSL)
   - Alternate DNS: 8.8.8.8
5. OK → OK
```

**PowerShell:**

```powershell
# Tự động set DNS
$InterfaceAlias = "Wi-Fi"  # hoặc "Ethernet"
$WSLIP = "172.28.196.237"   # IP của WSL

Set-DnsClientServerAddress -InterfaceAlias $InterfaceAlias -ServerAddresses $WSLIP,"8.8.8.8"
```

#### Bước 8: Test

```powershell
ipconfig /flushdns
nslookup DESKTOP-QEDCEJ1
ping DESKTOP-QEDCEJ1
```

#### Bước 9: Thêm hostname mới

Khi cần thêm hostname khác:

```bash
# Edit dnsmasq config
sudo nano /etc/dnsmasq.conf

# Thêm dòng:
address=/NEW-HOSTNAME/192.168.102.4

# Restart
sudo systemctl restart dnsmasq
```

### Troubleshooting WSL DNS:

**Lỗi: Port 53 đã được dùng**

```bash
# Kiểm tra process nào đang dùng port 53
sudo lsof -i :53

# Nếu là systemd-resolved, disable nó
sudo systemctl disable systemd-resolved
sudo systemctl stop systemd-resolved
```

**WSL IP thay đổi sau khi restart**
→ Tạo script PowerShell tự động update DNS:

```powershell
# auto-update-wsl-dns.ps1
$WSLIP = wsl hostname -I | ForEach-Object { $_.Trim() }
Set-DnsClientServerAddress -InterfaceAlias "Wi-Fi" -ServerAddresses $WSLIP,"8.8.8.8"
Write-Host "Updated DNS to: $WSLIP"
```

---

## 🐳 OPTION 3: Dùng Pi-hole trong Docker Container

### Ưu điểm:

✅ DNS Server + Ad blocker  
✅ Web UI đẹp, dễ quản lý  
✅ Chạy trong Docker, không cần VM

### Các bước thực hiện:

#### Bước 1: Tạo docker-compose cho Pi-hole

```yaml
# File: docker-compose-pihole.yml
version: "3"

services:
  pihole:
    container_name: pihole
    image: pihole/pihole:latest
    ports:
      - "53:53/tcp"
      - "53:53/udp"
      - "8080:80/tcp"
    environment:
      TZ: "Asia/Ho_Chi_Minh"
      WEBPASSWORD: "admin123" # Đổi password này
    volumes:
      - "./pihole/etc-pihole:/etc/pihole"
      - "./pihole/etc-dnsmasq.d:/etc/dnsmasq.d"
    dns:
      - 127.0.0.1
      - 8.8.8.8
    restart: unless-stopped
```

#### Bước 2: Chạy Pi-hole

```powershell
# Tạo thư mục
New-Item -ItemType Directory -Path "pihole" -Force

# Chạy container
docker-compose -f docker-compose-pihole.yml up -d

# Kiểm tra logs
docker logs pihole
```

#### Bước 3: Truy cập Pi-hole Admin

```
http://localhost:8080/admin
hoặc
http://192.168.102.3:8080/admin

Password: admin123 (hoặc password bạn đã đặt)
```

#### Bước 4: Thêm Local DNS Records

1. Login vào Pi-hole Admin
2. **Local DNS** → **DNS Records**
3. Add record:
   - Domain: `DESKTOP-QEDCEJ1`
   - IP Address: `192.168.102.3`
4. Add

#### Bước 5: Cấu hình Clients

Trên mỗi máy, set DNS server về `192.168.102.3`

```powershell
Set-DnsClientServerAddress -InterfaceAlias "Wi-Fi" -ServerAddresses "192.168.102.3","8.8.8.8"
```

#### Bước 6: Test

```powershell
ipconfig /flushdns
nslookup DESKTOP-QEDCEJ1 192.168.102.3
ping DESKTOP-QEDCEJ1
```

### Quản lý Pi-hole:

**Xem statistics:**

- Truy cập: http://192.168.102.3:8080/admin

**Thêm hostname mới:**

- Local DNS → DNS Records → Add

**Backup/Restore:**

- Settings → Teleporter → Backup

---

## 📝 SO SÁNH CÁC OPTION

| Tiêu chí        | Router DNS      | WSL dnsmasq      | Pi-hole Docker    |
| --------------- | --------------- | ---------------- | ----------------- |
| **Độ khó**      | ⭐ Dễ nhất      | ⭐⭐ Trung bình  | ⭐⭐ Trung bình   |
| **Setup time**  | 5-10 phút       | 20-30 phút       | 15-20 phút        |
| **Yêu cầu**     | Router hiện đại | WSL installed    | Docker installed  |
| **Quản lý**     | Web UI router   | CLI              | Web UI đẹp        |
| **Tính năng**   | Cơ bản          | DNS only         | DNS + Ad blocking |
| **Performance** | Tốt             | Tốt              | Tốt               |
| **Khuyến nghị** | ✅ Mạng nhỏ     | ✅ Dev/Tech user | ✅ Muốn Ad block  |

---

## 🎯 KHUYẾN NGHỊ CHO BẠN

Với môi trường của bạn (Windows 10 Home, IP 192.168.102.3), tôi khuyến nghị:

### ☑️ Nếu có 2-5 máy trong mạng:

→ **Dùng OPTION 1 (Router DNS)** nếu router hỗ trợ  
→ Hoặc dùng hosts file trên từng máy (đơn giản nhất)

### ☑️ Nếu có 5-10 máy, cần quản lý tập trung:

→ **Dùng OPTION 2 (WSL dnsmasq)** - Professional, lightweight

### ☑️ Nếu muốn DNS + Ad blocking + Web UI đẹp:

→ **Dùng OPTION 3 (Pi-hole Docker)**

---

## 🚀 SCRIPT TỰ ĐỘNG

Tôi sẽ tạo script giúp bạn setup tự động các option trên.

**Chọn option bạn muốn:**

```powershell
# Option 1: Kiểm tra router có hỗ trợ DNS không
.\check-router-dns.ps1

# Option 2: Auto setup WSL dnsmasq
.\setup-wsl-dns.ps1

# Option 3: Auto setup Pi-hole
.\setup-pihole-dns.ps1
```

---

## ✅ TEST SAU KHI SETUP

Trên mỗi máy client, chạy:

```powershell
# 1. Clear DNS cache
ipconfig /flushdns

# 2. Test DNS resolution
nslookup DESKTOP-QEDCEJ1

# 3. Test ping
ping DESKTOP-QEDCEJ1

# 4. Test Docker Registry
docker login DESKTOP-QEDCEJ1:5443

# 5. Kiểm tra DNS server đang dùng
ipconfig /all | Select-String "DNS Servers"
```

---

## 🔧 TROUBLESHOOTING

### Lỗi: "DNS server not responding"

```powershell
# Kiểm tra DNS server có chạy không
Test-NetConnection 192.168.102.3 -Port 53

# Thử switch DNS
Set-DnsClientServerAddress -InterfaceAlias "Wi-Fi" -ServerAddresses "8.8.8.8"
Set-DnsClientServerAddress -InterfaceAlias "Wi-Fi" -ServerAddresses "192.168.102.3","8.8.8.8"
```

### Lỗi: "Cannot resolve hostname"

```powershell
# Kiểm tra DNS query
nslookup DESKTOP-QEDCEJ1 192.168.102.3

# Nếu không work, check firewall
Test-NetConnection 192.168.102.3 -Port 53
```

### WSL dnsmasq không start

```bash
# Check logs
sudo journalctl -u dnsmasq -n 50

# Check port conflict
sudo lsof -i :53
```

---

## 📞 NEXT STEPS

1. **Chọn option phù hợp** với môi trường của bạn
2. **Follow hướng dẫn** từng bước
3. **Test kỹ** trên một máy trước
4. **Deploy** lên các máy khác
5. **Document** IP và hostname mapping

Bạn muốn tôi tạo script tự động cho option nào? 🚀
