^F2::
InsertDiscord(".mute")
; MsgBox mute
return

^F3::
InsertDiscord(".unmute")
; MsgBox unmute
return

InsertDiscord(DiscordText)
{
	WinGet, DiscordState, MinMax, ahk_exe Discord.exe
	If (DiscordState = -1) ; If Discord is minimized, then restore window
	WinRestore, ahk_exe Discord.exe

	ControlFocus,, ahk_exe Discord.exe
	sleep, 300
	ControlSend,, %DiscordText%{Enter}, ahk_exe Discord.exe

	If (DiscordState = -1) ; If Discord was previously minimized, return to minimized state
	WinMinimize, ahk_exe Discord.exe
	Return
}