@echo off
cd %~dp0
set /p ImportPath="Enter path to import folder: "
set /p OutputPath="Enter path to output folder: "
set ProtoFile=%~nx1

if [%ImportPath%] == [] set ImportPath=.
if [%OutputPath%] == [] set OutputPath=.
if [%ProtoFile%] == [] set /p ProtoFile="Enter proto file name  [example.proto]: "

rem echo %ImportPath%
rem echo %OutputPath%
rem echo %ProtoFile%

protoc -I %ImportPath% --csharp_out=%OutputPath% %ProtoFile%
rem pause