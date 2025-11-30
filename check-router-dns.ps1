# Script kiem tra router co ho tro local DNS khong
# Su dung: .\check-router-dns.ps1

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "         Router DNS Capability Check                      " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

# Lay thong tin network
Write-Host ""
Write-Host "[Step 1] Getting network information..." -ForegroundColor Yellow

$adapter = Get-NetAdapter | Where-Object Status -eq "Up" | Select-Object -First 1
$gateway = (Get-NetRoute | Where-Object DestinationPrefix -eq '0.0.0.0/0' | Sort-Object RouteMetric | Select-Object -First 1).NextHop

Write-Host "  Active Adapter: $($adapter.Name)" -ForegroundColor White
Write-Host "  Gateway (Router): $gateway" -ForegroundColor White

# Kiem tra ket noi den router
Write-Host ""
Write-Host "[Step 2] Testing connection to router..." -ForegroundColor Yellow
$pingResult = Test-NetConnection -ComputerName $gateway -WarningAction SilentlyContinue

if ($pingResult.PingSucceeded) {
    Write-Host "  OK Router is reachable" -ForegroundColor Green
}
else {
    Write-Host "  X Cannot reach router" -ForegroundColor Red
    exit 1
}

# Thu cac port pho bien
Write-Host ""
Write-Host "[Step 3] Checking router web interface..." -ForegroundColor Yellow
$ports = @(80, 8080, 443, 8443)
$openPorts = @()

foreach ($port in $ports) {
    $test = Test-NetConnection -ComputerName $gateway -Port $port -WarningAction SilentlyContinue -InformationLevel Quiet
    if ($test) {
        $openPorts += $port
        Write-Host "  OK Port $port is open" -ForegroundColor Green
    }
}

if ($openPorts.Count -eq 0) {
    Write-Host "  ! No common web ports found" -ForegroundColor Yellow
}
else {
    Write-Host ""
    Write-Host "  Router web interface may be available at:" -ForegroundColor Cyan
    foreach ($port in $openPorts) {
        if ($port -eq 443 -or $port -eq 8443) {
            Write-Host "    https://$gateway`:$port" -ForegroundColor White
        }
        else {
            Write-Host "    http://$gateway`:$port" -ForegroundColor White
        }
    }
}

# Hien thi huong dan
Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "              Router Configuration Guide                  " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

Write-Host ""
Write-Host "TO CONFIGURE LOCAL DNS ON YOUR ROUTER:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. Open router admin page:" -ForegroundColor White
if ($openPorts.Count -gt 0) {
    Write-Host "   -> http://$gateway" -ForegroundColor Cyan
}
else {
    Write-Host "   -> Try: http://$gateway or http://192.168.1.1" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "2. Login with admin credentials" -ForegroundColor White
Write-Host "   (Usually 'admin'/'admin' or check router label)" -ForegroundColor Gray

Write-Host ""
Write-Host "3. Look for these sections:" -ForegroundColor White
Write-Host "   Common locations:" -ForegroundColor Gray
Write-Host "   - Advanced -> Network -> DHCP Server" -ForegroundColor Gray
Write-Host "   - LAN -> DHCP Server -> Static DNS" -ForegroundColor Gray
Write-Host "   - Network -> DNS -> Local DNS Records" -ForegroundColor Gray
Write-Host "   - Services -> DNS -> Static DNS" -ForegroundColor Gray

Write-Host ""
Write-Host "4. Add DNS entry:" -ForegroundColor White
$currentIP = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object { $_.InterfaceAlias -like '*Wi-Fi*' }).IPAddress
Write-Host "   Hostname:   $env:COMPUTERNAME" -ForegroundColor Cyan
Write-Host "   IP Address: $currentIP" -ForegroundColor Cyan

Write-Host ""
Write-Host "5. Save and restart router" -ForegroundColor White

Write-Host ""
Write-Host "ROUTER-SPECIFIC GUIDES:" -ForegroundColor Yellow
Write-Host ""
Write-Host "TP-Link Routers:" -ForegroundColor Cyan
Write-Host "  -> Advanced -> Network -> DHCP Server -> Address Reservation" -ForegroundColor Gray

Write-Host ""
Write-Host "Asus Routers:" -ForegroundColor Cyan
Write-Host "  -> LAN -> DHCP Server -> Manual Assignment" -ForegroundColor Gray

Write-Host ""
Write-Host "Netgear Routers:" -ForegroundColor Cyan
Write-Host "  -> Advanced -> Setup -> LAN Setup -> Address Reservation" -ForegroundColor Gray

Write-Host ""
Write-Host "D-Link Routers:" -ForegroundColor Cyan
Write-Host "  -> Setup -> Network Settings -> Add DHCP Reservation" -ForegroundColor Gray

Write-Host ""
Write-Host "IF YOUR ROUTER DOESN'T SUPPORT LOCAL DNS:" -ForegroundColor Yellow
Write-Host "  Consider using alternative solutions:" -ForegroundColor White
Write-Host "  1. Pi-hole DNS (in Docker) - Run: .\setup-pihole-dns.ps1" -ForegroundColor Cyan
Write-Host "  2. Hosts file on each machine - Run: .\update-hosts-file.ps1" -ForegroundColor Cyan

Write-Host ""
$openBrowser = Read-Host "Open router admin page in browser? (y/n)"
if ($openBrowser -eq 'y' -or $openBrowser -eq 'Y') {
    Start-Process "http://$gateway"
    Write-Host ""
    Write-Host "OK Opened http://$gateway in browser" -ForegroundColor Green
}

Write-Host ""
