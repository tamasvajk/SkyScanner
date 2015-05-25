msbuild "..\SkyScanner.sln" /t:Rebuild /p:Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%
nuget pack "..\src\SkyScanner\bin\Release\SkyScanner.nuspec"