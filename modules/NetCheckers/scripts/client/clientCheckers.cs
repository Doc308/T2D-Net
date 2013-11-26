// clientCheckers - local client checkers game
function clientLaunchGame()
{
  //init the client checkerboard
  ClientCheckerBoard.init();

  //call the game started function on the server
  commandToServer('gameStarted');
}

function ClientCheckerBoard::init(%this)
{
  //create container object's for the team
  %this.redPieces = new SimSet();
  %this.bluePieces = new SimSet();
  //loop through and create the checker board and the initial checkers
  for(%i=1;%i<65;%i++)
  {
     //check if the checker board spot is black
     if(%this.isBlack(%i))
     {
        //check to see if we're populating the checker's starting area
        if(%i < 24)
        {
          //create a new blue checker piece
          %piece = %this.initPiece($blue);

          //set the piece to this location
          %this.setClientPiece(%i, $blue, %piece);
        }
        else if(%i > 41)
        {
          //create a new red piece
          %piece = %this.initPiece($red);
          
          //set this piece to this location
          %this.setClientPiece(%i, $red, %piece);
        }
        else
        {
          //set the empty space
          %this.setClientPiece(%i, 0);
        }
     }
     else
     {
        %this.setClientPiece(%i, 0);
     }
  }
}

function ClientCheckerBoard::initPiece(%this, %type)
{
  //check which team the piece is
  if(%type == $red)
  {
    //get the count of current pieces of that color
    %num = %this.redPieces.getCount();
    
    //create a new piece as a clone of our stored piece
    %class = "RedChecker";
    %image = "NetCheckers:sun_neutral";
    %piece = createChecker(%type, %class, %image);
    %piece.setName("redPiece" @ %num);
    
    //add the piece to its color's container object
    %this.redPieces.add(%piece);
  }
  else if(%type == $blue)
  {
    //get the count of current pieces of that color
    %num = %this.bluePieces.getCount();
    
    //create a new piece as a clone of our stored piece
    %class = "BlueChecker";
    %image = "NetCheckers:moon_neutral";
    %piece = createChecker(%type, %class, %image);
    %piece.setName("bluePiece" @ %num);
    
    //add the piece to its color's container object
    %this.bluePieces.add(%piece);
  }
  
  return %piece;
}

function createChecker(%type, %class, %image)
{
   %checker = new Sprite(){
      class = %class;};
   %checker.Size = "6 6";
   %checker.Image = %image;
   %checker.SceneLayer = 20;
   myScene.add(%checker);
   return %checker;
}

function ClientCheckerBoard::setClientPiece(%this, %pos, %type, %piece)
{
  //if the piece isn't empty we do some extra settings
  if(%type != 0)
  {
    //grab the position of the piece
    %this.selectSpriteId(%pos);
    %sPos = %this.getSpriteLocalPosition();
    
    //set the position of the piece
    %piece.setPosition(%sPos);
    //store this Sprite to the board index
    %this.checkerPiece[%pos] = %piece;
  }
  //set the piece's type
  %this.setPiece(%pos, %type);
}

function attemptSelectChecker(%pos)
{
   //first we check if it is our turn
  if(!$myTurn)
  {
     echo("Not your turn");
  }
  else if($clientSelected)
  {
     //this means we already have a checker selected, call attempt move
    attemptMovePiece(%pos);
  }
  else
  {
    //grab the selected piece
    %selection = ClientCheckerBoard.getPiece(%pos);
    
    //if it's of the right team then attempt to check it
    if(%selection == $playerTeam)
    {
      //ask the server if we can select it
      commandToServer('attemptSelectChecker', %pos);
    }
    else
    {
      //let the console know this is an invalid selection
      echo("Invalid Selection - Attempt Selection");
    }
  }
}

function clientSelectChecker(%pos)
{
  //set the selection with what's passed in
  $clientSelectedPos = %pos;
  $clientSelected = true;
  
  //have the checker follow the touch device
  ClientCheckerBoard.checkerFollowTouch(%pos);
}

function ClientCheckerBoard::checkerFollowTouch(%this, %pos)
{
  //grab the checker
  %checker = %this.checkerPiece[%pos];
  
  //store the selected checker
  $clientSelectedPiece = %checker;
  
  //set the checker to the $touchObj
  $touchObj = %checker;
}

function attemptMovePiece(%pos)
{
  //check if the move is legal
  %legal = ClientCheckerBoard.isLegalMove($clientSelectedPos, %pos);
  //check if moving to the same position, if so reset the piece
  if(%legal $= "same")
  {
    //dismount the checker from the mouse object
    $touchObj = 0;
    //reset the checker's position to the original position
    ClientCheckerBoard.selectSpriteId(%pos);
    %sPos = ClientCheckerBoard.getSpriteLocalPosition();
    $clientSelectedPiece.setPosition(%sPos);
    
    //set selected to false
    $clientSelected = false;
  }
  else if(%legal)
  {
    //request a move from the server
    commandToServer('attemptMovePiece', %pos);
  }
}

function clientMoveSelectedPiece(%pos)
{
  //dismount the selection from the touch system and set its position to the new spot
  $touchObj = 0;
  ClientCheckerBoard.selectSpriteId(%pos);
  %sPos = ClientCheckerBoard.getSpriteLocalPosition();
  $clientSelectedPiece.setPosition(%sPos);
  
  //grab the position information
  %piece = ClientCheckerBoard.checkerPiece[$clientSelectedPos];
  ClientCheckerBoard.checkerPiece[$clientSelectedPos] = "";
  ClientCheckerBoard.checkerPiece[%pos] = %piece;

  //grab the object's type
  %type = ClientCheckerBoard.getPiece($clientSelectedPos);
  
  //set the pieces position
  ClientCheckerBoard.setPiece($clientSelectedPos, 0);
  ClientCheckerBoard.setPiece(%pos, %type);
  
  //reset the client's selection
  $clientSelectedPos = "";
  $clientSelected = false;
}

function clientMovePiece(%type, %from, %pos)
{
  //get the piece's image
  %piece = ClientCheckerBoard.checkerPiece[%from];
  
  //set the position of the piece
  ClientCheckerBoard.selectSpriteId(%pos);
  %sPos = ClientCheckerBoard.getSpriteLocalPosition();
  %piece.setPosition(%sPos);
  
  //reset the image in the previous slot and set the new one
  ClientCheckerBoard.checkerPiece[$clientSelectedPos] = "";
  ClientCheckerBoard.checkerPiece[%pos] = %piece;
  
  //reset the piece in the previous spot and set the new one
  ClientCheckerBoard.setPiece(%from, 0);
  ClientCheckerBoard.setPiece(%pos, %type);
}

function clientDestroyPiece(%pos)
{
  //grab the team and image data of the piece to be destroyed
  %team = ClientCheckerBoard.getPiece(%pos);
  %image = ClientCheckerBoard.checkerPiece[%pos];
  
  //check which team the piece is
  if(%team == $red)
  {
    //remove the piece from the team's container
    ClientCheckerBoard.redPieces.remove(%image);
  }
  else if(%team == $blue)
  {
    //remove the piece from the team's container
    ClientCheckerBoard.bluePieces.remove(%image);
  }
  
  //remove the piece image from the image data
  ClientCheckerBoard.checkerPiece[%pos] = "";
  
  //remove the piece from the board data
  ClientCheckerBoard.removePiece(%pos);
  
  %image.safeDelete();
}

function attemptJumpPiece(%pos)
{
   %legal = ClientCheckerBoard.isLegalMove($clientJumpingPos, %pos);
   %jump = getWord(%legal, 1);
   if(%legal && %jump > 0)
   {
      $clientSelectedPos = $clientJumpingPos;
      attemptMovePiece(%pos);
   }
   else
   {
      //simple way to dismount the checker, get same legal move from attemptMovePiece
      %pos = $clientSelectedPos;
      attemptMovePiece(%pos);
      swapTurns();
   }
   
}