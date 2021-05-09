$path = ".coverage"
If(!(test-path $path -PathType Container ))
{
	New-Item -ItemType Directory -Force -Path $path
}

IF(!(Test-Path .coverage\OpenCover* -PathType Container))
{
	& nuget install OpenCover -OutputDirectory $path
}
IF(!(Test-Path .coverage\xunit.runner.console* -PathType Container))
{
	& nuget install xunit.runner.console -OutputDirectory $path 
}

$DotNet=(Get-Command dotnet).Source
$OpenCover=Get-ChildItem -Path $((Get-ChildItem -Path .coverage\OpenCover* | Sort-Object -Descending | Select-Object -First 1).FullName)tools\OpenCover.Console.exe
$XUnitRunner=Get-ChildItem -Path $((Get-ChildItem -Path .coverage\xunit.runner.console* | Sort-Object -Descending | Select-Object -First 1).FullName)tools\net46\xunit.console.exe


If($OpenCover -eq ''){
	throw "Could not find OpenCover. Make sure to restore NuGet packages"
}

If($XUnitRunner -eq ''){
	throw "Could not find xunit.runner.console. Make sure to restore NuGet packages"
}

& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.5.Tests\bin\Debug\net452\XrmEntitySerializer.5.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.5.net40.xml" -register:Path64 -filter:"+[XrmEntitySerializer.5]* -[XrmEntitySerializer.5.Tests]*"
& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.6.Tests\bin\Debug\net452\XrmEntitySerializer.6.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.6.net40.xml" -register:Path64 -filter:"+[XrmEntitySerializer.6]* -[XrmEntitySerializer.6.Tests]*"
& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.7.Tests\bin\Debug\net452\XrmEntitySerializer.7.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.7.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.7]* -[XrmEntitySerializer.7.Tests]*"
& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.8.Tests\bin\Debug\net452\XrmEntitySerializer.8.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.8.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.8]* -[XrmEntitySerializer.8.Tests]*"
& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.9.Tests\bin\Debug\net452\XrmEntitySerializer.9.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.9.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.9]* -[XrmEntitySerializer.9.Tests]*"
& $OpenCover -target:"$XUnitRunner" -targetargs:"""XrmEntitySerializer.9.Tests\bin\Debug\net462\XrmEntitySerializer.9.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.9.net462.xml" -register:Path64 -filter:"+[XrmEntitySerializer.9]* -[XrmEntitySerializer.9.Tests]*"
CD XrmEntitySerializer.Core.Tests
& $OpenCover -target:"$DotNet" -targetargs:"test --no-build --framework net5.0" -output:"..\.coverage\XrmEntitySerializer.core.net5.0.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
& $OpenCover -target:"$DotNet" -targetargs:"test --no-build --framework netcoreapp3.1" -output:"..\.coverage\XrmEntitySerializer.core.netcoreapp3.0.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
& $OpenCover -target:"$DotNet" -targetargs:"test --no-build --framework netcoreapp3.0" -output:"..\.coverage\XrmEntitySerializer.core.netcoreapp3.1.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
CD ..