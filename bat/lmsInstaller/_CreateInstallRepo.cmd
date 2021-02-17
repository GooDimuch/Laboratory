if "%InstallRepoName%"=="" (set InstallRepoName=InstallRepo)

if not exist C:\repositories (
md C:\repositories
)
cd /d c:\repositories

if exist "%cd%\%InstallRepoName%\" (
rmdir %InstallRepoName% /S /Q
)

echo Creating repo and setting up server
svnadmin create "c:\repositories\%InstallRepoName%"

rem installing hook
cd /d "c:\repositories\%InstallRepoName%\hooks"
xcopy "%~dp0pre-revprop-change.cmd" "%cd%" /E /Y /Q


rem Server configuration
cd /d "c:\repositories\%InstallRepoName%\conf"
rename svnserve.conf "svnserve.conf.backup"
(echo ^[general^] & echo anon^-access^=write)>>svnserve.conf

start /wait net stop svnserve_install
sc delete svnserve_install
sc create svnserve_install binpath= "%svnDestinationDir%\bin\svnserve --service -r c:\repositories" displayname= "Subversion Server" depend= Tcpip start= auto
net start svnserve_install

if "%silentMode%"=="v" (
pause
)