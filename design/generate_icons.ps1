<#
 Original design source for the Amazing Player control icons.
 Draws six simple 64x64 transparent PNGs (play, pause, stop, previous, next, add)
 with System.Drawing primitives only. No third-party artwork or fonts are used.
 Usage (from the repository root):  powershell -File design\generate_icons.ps1
#>
Add-Type -AssemblyName System.Drawing

$outDir = Join-Path $PSScriptRoot '..\app\MediaPlayerPrj\Assets\icons'
New-Item -ItemType Directory -Force $outDir | Out-Null
$outDir = (Resolve-Path $outDir).Path

# Accent color shared with Theme.Accent (31,111,98).
$brush = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(255, 31, 111, 98))

function New-Icon([string]$name, [scriptblock]$draw) {
    $bmp = New-Object System.Drawing.Bitmap 64, 64, ([System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)
    & $draw $g
    $g.Dispose()
    $bmp.Save((Join-Path $outDir "$name.png"), [System.Drawing.Imaging.ImageFormat]::Png)
    $bmp.Dispose()
}

function P($x, $y) { New-Object System.Drawing.PointF ([single]$x), ([single]$y) }

New-Icon 'play'  { param($g) $g.FillPolygon($brush, @((P 20 12), (P 20 52), (P 52 32))) }
New-Icon 'pause' { param($g) $g.FillRectangle($brush, 16, 12, 11, 40); $g.FillRectangle($brush, 37, 12, 11, 40) }
New-Icon 'stop'  { param($g) $g.FillRectangle($brush, 14, 14, 36, 36) }
New-Icon 'next'  { param($g) $g.FillPolygon($brush, @((P 10 14), (P 10 50), (P 38 32))); $g.FillRectangle($brush, 42, 14, 10, 36) }
New-Icon 'prev'  { param($g) $g.FillPolygon($brush, @((P 54 14), (P 54 50), (P 26 32))); $g.FillRectangle($brush, 12, 14, 10, 36) }
New-Icon 'add'   { param($g) $g.FillRectangle($brush, 27, 10, 10, 44); $g.FillRectangle($brush, 10, 27, 44, 10) }

Get-ChildItem $outDir -Filter *.png | ForEach-Object { "{0}  {1} bytes" -f $_.Name, $_.Length }
