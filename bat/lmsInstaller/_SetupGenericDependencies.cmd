@echo off
REM echo on
if "%silentMode%"=="v" (
set promting=Prompt
) else (
set promting=
)
msiexec /x "%packagesDir%VisualSVN.msi" /passive /le "%~dp0install_SVN.log"
call "%resDir%_%promting%setupDotNET"
call "%resDir%_%promting%setupSVN"
call "%resDir%_copyLmsUpdater"
if "%silentMode%"=="v" (
pause
)
exit