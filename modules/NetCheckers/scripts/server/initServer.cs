// initServer - initialize server
function initServer()
{
  //Create an object to act as our server check board
  %board = new CompositeSprite(ServerCheckerBoard){
    class = "CheckerBoard";};

  %newBoard = createCheckerBoard(%board);
  //init the inmatch value to false;
  $inMatch = false;
}

function onServerCreated()
{
  //toggle first connection to true
  $firstConnection = true;
}

function onClientConnected(%client)
{
  // If this is the first connection, it's local, second is client
  // any beyond is sent an "in match" message
  if($firstConnection)
  {
    $serverConnection = %client;
    setTeam(%client, $red);
    
    $firstConnection = false;
    
    initClient();
    initServer();
  }
  else if(!$firstConnection)
  {
    if($inMatch)
    {
      commandToClient(%client, 'inMatch');
    }
    else
    {
      $playerConnection = %client;
      setTeam(%client, $blue);
      
      commandToClient(%client, 'initClient');
      serverLaunchGame();
    }
  }
}

function setTeam(%client, %team)
{
  //store the team for the client
  %client.team = %team;
  
  //tell the client what team they are
  commandToClient(%client, 'setTeam', %team);
}