LButton::
   if (First = 0)
   {
      Clicks := 0
      First := 1
   }
   If (A_ThisHotkey = A_PriorHotkey)
   {
      If (A_TimeSincePriorHotkey <= 157 && A_TimeSincePriorHotkey > 122)
      {
         If (Clicks = 0)
         {
             Clicks := 1
         }
         else
         {
             Clicks := 0
             Return
         }
      }
      else
      {
         Clicks := 0
      }
   }
   else
   {
      Clicks := 0      
   }
   Click Down
   KeyWait, LButton
   Click Up
Return