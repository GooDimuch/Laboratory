@echo off
cd /d %~dp0

call getUserEnviromentsPath UserPath
rem echo %UserPath%

for %%Q in ("%~dp0\.") DO set "BuildDir=%%~fQ"
set xplanePath=%BuildDir%

echo "Set X-Plane path:"
setx XPlane_Path "%xplanePath%"

echo "Set paths for plugins:"
setx path "%xplanePath%\Resources\plugins\MOXAResident\libs;%xplanePath%\Resources\plugins\InstructorUI\libs\;%UserPath%"
pause


