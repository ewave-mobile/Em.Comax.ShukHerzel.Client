# Build Angular (production), copy to API wwwroot, publish API Release
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot

$web = Join-Path $root 'EM.Comax.ShukHerzel.Web'
$api = Join-Path $root 'EM.Comax.ShukHerzel.Api'
$publishOut = Join-Path $root 'publish\api-prod'

Push-Location $web
try {
  npm run build
} finally {
  Pop-Location
}

$src = Join-Path $web 'dist\em.comax.shuk-herzel.web\browser'
$dst = Join-Path $api 'wwwroot'
if (-not (Test-Path $src)) {
  throw "Angular browser output not found: $src"
}

if (Test-Path $dst) { Remove-Item $dst -Recurse -Force }
New-Item -ItemType Directory -Path $dst | Out-Null
Copy-Item (Join-Path $src '*') $dst -Recurse -Force
Write-Host "SPA copied to $dst"

dotnet publish (Join-Path $api 'EM.Comax.ShukHerzel.Api.csproj') -c Release -o $publishOut
Write-Host "Published to $publishOut"
