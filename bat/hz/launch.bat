@echo off

for /f "delims=" %%F in ('WHERE /R D:\Residents ResidentDriver.exe') do set var=%%F 
ECHO %var%
start %var%

pause