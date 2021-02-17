@echo off
cd %~dp0

set /p "buk_path=" < _buk_path.txt

set ProtoFile=%~fs1

echo %buk_path%
echo %ProtoFile%

call %buk_path% -test "%ProtoFile%"
rem pause
