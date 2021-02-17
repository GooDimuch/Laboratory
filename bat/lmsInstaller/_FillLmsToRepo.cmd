echo Copy LMS to repo
cd /d "C:\%CheckoutFolderName%"
if exist "%cd%\lms" (
rmdir Lms /S /Q
)
md LMS
cd Lms
xcopy "%subjectsDir%Lms" "%cd%" /E /Y /Q

echo Adding ignores
svn add . --depth empty %quietFlag%
svn add Client --depth empty %quietFlag%
svn add Server --depth empty %quietFlag%
svn add Launcher --depth empty %quietFlag%

svn add Client\LmsClient --depth empty %quietFlag%
svn propset svn:ignore -F "%~dp0..\SvnIgnore.txt" Client\LmsClient

svn add Server\LmsServer --depth empty %quietFlag%
svn propset svn:ignore -F "%~dp0..\SvnIgnore.txt" Server\LmsServer
if not exist Server\LmsServer\db (
	md Server\LmsServer\db
)

svn add Launcher\LmsLauncher --depth empty %quietFlag%
svn propset svn:ignore -F "%~dp0..\SvnIgnore.txt" Launcher\LmsLauncher

echo Adding files to version controll
if "%silentMode%"=="v" (
pause
)
svn add . --force %quietFlag%
echo Commiting LMS
svn commit -m "LMS copied" %quietFlag%
echo Listing current repo status. Ignored files marked with letter I on the left.
svn status --no-ignore 
