@echo off
for /F "tokens=4*" %%a in ('netsh interface show interface ^| more +2') do echo %%a %%b
pause