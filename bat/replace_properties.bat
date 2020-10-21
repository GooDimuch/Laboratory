@echo off
cd %~dp0
set temp_file=temp_resources
set Ip=000.000.0.00
set properties_path="%PROGRAMFILES%\LMS\Client\Client\res\"
set properties_name=Properties.csv
set Number=0
echo %temp_file%\set_number_computer.bat
echo %properties_path%
echo %properties_name%
echo %Ip%
call %temp_file%\set_number_computer.bat %properties_path% %properties_name% %Number%
call %temp_file%\set_server_ip.bat %properties_path% %properties_name% %Ip%

pause