# Script setup Pi-hole DNS Server trong Docker
# Sử dụng: .\setup-pihole-dns.ps1

Write-Host "╔══════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║          Pi-hole DNS Server Setup Script                ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════╝" -ForegroundColor Cyan

$hostname = $env:COMPUTERNAME
$ipAddress = (Get-NetIPAddress -AddressFamily IPv4 | 
    Where-Object { $_.InterfaceAlias -like "*Wi-Fi*" }).IPAddress

Write-Host "`nServer Info:" -ForegroundColor Yellow
Write-Host "  Hostname: $hostname" -ForegroundColor White
Write-Host "  IP:       $ipAddress" -ForegroundColor White

# Kiểm tra Docker
Write-Host "`n[Step 1] Checking Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "  ✓ Docker installed: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Docker not found!" -ForegroundColor Red
    Write-Host "  Please install Docker Desktop first" -ForegroundColor Yellow
    exit 1
}

# Tạo docker-compose file
Write-Host "`n[Step 2] Creating docker-compose file..." -ForegroundColor Yellow

$composeContent = @"
version: "3"

services:
  pihole:
    container_name: pihole-dns
    image: pihole/pihole:latest
    hostname: pihole
    ports:
      - "53:53/tcp"
      - "53:53/udp"
      - "8080:80/tcp"
    environment:
      TZ: 'Asia/Ho_Chi_Minh'
      WEBPASSWORD: 'studentactivities2025'
      DNSMASQ_LISTENING: 'all'
      ServerIP: '$ipAddress'
    volumes:
      - './pihole/etc-pihole:/etc/pihole'
      - './pihole/etc-dnsmasq.d:/etc/dnsmasq.d'
    dns:
      - 127.0.0.1
      - 8.8.8.8
    cap_add:
      - NET_ADMIN
    restart: unless-stopped
"@

$composeFile = "$PSScriptRoot\docker-compose-pihole.yml"
$composeContent | Set-Content $composeFile -Encoding UTF8
Write-Host "  ✓ Created: $composeFile" -ForegroundColor Green

# Tạo thư mục
Write-Host "`n[Step 3] Creating directories..." -ForegroundColor Yellow
New-Item -ItemType Directory -Path "$PSScriptRoot\pihole" -Force | Out-Null
Write-Host "  ✓ Created pihole directory" -ForegroundColor Green

# Cấu hình firewall
Write-Host "`n[Step 4] Configuring Windows Firewall..." -ForegroundColor Yellow
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if ($isAdmin) {
    try {
        New-NetFirewallRule -DisplayName "Pi-hole DNS (UDP)" -Direction Inbound -LocalPort 53 -Protocol UDP -Action Allow -ErrorAction SilentlyContinue | Out-Null
        New-NetFirewallRule -DisplayName "Pi-hole DNS (TCP)" -Direction Inbound -LocalPort 53 -Protocol TCP -Action Allow -ErrorAction SilentlyContinue | Out-Null
        New-NetFirewallRule -DisplayName "Pi-hole Web (TCP)" -Direction Inbound -LocalPort 8080 -Protocol TCP -Action Allow -ErrorAction SilentlyContinue | Out-Null
        Write-Host "  ✓ Firewall rules added" -ForegroundColor Green
    } catch {
        Write-Host "  ⚠ Could not add firewall rules (may already exist)" -ForegroundColor Yellow
    }
} else {
    Write-Host "  ⚠ Not running as Admin - skipping firewall config" -ForegroundColor Yellow
}

# Khởi động Pi-hole
Write-Host "`n[Step 5] Starting Pi-hole container..." -ForegroundColor Yellow
Write-Host "  This may take a few minutes for first time..." -ForegroundColor Gray

try {
    docker-compose -f $composeFile up -d
    Write-Host "  ✓ Pi-hole started!" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Failed to start Pi-hole: $_" -ForegroundColor Red
    exit 1
}

# Đợi Pi-hole khởi động
Write-Host "`n[Step 6] Waiting for Pi-hole to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

$ready = $false
for ($i = 1; $i -le 6; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:8080" -TimeoutSec 5 -ErrorAction Stop
        $ready = $true
        break
    } catch {
        Write-Host "  Waiting... ($i/6)" -ForegroundColor Gray
        Start-Sleep -Seconds 5
    }
}

if ($ready) {
    Write-Host "  ✓ Pi-hole is ready!" -ForegroundColor Green
} else {
    Write-Host "  ⚠ Pi-hole may still be starting..." -ForegroundColor Yellow
}

# Thêm DNS record
Write-Host "`n[Step 7] Adding local DNS record..." -ForegroundColor Yellow
Write-Host "  Adding: $hostname → $ipAddress" -ForegroundColor Gray

# Tạo custom dnsmasq config
$dnsmasqConfig = "$PSScriptRoot\pihole\etc-dnsmasq.d\02-custom.conf"
$customDns = "address=/$hostname/$ipAddress"
$customDns | Set-Content $dnsmasqConfig -Encoding UTF8

# Restart Pi-hole để apply
docker restart pihole-dns | Out-Null
Start-Sleep -Seconds 5
Write-Host "  ✓ DNS record added" -ForegroundColor Green

# Hiển thị thông tin
Write-Host "`n╔══════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║              Pi-hole DNS Setup Complete!                ║" -ForegroundColor Green
Write-Host "╚══════════════════════════════════════════════════════════╝" -ForegroundColor Green

Write-Host "`nPi-hole Admin Panel:" -ForegroundColor Cyan
Write-Host "  URL:      http://$ipAddress`:8080/admin" -ForegroundColor White
Write-Host "  Password: studentactivities2025" -ForegroundColor White

Write-Host "`nDNS Configuration:" -ForegroundColor Cyan
Write-Host "  DNS Server: $ipAddress" -ForegroundColor White
Write-Host "  Hostname:   $hostname" -ForegroundColor White
Write-Host "  IP Address: $ipAddress" -ForegroundColor White

Write-Host "`nNext Steps:" -ForegroundColor Yellow
Write-Host "  1. Open Pi-hole Admin: http://$ipAddress`:8080/admin" -ForegroundColor Gray
Write-Host "  2. Verify DNS record in: Local DNS → DNS Records" -ForegroundColor Gray
Write-Host "  3. Configure client machines to use this DNS server" -ForegroundColor Gray
Write-Host ""
Write-Host "  On each client machine, run:" -ForegroundColor Gray
Write-Host "    Set-DnsClientServerAddress -InterfaceAlias 'Wi-Fi' -ServerAddresses '$ipAddress','8.8.8.8'" -ForegroundColor Cyan
Write-Host ""
Write-Host "  4. Test:" -ForegroundColor Gray
Write-Host "    nslookup $hostname $ipAddress" -ForegroundColor Cyan
Write-Host "    ping $hostname" -ForegroundColor Cyan
Write-Host ""

# Tạo script cho client
$clientScript = @"
# Script cấu hình DNS client để dùng Pi-hole
# Chạy với quyền Administrator trên máy client

`$DNSServer = "$ipAddress"
`$InterfaceAlias = "Wi-Fi"  # Đổi thành "Ethernet" nếu dùng dây

Write-Host "Configuring DNS to use Pi-hole at `$DNSServer..." -ForegroundColor Cyan

Set-DnsClientServerAddress -InterfaceAlias `$InterfaceAlias -ServerAddresses `$DNSServer,"8.8.8.8"

ipconfig /flushdns

Write-Host "Done! Testing..." -ForegroundColor Green
nslookup $hostname `$DNSServer
"@

$clientScript | Set-Content "$PSScriptRoot\configure-client-pihole.ps1" -Encoding UTF8
Write-Host "Created client configuration script:" -ForegroundColor Green
Write-Host "  → configure-client-pihole.ps1" -ForegroundColor Cyan
Write-Host ""
