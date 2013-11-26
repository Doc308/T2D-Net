function mySceneWindow::onTouchUp(%this, %touchID, %worldPosition)
{
   // If the client checkerboard is created, pick the spot
   if(isObject(ClientCheckerBoard))
   {
      // Pick sprites
      %sprites = ClientCheckerBoard.pickPoint(%worldPosition);
      
      // Get count (more than 1 means selection is close to multiple)
      %spriteCount = %sprites.count;
      
      // Make sure a spot on the board is picked
      if(%spriteCount == 0)
        return;
        
      // Only 1 sprite selected
      if(%spriteCount == 1)
      {
         %pos = getWord(%sprites, 0);
         
         if($clientJumping)
           attemptJumpPiece(%pos);
         else
           attemptSelectChecker(%pos);
      }
      else
        echo("Invalid Selection - Touch");
   }
}

function mySceneWindow::onTouchMoved(%this, %touchID, %worldPosition)
{
   if($touchObj == 0)
     return;
   // Set the position of the object to follow the touch control
   $touchObj.setPosition(%worldPosition);
}