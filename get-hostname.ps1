# Script lấy hostname của máy
# Sử dụng: .\get-hostname.ps1

Write-Host "Detecting server hostname..." -ForegroundColor Cyan

# Lấy hostname
$hostname = $env:COMPUTERNAME

if ($hostname) {
    Write-Host "Detected Hostname: $hostname" -ForegroundColor Green
    
    # Cập nhật file .env.hostname
    $envFile = "$PSScriptRoot\.env.hostname"
    if (Test-Path $envFile) {
        $content = Get-Content $envFile
        $newContent = $content -replace "DEPLOY_HOSTNAME=.*", "DEPLOY_HOSTNAME=$hostname"
        $newContent | Set-Content $envFile
        Write-Host "Updated .env.hostname with hostname: $hostname" -ForegroundColor Green
    }
    
    # Trả về hostname
    Write-Host "`nHostname Information:" -ForegroundColor Yellow
    Write-Host "  Computer Name: $hostname" -ForegroundColor Cyan
    Write-Host "  Full DNS Name: $([System.Net.Dns]::GetHostName())" -ForegroundColor Cyan
    
    return $hostname
} else {
    Write-Host "Could not detect hostname. Please check system configuration." -ForegroundColor Red
    exit 1
}
