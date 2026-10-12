# Genera dist/FreeConveyors-<version>.zip listo para subir a Thunderstore.
$root = Split-Path $PSScriptRoot -Parent
$dll  = "$root\FreeConveyors\bin\Release\netstandard2.1\FreeConveyors.dll"
$out  = "$root\dist"; $tmp = "$out\pkg"
if (-not (Test-Path $dll)) { throw "Compila primero: dotnet build FreeConveyors -c Release" }
if (-not (Test-Path "$PSScriptRoot\icon.png")) {
  Add-Type -AssemblyName System.Drawing
  $b = New-Object System.Drawing.Bitmap 256,256; $g = [System.Drawing.Graphics]::FromImage($b)
  $g.Clear([System.Drawing.Color]::FromArgb(40,44,52))
  $g.DrawString("FC", (New-Object System.Drawing.Font "Arial",96,[System.Drawing.FontStyle]::Bold), [System.Drawing.Brushes]::Orange, 30, 60)
  $b.Save("$PSScriptRoot\icon.png"); $g.Dispose(); $b.Dispose()
}
Remove-Item $out -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force $tmp | Out-Null
$loc  = "$root\FreeConveyors\bin\Release\netstandard2.1\FreeConveyors.Localization.json"
Copy-Item $dll, $loc, "$PSScriptRoot\manifest.json", "$PSScriptRoot\README.md", "$PSScriptRoot\icon.png" $tmp
if (Test-Path "$PSScriptRoot\CHANGELOG.md") { Copy-Item "$PSScriptRoot\CHANGELOG.md" $tmp }
$ver = (Get-Content "$PSScriptRoot\manifest.json" -Raw | ConvertFrom-Json).version_number
Compress-Archive "$tmp\*" "$out\FreeConveyors-$ver.zip" -Force
Remove-Item $tmp -Recurse -Force
"Listo: $out\FreeConveyors-$ver.zip"
