# Script de fix hostname resolution ve IPv4
# Chay as Administrator

param(
    [switch]$Check
)

$hostname = $env:COMPUTERNAME
$hostsFile = "C:\Windows\System32\drivers\etc\hosts"

# Lay IPv4 address
$ipv4 = (Get-NetIPAddress -AddressFamily IPv4 | Where-Object { 
    $_.InterfaceAlias -like "*Wi-Fi*" -or $_.InterfaceAlias -like "*Ethernet*" 
} | Where-Object { 
    $_.IPAddress -notlike "127.*" -and $_.IPAddress -notlike "169.254.*" 
} | Select-Object -First 1).IPAddress

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "           FIX HOSTNAME IPv4 RESOLUTION                   " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Hostname: $hostname" -ForegroundColor Yellow
Write-Host "IPv4:     $ipv4" -ForegroundColor Yellow
Write-Host ""

if ($Check) {
    Write-Host "Checking current hosts file..." -ForegroundColor Cyan
    Write-Host ""
    
    $hostsContent = Get-Content $hostsFile
    $found = $hostsContent | Select-String -Pattern $hostname
    
    if ($found) {
        Write-Host "Found in hosts file:" -ForegroundColor Green
        $found | ForEach-Object { Write-Host "  $_" -ForegroundColor White }
    }
    else {
        Write-Host "Hostname NOT found in hosts file" -ForegroundColor Red
    }
    
    Write-Host ""
    Write-Host "Testing ping..." -ForegroundColor Cyan
    ping $hostname -n 2
    
    exit
}

# Kiem tra quyen Admin
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Write-Host "ERROR: Script can chay voi quyen Administrator!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Cach chay:" -ForegroundColor Yellow
    Write-Host "  1. Mo PowerShell as Administrator" -ForegroundColor White
    Write-Host "  2. Chay: .\fix-hostname-ipv4.ps1" -ForegroundColor White
    Write-Host ""
    exit 1
}

# Doc hosts file
$hostsContent = Get-Content $hostsFile

# Kiem tra da co entry chua
$existingEntry = $hostsContent | Where-Object { $_ -match "^\s*$ipv4\s+$hostname" }

if ($existingEntry) {
    Write-Host "OK Entry da ton tai trong hosts file:" -ForegroundColor Green
    Write-Host "  $existingEntry" -ForegroundColor White
}
else {
    # Xoa cac entry cu cua hostname nay
    $cleanContent = $hostsContent | Where-Object { $_ -notmatch "\s+$hostname\s*$" }
    
    # Them entry moi
    $newEntry = "$ipv4    $hostname"
    $cleanContent += ""
    $cleanContent += "# Added by fix-hostname-ipv4.ps1 on $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    $cleanContent += $newEntry
    
    # Ghi vao file
    try {
        $cleanContent | Set-Content $hostsFile -Force
        Write-Host "OK Da them entry vao hosts file:" -ForegroundColor Green
        Write-Host "  $newEntry" -ForegroundColor White
    }
    catch {
        Write-Host "ERROR: Khong the ghi vao hosts file!" -ForegroundColor Red
        Write-Host $_.Exception.Message -ForegroundColor Red
        exit 1
    }
}

# Clear DNS cache
Write-Host ""
Write-Host "Clearing DNS cache..." -ForegroundColor Cyan
try {
    ipconfig /flushdns | Out-Null
    Write-Host "OK DNS cache cleared" -ForegroundColor Green
}
catch {
    Write-Host "WARNING: Khong the clear DNS cache" -ForegroundColor Yellow
}

# Test
Write-Host ""
Write-Host "Testing hostname resolution..." -ForegroundColor Cyan
Write-Host ""

$testResult = Test-NetConnection -ComputerName $hostname -WarningAction SilentlyContinue

if ($testResult.PingSucceeded) {
    Write-Host "OK Hostname resolves correctly!" -ForegroundColor Green
    Write-Host "  $hostname -> $($testResult.RemoteAddress)" -ForegroundColor White
}
else {
    Write-Host "WARNING: Hostname khong resolve" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Success! You can now use: http://$hostname/swagger" -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host ""
