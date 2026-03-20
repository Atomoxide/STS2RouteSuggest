$ErrorActionPreference = "Stop"
Set-PSDebug -Trace 1


$GODOT = "../Godot_v4.5.1-stable_mono_win64/Godot_v4.5.1-stable_mono_win64_console.exe"

# Godot build
& $GODOT --build-solutions --quit --headless --verbose