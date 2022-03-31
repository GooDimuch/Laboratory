@echo off

for /F "skip=2 tokens=1,2*" %%N in ('%SystemRoot%\System32\reg.exe query "HKCU\Environment" /v "Path" 2^>nul') do if /I "%%N" == "Path" call set "UserPath=%%P" & goto RETURN

echo There is no user PATH defined.
echo/
pause
goto :EOF

:RETURN

for %%Q in ("%~dp0\.") DO set "BuildDir=%%~fQ"
set xplanePath=%BuildDir%

echo Set X-Plane path:
setx XPlane_Path "%xplanePath%"
echo ------------------------------------
echo/

echo Set paths for plugins:
setx path "%xplanePath%\Resources\plugins\MOXAResident\libs;%xplanePath%\Resources\plugins\InstructorUI\libs\;%UserPath%"
echo ------------------------------------
echo/
pause


