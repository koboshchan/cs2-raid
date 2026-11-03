param(
    [string]$CssApiDir = 'C:\cs2server\game\csgo\addons\counterstrikesharp\api',
    [string]$Dotnet = ''
)

$ErrorActionPreference = 'Stop'

if (-not $Dotnet) {
    $localDotnet = Join-Path (Split-Path $PSScriptRoot -Parent) '.dotnet\dotnet.exe'
    $Dotnet = if (Test-Path -LiteralPath $localDotnet) { $localDotnet } else { 'dotnet' }
}

if (-not (Test-Path -LiteralPath (Join-Path $CssApiDir 'CounterStrikeSharp.API.dll'))) {
    throw "CounterStrikeSharp API not found under: $CssApiDir"
}

$output = Join-Path $PSScriptRoot 'bin'
& $Dotnet build (Join-Path $PSScriptRoot 'DeltaOps.csproj') `
    -c Release `
    "-p:CSS_API_DIR=$CssApiDir" `
    -o $output

if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE"
}

Write-Host "Built: $(Join-Path $output 'DeltaOps.dll')"
