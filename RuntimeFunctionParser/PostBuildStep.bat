REM Copies the release dll into to lib folder to pack the nuget package
set projectPath=%~1
set dllPath="%projectPath%bin\Release\RuntimeFunctionParser.dll"
ECHO %dllPath%
set libPath="%projectPath%lib"
ECHO %libPath%
copy %dllPath% %libPath% /Y