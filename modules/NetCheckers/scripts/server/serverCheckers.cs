// serverCheckers - master server checker game
function serverLaunchGame()
{
  //we are now in a match
  $inMatch = true;
  
  //init the server's boards
  ServerCheckerBoard.init();
  ClientCheckerBoard.init();
  
  //tell the client to start the game
  commandToClient($playerConnection, 'StartGame');
}

function serverBeginGame()
{
  //set turn to server's turn
  setTurn($serverConnection);
}

function setTurn(%player)
{
  //get the other player
  %otherPlayer = getOtherPlayer(%player);
  
  //call the client's set turn function properly
  commandToClient(%otherPlayer, 'setPlayerTurn', 0);
  commandToClient(%player, 'setPlayerTurn', 1);
  
  //set the server's turn properly
  $playersTurn = %player;
}

function getOtherPlayer(%player)
{
  //check to see what play was passed and return the other player
  if(%player == $playerConnection)
  {
    %otherPlayer = $serverConnection;
  }
  else
  {
    %otherPlayer = $playerConnection;
  }
  
  return %otherPlayer;
}

function ServerCheckerBoard::init(%this)
{
  //init the team counts
  $redCount = 0;
  $blueCount = 0;
  
  //loop through and create the server board
  for(%i=1;%i<65;%i++)
  {
     //check if this is a black checkerboard spot
     if(%this.isBlack(%i))
     {
        //check if this is a checker's starting spot
        if(%i < 24)
        {
           //set the blue team's piece and add to the count
           %this.setPiece(%i, $blue);
           $blueCount++;
        }
        else if(%i > 41)
        {
           //set the red team's piece and add to the count
           %this.setPiece(%i, $red);
           $redCount++;
        }
        else
        {
           //set the blank spot
           %this.setPiece(%i, 0);
        }
     }
     else
     {
        //set the blank spot
        %this.setPiece(%i, 0);
     }
  }
}

function serverAttemptSelectChecker(%client, %pos)
{
  //check if it's this client's turn
  if($playersTurn != %client)
  {
    //it is not this client's turn, nice try
    commandToClient(%client, 'notYourTurn');
  }
  else if($playersTurn == %client)
  {
    //grab the selection
    %selection = ServerCheckerBoard.getPiece(%pos);
    
    //check if this is the client's piece, or a king
    if(%selection == %client.team)
    {
      //proper move, process sit
      serverSelectChecker(%client, %pos);
    }
    else
    {
      //the client cannot move another team's piece
      commandToClient(%client, 'selectCheckerResponse', %pos, 0);
    }
  }
}

function serverSelectChecker(%client, %pos)
{
  //set the selected piece
  $playersTurn.selectedPos = %pos;
  $playersTurn.hasSelected = true;
  
  //respond to the client
  commandToClient(%client, 'selectCheckerResponse', %pos, 1);
}

function serverAttemptMovePiece(%client, %pos)
{
  //grab where the move is legal
  %legal = ServerCheckerBoard.isLegalMove($playersTurn.selectedPos, %pos);
  %jump = getWord(%legal, 1);
  //init jumped to false
  %jumped = false;
  
  //if we receive a value for %jump than that's the position of a jumped piece
  if(%legal && %jump > 0)
  {
    //grab the jumped piece
    %jumpedPiece = ServerCheckerBoard.getPiece(%jump);
    
    //grab the other player
    %otherPlayer = getOtherPlayer(%client);
    
    //remove the piece
    ServerCheckerBoard.removePiece(%jump);
    
    //tell the client's to remove the piece
    commandToClient(%client, 'destroyPiece', %jump);
    commandToClient(%otherPlayer, 'destroyPiece', %jump);
    
    //decrement the proper count
    if(%jumpedPiece == $red)
    {
      $redCount--;
    }
    else
    {
      $blueCount--;
    }
    
    //check if this means the game is over
    serverEndGameCheck();
    
    //toggle jumped to true;
    %jumped = true;
  }
  
  //check if the move is legal
  if(%legal)
  {
    //grab the type of piece that is selected
    %type = ServerCheckerBoard.getPiece($playersTurn.selectedPos);
    
    //grab the other player
    %otherPlayer = getOtherPlayer(%client);
    
    //tell the client to move the piece since this was a legal move
    commandToClient(%otherPlayer, 'moveCheckerPiece', %type, $playersTurn.selectedPos, %pos);
    commandToClient(%client, 'movePieceResponse', %pos, 1);
    
    //set the proper piece settings
    ServerCheckerBoard.setPiece($playersTurn.selectedPos, 0);
    ServerCheckerBoard.setPiece(%pos, %type);

    //reset the player's selection
    $playersTurn.selectedPos = "";
    $playersTurn.hasSelected = false;
    
    //if this wasn't a jump then we just swap turns
    if(%jumped)
      commandToClient(%client, 'setClientJumping', %pos);
    else
      swapTurns();
  }
  else
  {
     // Client has bad information
     error("Warning! Corrupt client information passed to server.");
  }
}

function swapTurns()
{
  //get the other player
  %player = getOtherPlayer($playersTurn);
  
  $clientJumping = false;
  
  //set the turn to the other player
  setTurn(%player);
}

function serverEndGameCheck()
{
   if($redCount == 0)
   {
     commandToClient($serverConnection, 'youWin');
     commandToClient($playerConnection, 'youLose');
   }
   else if($blueCount == 0)
   {
     commandToClient($serverConnection, 'youLose');
     commandToClient($playerConnection, 'youWin');
   }
}