// checkerBoard.cs - Set up our checker board and provide game logic for it
function createCheckerBoard(%board)
{
  // Use a composite Sprite to set up an 8x8 grid for our board
  %board.SceneLayer = 30;
  %board.setDefaultSpriteStride( 8, 8 );
  %board.setDefaultSpriteSize( 8, 8 );
  %board.SetBatchLayout( "rect" );
  // Use 3.5 to keep the board centered, so we start & end equal from 0
  for(%y=-3.5;%y<4;%y++)
  {
    for(%x=-3.5;%x<4;%x++)
    {
      %board.addSprite( %x SPC %y );
      
      if((%x + %y) % 2 == 0)
      {
        %board.setSpriteImage("NetCheckers:checker1");
        %board.setSpriteDepth(0); // Depth 0 = White
      }
      else
      {
        %board.setSpriteImage("NetCheckers:checker2");
        %board.setSpriteDepth(1); // Depth 1 = Black
      }
    }
  }
  return %board;
}

function CheckerBoard::isBlack(%this, %pos)
{
   // Check the Depth of the sprite at selected position
   %this.selectSpriteId(%pos);
   if(%this.getSpriteDepth() == 1)
     return true;
   else
     return false;
}

function CheckerBoard::setPiece(%this, %pos, %type)
{
   // Set the board position
   %this.board[%pos] = %type;
}

function CheckerBoard::getPiece(%this, %pos)
{
   // Return the piece at this position
   return %this.board[%pos];
}

function CheckerBoard::removePiece(%this, %pos)
{
   // Set the board position to 0
   %this.board[%pos] = 0;
}

function CheckerBoard::isLegalMove(%this, %from, %to)
{
   // Check if moved to same location
   if(%from == %to)
     return "same";
   
   // What color is it?
   %piece = %this.getPiece(%from);
   // Make sure it's a black square
   if(!%this.isBlack(%to))
     return false;
   
   // Make sure another piece isn't there
   %movePos = %this.getPiece(%to);
   // If a piece is there, we can't move there
   if(%movePos != 0)
     return false;
   // Check we are moving in the correct direction
   switch(%piece)
   {
      case 0: // Sanity
        return false;
      case 1: // Red team going up
        if(%from < %to)
          return false;
      case 2: // Blue team going down
        if(%from > %to)
          return false;
   }
   
   // Make sure we traveled a legal distance
   %dist = mAbs(%from - %to);
   // If we moved 1 position (which is 8 +/- 1)
   if(%dist == 7 || %dist == 9)
     return true;
   // Jump check (double the last check)
   else if(%dist == 14 || %dist == 18)
   {
      // First get the jump spot, which is the distance divided by 2
      if(%from < %to)
        %jumpSpot = %from + (%dist/2);
      else 
        %jumpSpot = %to + (%dist/2);
      %normal = getNormalPiece(%piece); 
      %vector = true SPC %jumpSpot;  
      // Check if jump spot is our own team or nothing, otherwise jump
      if((%jumpSpot == %normal) || (%jumpSpot == 0))
        return false;
      else
        return %vector;
   }
   // Moved more than allowed
   return false;
}

function getNormalPiece(%team)
{
   // Check what team is being passed and return proper normal piece
   if(%team == $red)
     %piece = $red;
   else if(%team == $blue)
     %piece = $blue;
   
   return %piece;
}