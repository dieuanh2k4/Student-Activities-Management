# Script thi?t l?p hostname cho h? th?ng
# S? d?ng: .\setup-hostname.ps1 [HOSTNAME]

param(
    [string]$Hostname = ""
)

Write-Host "" -ForegroundColor Cyan
Write-Host "     STUDENT ACTIVITIES - HOSTNAME SETUP                  " -ForegroundColor Cyan
Write-Host "-" -ForegroundColor Cyan

# N?u không có hostname, t? d?ng phát hi?n
if ([string]::IsNullOrEmpty($Hostname)) {
    Write-Host "`n[STEP 1] Auto-detecting hostname..." -ForegroundColor Yellow
    $Hostname = $env:COMPUTERNAME
    Write-Host "  Detected: $Hostname" -ForegroundColor Green
} else {
    Write-Host "`n[STEP 1] Using provided hostname: $Hostname" -ForegroundColor Yellow
}

# C?p nh?t .env.hostname
Write-Host "`n[STEP 2] Updating .env.hostname..." -ForegroundColor Yellow
$envFile = "$PSScriptRoot\.env.hostname"
if (Test-Path $envFile) {
    $content = Get-Content $envFile
    $newContent = $content -replace "DEPLOY_HOSTNAME=.*", "DEPLOY_HOSTNAME=$Hostname"
    $newContent | Set-Content $envFile
    Write-Host "   Updated .env.hostname" -ForegroundColor Green
}

# T?o .env file cho docker-compose v?i hostname
Write-Host "`n[STEP 3] Creating/Updating .env files..." -ForegroundColor Yellow

# .env cho docker-compose
$dockerEnvFile = "$PSScriptRoot\StudentActivies\.env"
if (Test-Path $dockerEnvFile) {
    $dockerEnv = Get-Content $dockerEnvFile
    
    # Thêm HOSTNAME n?u chua có
    if ($dockerEnv -notmatch "HOSTNAME=") {
        Add-Content -Path $dockerEnvFile -Value "`nHOSTNAME=$Hostname"
        Write-Host "   Added HOSTNAME to docker .env" -ForegroundColor Green
    } else {
        $dockerEnv = $dockerEnv -replace "HOSTNAME=.*", "HOSTNAME=$Hostname"
        $dockerEnv | Set-Content $dockerEnvFile
        Write-Host "   Updated HOSTNAME in docker .env" -ForegroundColor Green
    }
}

# .env.production
$prodEnvFile = "$PSScriptRoot\.env.production"
if (Test-Path $prodEnvFile) {
    $prodEnv = Get-Content $prodEnvFile
    
    if ($prodEnv -notmatch "HOSTNAME=") {
        Add-Content -Path $prodEnvFile -Value "HOSTNAME=$Hostname"
        Write-Host "   Added HOSTNAME to .env.production" -ForegroundColor Green
    } else {
        $prodEnv = $prodEnv -replace "HOSTNAME=.*", "HOSTNAME=$Hostname"
        $prodEnv | Set-Content $prodEnvFile
        Write-Host "   Updated HOSTNAME in .env.production" -ForegroundColor Green
    }
}

# C?p nh?t Docker daemon.json
Write-Host "`n[STEP 4] Configuring Docker daemon..." -ForegroundColor Yellow
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if ($isAdmin) {
    $daemonPath = "C:\ProgramData\docker\config"
    $daemonFile = "$daemonPath\daemon.json"
    
    if (-not (Test-Path $daemonPath)) {
        New-Item -ItemType Directory -Path $daemonPath -Force | Out-Null
    }
    
    $registryUrl = "${Hostname}:5443"
    
    if (Test-Path $daemonFile) {
        $config = Get-Content $daemonFile -Raw | ConvertFrom-Json
        $backupFile = "$daemonFile.backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
        Copy-Item $daemonFile $backupFile
        Write-Host "   Backup created: $backupFile" -ForegroundColor Gray
    } else {
        $config = New-Object PSObject
    }
    
    if ($config.PSObject.Properties.Name -contains "insecure-registries") {
        $registries = [System.Collections.ArrayList]@($config."insecure-registries")
        if ($registries -notcontains $registryUrl) {
            $registries.Add($registryUrl) | Out-Null
        }
        $config."insecure-registries" = $registries
    } else {
        $config | Add-Member -MemberType NoteProperty -Name "insecure-registries" -Value @($registryUrl) -Force
    }
    
    $config | ConvertTo-Json -Depth 10 | Set-Content $daemonFile -Encoding UTF8
    Write-Host "   Updated daemon.json with registry: $registryUrl" -ForegroundColor Green
    
    $restart = Read-Host "`n  Restart Docker service now? (y/n)"
    if ($restart -eq 'y' -or $restart -eq 'Y') {
        Write-Host "  Restarting Docker..." -ForegroundColor Yellow
        Restart-Service docker -Force
        Start-Sleep -Seconds 5
        Write-Host "   Docker restarted" -ForegroundColor Green
    }
} else {
    Write-Host "   Not running as Administrator - cannot update daemon.json" -ForegroundColor Yellow
    Write-Host "  Run as Admin to configure Docker Registry with hostname" -ForegroundColor Gray
}

Write-Host "`n[STEP 5] Jenkins Configuration" -ForegroundColor Yellow
Write-Host "  To configure Jenkins to use hostname:" -ForegroundColor Gray
Write-Host "  Option 1: Set Environment Variable in Jenkins" -ForegroundColor Gray
Write-Host "    - Go to: Manage Jenkins  Configure System" -ForegroundColor Gray
Write-Host "    - Find: Global properties  Environment variables" -ForegroundColor Gray
Write-Host "    - Add variable: Name=DEPLOY_HOSTNAME, Value=$Hostname" -ForegroundColor Gray
Write-Host "  Option 2: Jenkinsfile will auto-detect from COMPUTERNAME" -ForegroundColor Gray

Write-Host "`n[STEP 6] Network Configuration (Important!)" -ForegroundColor Yellow
Write-Host "  For hostname resolution to work on all machines:" -ForegroundColor Gray
Write-Host "  Option 1: Configure DNS Server (Recommended)" -ForegroundColor Gray
Write-Host "  Option 2: Update hosts file on each machine" -ForegroundColor Gray
Write-Host "    - File: C:\Windows\System32\drivers\etc\hosts" -ForegroundColor Gray
Write-Host "    - Add: <IP_ADDRESS>  $Hostname" -ForegroundColor Gray

Write-Host "`n" -ForegroundColor Green
Write-Host "              HOSTNAME SETUP COMPLETED                    " -ForegroundColor Green
Write-Host "" -ForegroundColor Green

Write-Host "`nCurrent Configuration:" -ForegroundColor Cyan
Write-Host "  Hostname:         $Hostname" -ForegroundColor White
Write-Host "  Docker Registry:  ${Hostname}:5443" -ForegroundColor White
Write-Host "  Application URL:  http://${Hostname}" -ForegroundColor White
Write-Host ""
