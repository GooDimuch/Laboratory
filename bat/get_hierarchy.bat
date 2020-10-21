set sPath=%~dp0
for /r %sPath% %%i in (*.*) do echo %%i>>hierarchy.txt