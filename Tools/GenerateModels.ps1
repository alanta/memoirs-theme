dotnet tool restore
$OUTPUT_PATH = Join-Path $PSScriptRoot "..\Models\ContentTypes"
dotnet tool run KontentModelGenerator -p "e10d7fbd-315f-01e2-20a5-e067cdf43f2f" -o $OUTPUT_PATH -n "Kentico.Kontent.Statiq.Memoirs.Models"