@echo off

if not exist ".coverage" mkdir .coverage

for /F "tokens=*" %%G in ('nuget locals global-packages -List') do (
	SET NuGets_2=%%G 
	SET NuGets_1=%NuGets_2:~17%
	SET NuGets=%NuGets_1:~0,-1%
)

for /F "tokens=*" %%G in ('where dotnet') do (
	SET DOT_NET=%%G
)

SET OPEN_COVER=
for /D %%D in ("%NuGets%opencover\*") do (
    SET OPEN_COVER=%%~D\tools\OpenCover.Console.exe
	ECHO %OPEN_COVER%
)

SET XUNIT_RUNNER=
for /D %%D in ("%NuGets%xunit.runner.console\*") do (
   SET XUNIT_RUNNER=%%~D\tools\net46\xunit.console.exe
)

IF "%OPEN_COVER%"=="" (
	ECHO Could not find OpenCover. Make sure to restore NuGet packages 1>&2
	EXIT 1
)

IF "%XUNIT_RUNNER%"=="" (
	ECHO Could not find xunit.runner.console. Make sure to restore NuGet packages 1>&2
	EXIT 1
)

"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.5.Tests\bin\Debug\net452\XrmEntitySerializer.5.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.5.net40.xml" -register:Path64 -filter:"+[XrmEntitySerializer.5]* -[XrmEntitySerializer.5.Tests]*"
"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.6.Tests\bin\Debug\net452\XrmEntitySerializer.6.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.6.net40.xml" -register:Path64 -filter:"+[XrmEntitySerializer.6]* -[XrmEntitySerializer.6.Tests]*"
"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.7.Tests\bin\Debug\net452\XrmEntitySerializer.7.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.7.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.7]* -[XrmEntitySerializer.7.Tests]*"
"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.8.Tests\bin\Debug\net452\XrmEntitySerializer.8.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.8.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.8]* -[XrmEntitySerializer.8.Tests]*"
"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.9.Tests\bin\Debug\net452\XrmEntitySerializer.9.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.9.net452.xml" -register:Path64 -filter:"+[XrmEntitySerializer.9]* -[XrmEntitySerializer.9.Tests]*"
"%OPEN_COVER%" -target:"%XUNIT_RUNNER%" -targetargs:"""XrmEntitySerializer.9.Tests\bin\Debug\net462\XrmEntitySerializer.9.Tests.dll"" -noshadow"  -output:".coverage\XrmEntitySerializer.9.net462.xml" -register:Path64 -filter:"+[XrmEntitySerializer.9]* -[XrmEntitySerializer.9.Tests]*"
CD XrmEntitySerializer.Core.Tests
"%OPEN_COVER%" -target:"%DOT_NET%" -targetargs:"test --no-build --framework net5.0" -output:"..\.coverage\XrmEntitySerializer.core.net5.0.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
"%OPEN_COVER%" -target:"%DOT_NET%" -targetargs:"test --no-build --framework netcoreapp3.1" -output:"..\.coverage\XrmEntitySerializer.core.netcoreapp3.0.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
"%OPEN_COVER%" -target:"%DOT_NET%" -targetargs:"test --no-build --framework netcoreapp3.0" -output:"..\.coverage\XrmEntitySerializer.core.netcoreapp3.1.xml" -register:user -filter:"+[XrmEntitySerializer.Core]* -[XrmEntitySerializer.Core.Tests]*" -oldstyle
CD ..