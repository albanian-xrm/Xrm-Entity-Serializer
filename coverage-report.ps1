& .\coverage.ps1

IF(!(Test-Path .coverage\ReportGenerator* -PathType Container))
{
	& nuget install ReportGenerator -OutputDirectory .coverage
}

$ReportGenerator=Get-ChildItem -Path $((Get-ChildItem -Path .coverage\ReportGenerator* | Sort-Object -Descending | Select-Object -First 1).FullName)tools\net47\ReportGenerator.exe

If($ReportGenerator -eq ''){
	throw "Could not find ReportGenerator. Make sure to restore NuGet packages"
}

& $ReportGenerator -reports:".coverage\XrmEntitySerializer.5.net40.xml;.coverage\XrmEntitySerializer.6.net40.xml;.coverage\XrmEntitySerializer.7.net452.xml;.coverage\XrmEntitySerializer.8.net452.xml;.coverage\XrmEntitySerializer.9.net452.xml;.coverage\XrmEntitySerializer.9.net462.xml;.coverage\XrmEntitySerializer.core.net5.0.xml;.coverage\XrmEntitySerializer.core.netcoreapp3.0.xml;.coverage\XrmEntitySerializer.core.netcoreapp3.1.xml" -targetdir:".coverage\report" -reporttypes:"html"

Start-Process .coverage\report\index.htm
