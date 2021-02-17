@echo off
cd %~dp0

call GetHostname CurrentPcName
if [%CurrentPcName%] == [] set /p CurrentPcName="Enter current PC name: "

echo Current PC name: %CurrentPcName%
set /p NewPcName="Enter new PC name: "

WMIC computersystem where caption='%CurrentPcName%' rename %NewPcName%
pause