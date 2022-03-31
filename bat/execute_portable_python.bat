@echo off

set python_path="%~dp0venv\Scripts"
echo Set paths for python:
echo %python_path%
setx path %python_path%
echo ------------------------------------
echo/

if [%1] == [] (
	python main.py
) else (
	python %1
)
