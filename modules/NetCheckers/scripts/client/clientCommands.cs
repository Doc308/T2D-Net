// clientCommands - commands for the client to run
function clientCmdInitClient()
{
   initClient();
}

function clientCmdInMatch()
{
  //spawn the message telling a client they are rejected, we are in a game without you
  MessageBoxOK("Server in a Match...", "The server you are trying to connect to is already in a match.", "");
}

function clientCmdStartGame()
{
  //call the start game on the client side
  clientLaunchGame();
}

function clientCmdSetTeam(%team)
{
  //set the player's team
  $playerTeam = %team;
}

function clientCmdSetPlayerTurn(%val)
{
  //set the player's turn to the value passed
  $myTurn = %val;
}

function clientCmdNotYourTurn()
{
  //tell the client that you -cannot- play when its NOT your turn
  MessageBoxOK("Not your turn...", "It is currently not your turn.", "");
}

function clientCmdSelectCheckerResponse(%pos, %answer)
{
  //select response, if no we tell client piece can't be selected, if yes select piece
  if(!%answer)
  {
    MessageBoxOK("Invalid Selection...", "You cannot select this checker piece!", "");
  }
  else
  {
    clientSelectChecker(%pos);
  }
}

function clientCmdMovePieceResponse(%pos, %answer)
{
  //here is the server's response to trying to move a piece, if no tell client, if yes move it
  if(!%answer)
  {
    MessageBoxOK("You cannot move...", "You cannot move to the selected spot!", "");
  }
  else
  {
    clientMoveSelectedPiece(%pos);
  }
}

function clientCmdMoveCheckerPiece(%type, %from, %pos)
{
  //this moves the piece directly to a new location
  clientMovePiece(%type, %from, %pos);
}

function clientCmdDestroyPiece(%pos)
{
  //this destroy's a checker piece
  clientDestroyPiece(%pos);
}

function clientCmdYouWin()
{
   MessageBoxOK("You Won!!", "Congratulations!", "");
}

function clientCmdYouLose()
{
   MessageBoxOK("You Lost...", "Better luck next time.", "");
}

function clientCmdSetClientJumping(%pos)
{
   $clientJumping = true;
   $clientJumpingPos = %pos;
   
   commandToServer('attemptSelectChecker', %pos);
}