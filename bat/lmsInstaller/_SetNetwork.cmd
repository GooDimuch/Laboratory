
set addressEnding=%1
set /p mask=<"%~dp0..\SubnetMask.txt"
set /p addressBeginning=<"%~dp0..\addressBeginning.txt"
echo Starting network configuration. Subnet mask will be set to %mask% Network address begins with %addressBeginning%
if "%addressEnding%"=="" (
    set /p addressEnding="Hit Enter to skip network configuration. Type in the last digit and hit enter to configure static IP adress -> "
)
if "%addressEnding%"=="" (
echo Network configuration was skipped.
goto :end
)

netsh interface ip set address "Ethernet" static %addressBeginning%%addressEnding% %mask%
set networkSetupCompleted=true
echo Network configuration completed
:end

if "%silentMode%"=="v" (
pause
)