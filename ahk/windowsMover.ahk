; Example #1: Close unwanted windows whenever they appear:
#Persistent
SetTimer, CloseMailWarnings, 1000
return

CloseMailWarnings:
WinGetPos, X, Y, Width, Height, SCREEN_TITLE
;MsgBox, AutoHotkey is at %X%`,%Y%
if (x < 1920)
{
	WinMove,SCREEN_TITLE,,1920, 0 
}
return