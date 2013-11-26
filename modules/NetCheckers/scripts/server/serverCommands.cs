// serverCommands - commands for the server to run
function serverCmdGameStarted(%client)
{
  //tell the server to begin the game
  serverBeginGame();
}

function serverCmdAttemptSelectChecker(%client, %pos)
{
  //pass the attempted selection on to the server function
  serverAttemptSelectChecker(%client, %pos);
}

function serverCmdAttemptMovePiece(%client, %pos)
{
  //pass the attempted move values on to the server function
  serverAttemptMovePiece(%client, %pos);
}