@echo off
call ip_to_txt
set /p "x=" < ip.txt
del ip.txt
echo %x%
pause