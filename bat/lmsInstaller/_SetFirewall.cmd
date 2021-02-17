set state=%1
rem On for enabling
rem Off for disabling

if "%state%"=="" (
    set state=On
)
echo Setting windows firewall %state%
@NetSh AdvFirewall Set AllProfiles State %state%