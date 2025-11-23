# Script de lay nhanh ngrok public URL
$SERVER = "192.168.102.3"

Write-Host "Getting ngrok public URL..." -ForegroundColor Cyan

# Lay logs va tim URL
Write-Host "Fetching logs from ngrok container..." -ForegroundColor Gray
$logs = ssh jenkins@$SERVER "docker logs ngrok-tunnel 2>&1"

$urlPattern = "url=(https://[^\s]+)"
if ($logs -match $urlPattern) {
    $url = $matches[1]
    Write-Host "`n=== Ngrok Public URL ===" -ForegroundColor Green
    Write-Host $url -ForegroundColor Yellow
    Write-Host "`nSwagger UI: ${url}/swagger" -ForegroundColor Cyan
    Write-Host "API Base: ${url}/api" -ForegroundColor Cyan
    
    # Copy to clipboard
    Set-Clipboard -Value $url
    Write-Host "`nURL copied to clipboard!" -ForegroundColor Green
    
    # Hoi co muon mo khong
    $open = Read-Host "`nOpen Swagger in browser? (Y/n)"
    if ($open -ne "n") {
        Start-Process "${url}/swagger"
    }
} else {
    Write-Host "Could not find ngrok URL in logs." -ForegroundColor Red
    Write-Host "`nPlease check the ngrok dashboard at:" -ForegroundColor Yellow
    Write-Host "  http://${SERVER}:4040" -ForegroundColor Cyan
    Write-Host "`nThe URL might not be ready yet. Wait a few seconds and try again." -ForegroundColor Gray
    
    $openDashboard = Read-Host "`nOpen dashboard now? (Y/n)"
    if ($openDashboard -ne "n") {
        Start-Process "http://${SERVER}:4040"
    }
}
