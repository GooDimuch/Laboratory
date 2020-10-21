^j::
WinGetPos, X, Y, Width, Height, VBS3 18.1.151.441 VBS3_64.exe  -admin -forceSimul -forceRender -nosplash -name=SCREEN_LEFT2 -autoassign=SCREEN_LEFT2 -connect=#auto -cfg=C:\Users\xenon\OneDrive\Documents\VBS3\SCREEN_LEFT2.cfg -cfgProfile=C:\Users\xenon\OneDrive\Documents\VBS3\SERVER.VBS3Profile -cfgNoSave
;MsgBox, -name=SCREEN_LEFT2 is at %X%`,%Y%
if (x >= 0)
{
	WinMove, VBS3 18.1.151.441 VBS3_64.exe  -admin -forceSimul -forceRender -nosplash -name=SCREEN_LEFT2 -autoassign=SCREEN_LEFT2 -connect=#auto -cfg=C:\Users\xenon\OneDrive\Documents\VBS3\SCREEN_LEFT2.cfg -cfgProfile=C:\Users\xenon\OneDrive\Documents\VBS3\SERVER.VBS3Profile -cfgNoSave, , -1920, 0 
}
return