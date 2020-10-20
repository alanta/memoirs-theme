dotnet tool restore
$OUTPUT_PATH = Join-Path $PSScriptRoot "..\Models\ContentTypes"
dotnet tool run KontentModelGenerator -p "e1226067-3c50-00f3-3803-cf98cbb7af6b" -o $OUTPUT_PATH -n "Kentico.Kontent.Statiq.Memoirs.Models"