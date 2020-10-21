@echo off
call C:\_Data\Temp\_svn74\.svn\update_bd.bat 192.168.0.57
del script.txt
cd %~dp0
pause