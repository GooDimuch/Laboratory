@echo off
cd %~dp0
Set infile=script.txt

echo update repository>%infile%
echo set root = 'https://%1/svn/repo'>>%infile%
echo where id = 1;>>%infile%
echo .exit>>%infile%

sqlite3 wc.db < script.txt
pause