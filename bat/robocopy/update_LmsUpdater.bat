cd c:/
rmdir /s /q LmsUpdater
robocopy \\192.168.0.10\share\LmsUpdater C:\LmsUpdater /E /COPY:DAT
pause 