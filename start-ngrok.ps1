# Script de start ngrok tunnel cho app da deploy qua Jenkins
# Chay script nay tu may local hoac bat ky dau

# Load .env.production file
$envFile = ".env.production"
if (Test-Path $envFile) {
    Write-Host "Loading environment variables from $envFile..." -ForegroundColor Gray
    Get-Content $envFile | ForEach-Object {
        if ($_ -match '^\s*([^#][^=]+)\s*=\s*(.*)$') {
            $key = $matches[1].Trim()
            $value = $matches[2].Trim().Trim('"')
            Set-Variable -Name $key -Value $value -Scope Script
        }
    }
}

# Dung hostname tu .env.production hoac fallback
$SERVER = if ($HOSTNAME) { $HOSTNAME }
          elseif ($env:DEPLOY_HOSTNAME) { $env:DEPLOY_HOSTNAME }
          elseif ($env:COMPUTERNAME) { $env:COMPUTERNAME }
          else { "192.168.102.3" }

# Check if running on same machine (local)
$isLocal = ($SERVER -eq $env:COMPUTERNAME) -or ($SERVER -eq "localhost") -or ($SERVER -eq "127.0.0.1")

# Lay NGROK_AUTHTOKEN tu .env.production
$NGROK_TOKEN = $NGROK_AUTHTOKEN
if (-not $NGROK_TOKEN) {
    Write-Host "ERROR: NGROK_AUTHTOKEN not found in $envFile!" -ForegroundColor Red
    exit 1
}

Write-Host "=== Starting Ngrok Tunnel ===" -ForegroundColor Green
Write-Host "Server: $SERVER" -ForegroundColor Cyan

# 1. Kiem tra app container co dang chay khong
Write-Host "`n[1/4] Checking app container..." -ForegroundColor Yellow

if ($isLocal) {
    # Local: use docker-compose
    Write-Host "Running locally via docker-compose..." -ForegroundColor Cyan
    Set-Location "StudentActivies"
    docker-compose --env-file .env ps | Select-String "backend.*Up"
    $appRunning = $?
    Set-Location ".."
    
    if ($appRunning) {
        Write-Host "App containers are running" -ForegroundColor Green
    } else {
        Write-Host "Starting app containers..." -ForegroundColor Yellow
        Set-Location "StudentActivies"
        docker-compose --env-file .env up -d
        Set-Location ".."
    }
} else {
    # Remote: use SSH
    $appStatus = ssh jenkins@$SERVER "docker ps --filter name=studentactivities --format '{{.Status}}'"
if ($appStatus -match "Up") {
    Write-Host "App container is running" -ForegroundColor Green
} else {
    Write-Host "App container not found! Starting it..." -ForegroundColor Red
    ssh jenkins@$SERVER "docker start studentactivities"
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Container doesn't exist, creating new one..." -ForegroundColor Yellow
        ssh jenkins@$SERVER "docker run -d --name studentactivities -p 80:8080 --restart always --env-file C:/Users/jenkins/app.env ${SERVER}:5443/studentactivities:latest"
    }
    Start-Sleep -Seconds 3
}

}

# 2. Stop va xoa ngrok container cu
Write-Host "`n[2/4] Cleaning old ngrok container..." -ForegroundColor Yellow

if ($isLocal) {
    Set-Location "StudentActivies"
    docker-compose --env-file .env stop ngrok 2>$null
    docker-compose --env-file .env rm -f ngrok 2>$null
    Set-Location ".."
} else {
    ssh jenkins@$SERVER "docker stop ngrok-tunnel" 2>$null
    ssh jenkins@$SERVER "docker rm ngrok-tunnel" 2>$null
}

# 3. Start ngrok moi
Write-Host "`n[3/4] Starting ngrok tunnel..." -ForegroundColor Yellow

if ($isLocal) {
    Set-Location "StudentActivies"
    docker-compose --env-file .env up -d ngrok
    Set-Location ".."
} else {
    ssh jenkins@$SERVER "docker run -d --name ngrok-tunnel -p 4040:4040 --link studentactivities:app -e NGROK_AUTHTOKEN=$NGROK_TOKEN ngrok/ngrok:latest http app:8080"
}

# 4. Doi va lay URL
Write-Host "`n[4/4] Getting public URL..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

Write-Host "`n=== Ngrok Started Successfully ===" -ForegroundColor Green
Write-Host "`nNgrok Dashboard: http://${SERVER}:4040" -ForegroundColor Cyan
Write-Host "`nTo get the public URL, run:" -ForegroundColor Yellow
Write-Host "  .\get-ngrok-url.ps1" -ForegroundColor Gray

# Tu dong mo dashboard
$openDashboard = Read-Host "`nOpen ngrok dashboard now? (Y/n)"
if ($openDashboard -ne "n") {
    Start-Process "http://${SERVER}:4040"
}
