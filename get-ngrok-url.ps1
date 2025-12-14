# Script de lay nhanh ngrok public URL
# Dung hostname thay vi IP
$SERVER = $env:DEPLOY_HOSTNAME
if (-not $SERVER) {
    $SERVER = $env:COMPUTERNAME
}
if (-not $SERVER) {
    $SERVER = "192.168.102.3"  # Fallback
}

# Check if running on same machine (local)
$isLocal = ($SERVER -eq $env:COMPUTERNAME) -or ($SERVER -eq "localhost") -or ($SERVER -eq "127.0.0.1")

Write-Host "Getting ngrok public URL..." -ForegroundColor Cyan

if ($isLocal) {
    # Local: use API directly
    try {
        $response = Invoke-RestMethod http://localhost:4040/api/tunnels
        
        if ($response.tunnels -and $response.tunnels.Count -gt 0) {
            $url = $response.tunnels[0].public_url
            
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
            Write-Host "No tunnels found. Check dashboard at http://localhost:4040" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "Could not connect to ngrok API." -ForegroundColor Red
        Write-Host "Make sure ngrok is running: .\start-ngrok.ps1" -ForegroundColor Yellow
    }
} else {
    # Remote: use SSH and logs
    Write-Host "Fetching logs from remote server..." -ForegroundColor Gray
    $logs = ssh jenkins@$SERVER "docker logs ngrok-tunnel 2>&1"

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
}
