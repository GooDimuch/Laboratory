#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.


; Send {w down}  ; Press down the up-arrow key.
; Send {shift down}  ; Press down the up-arrow key.
; Sleep 3000  ; Keep it down for one second.
; Send {W up}  ; Release the up-arrow key.
; Send {Shift up}  ; Release the up-arrow key.


; RButton::
; Toggle := !Toggle
; If Toggle
; 	Send {w down}
; 	Send {shift down}
; else
; 	Send {W up}
; 	Send {Shift up}
; return

RShift::
	toggle := !toggle
	state := (toggle ? "down" : "up")  ;if toggle = 1 send w down, if toggle = 0 send w up
	send {w %state%}
	send {LShift %state%}
return

~s::
	toggle := 0
	state := "up"  ;if toggle = 1 send w down, if toggle = 0 send w up
	send {w %state%}
	send {LShift %state%}
return

~w::
	toggle := 0
	state := "up"  ;if toggle = 1 send w down, if toggle = 0 send w up
	send {LShift %state%}
return

~m::
	toggle := 0
	state := "up"  ;if toggle = 1 send w down, if toggle = 0 send w up
	send {LShift %state%}
return


~tab::
	SetKeyDelay, 20
	if !GetKeyState("LShift")
		return
	toggle := 0
	state := "up"  ;if toggle = 1 send w down, if toggle = 0 send w up
	send {LShift %state%}
	; send {tab}
return