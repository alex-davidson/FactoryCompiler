param([string]$name)

$factoryPath = Join-Path $PSScriptRoot $name;
$debugBinary = Get-Item $(Join-Path $PSScriptRoot "..\FactoryCompiler\bin\Debug\net6.0-windows7.0\FactoryCompiler.exe");
$releaseBinary = Get-Item $(Join-Path $PSScriptRoot "..\FactoryCompiler\bin\Debug\net6.0-windows7.0\FactoryCompiler.exe");

if (-not $releaseBinary) { $binary = $debugBinary; }
elseif (-not $debugBinary) { $binary = $releaseBinary; }
elseif ($debugBinary.LastWriteTime -gt $releaseBinary.LastWriteTime) { $binary = $debugBinary; }
else { $binary = $releaseBinary; }

$factoryItems = Get-ChildItem $factoryPath -recurse -include *.json | % { $_.FullName };


& $binary visualise @factoryItems;
