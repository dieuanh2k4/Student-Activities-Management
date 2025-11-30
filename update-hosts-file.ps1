# Script cập nhật hosts file trên Windows
# Yêu cầu: Chạy với quyền Administrator
# Sử dụng: .\update-hosts-file.ps1 -Hostname <HOSTNAME> -IPAddress <IP>

param(
    [Parameter(Mandatory=$true)]
    [string]$Hostname,
    
    [Parameter(Mandatory=$true)]
    [string]$IPAddress
)

# Kiểm tra quyền Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "ERROR: This script requires Administrator privileges!" -ForegroundColor Red
    Write-Host "Please run PowerShell as Administrator and try again." -ForegroundColor Yellow
    exit 1
}

Write-Host "=== Updating Windows hosts file ===" -ForegroundColor Cyan

# Đường dẫn hosts file
$hostsFile = "C:\Windows\System32\drivers\etc\hosts"

# Backup hosts file
$backupFile = "$hostsFile.backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
Copy-Item $hostsFile $backupFile
Write-Host "✓ Backup created: $backupFile" -ForegroundColor Green

# Đọc nội dung hiện tại
$hostsContent = Get-Content $hostsFile

# Entry mới
$newEntry = "$IPAddress`t$Hostname"
$commentEntry = "# Added by Student Activities setup script - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"

# Kiểm tra xem hostname đã tồn tại chưa
$existingEntry = $hostsContent | Where-Object { $_ -match "\s+$Hostname(\s|$)" -and $_ -notmatch "^\s*#" }

if ($existingEntry) {
    Write-Host "Found existing entry for hostname: $Hostname" -ForegroundColor Yellow
    Write-Host "  Old: $existingEntry" -ForegroundColor Gray
    Write-Host "  New: $newEntry" -ForegroundColor Gray
    
    # Thay thế entry cũ
    $hostsContent = $hostsContent | ForEach-Object {
        if ($_ -match "\s+$Hostname(\s|$)" -and $_ -notmatch "^\s*#") {
            $newEntry
        } else {
            $_
        }
    }
    
    Write-Host "✓ Updated existing entry" -ForegroundColor Green
} else {
    # Thêm entry mới
    $hostsContent += ""
    $hostsContent += $commentEntry
    $hostsContent += $newEntry
    
    Write-Host "✓ Added new entry: $newEntry" -ForegroundColor Green
}

# Ghi lại file
$hostsContent | Set-Content $hostsFile -Encoding ASCII

Write-Host "`nCurrent hosts file entries for $Hostname`:" -ForegroundColor Cyan
Get-Content $hostsFile | Where-Object { $_ -match $Hostname } | ForEach-Object {
    Write-Host "  $_" -ForegroundColor Gray
}

# Test hostname resolution
Write-Host "`nTesting hostname resolution..." -ForegroundColor Yellow
try {
    $resolved = [System.Net.Dns]::GetHostAddresses($Hostname)
    Write-Host "✓ Hostname resolves to: $($resolved.IPAddressToString)" -ForegroundColor Green
} catch {
    Write-Host "⚠ Could not resolve hostname. DNS cache may need to be flushed." -ForegroundColor Yellow
    Write-Host "  Run: ipconfig /flushdns" -ForegroundColor Gray
}

Write-Host "`n=== Update completed ===" -ForegroundColor Green
Write-Host "Run 'ipconfig /flushdns' to flush DNS cache if needed." -ForegroundColor Gray
