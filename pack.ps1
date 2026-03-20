$ErrorActionPreference = "Stop"
Set-PSDebug -Trace 1


$GODOT = "../Godot_v4.5.1-stable_mono_win64/Godot_v4.5.1-stable_mono_win64_console.exe"

# Crate dist directory and move build result to it
if (Test-Path "dist") { Remove-Item -Recurse -Force "dist" }
New-Item -ItemType Directory -Path "dist" -Force

Copy-Item -Path "./.godot/mono/temp/bin/Debug/RouteSuggest.dll" -Destination "dist/" -Force

# Export Godot Pack to dist directory
& $GODOT --export-pack "Windows Desktop" "dist/RouteSuggest.pck" --headless

# Copy JSON modding metadata
Copy-Item -Path "RouteSuggest.json" -Destination "dist/RouteSuggest.json" -Force

# Extract Version and pack in zip
$json = Get-Content "RouteSuggest.json" -Raw | ConvertFrom-Json
$VERSION = $json.version

$zipName = "RouteSuggest-v$VERSION.zip"
if (Test-Path $zipName) { Remove-Item $zipName }

Compress-Archive -Path "dist\*" -DestinationPath $zipName -Force