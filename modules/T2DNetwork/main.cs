function T2DNetwork::create( %this )
{
  // Load GUI profiles.
  exec("./gui/guiProfiles.cs");
  
  // Create the scene window
  exec("./scripts/scenewindow.cs");
  createSceneWindow();
  
  // Create the scene
  exec("./scripts/scene.cs");
  createScene();
  mySceneWindow.setScene(myScene);
  
  // Load and configure the console.
  exec("./scripts/console.cs");
  TamlRead("./gui/ConsoleDialog.gui.taml"); //Notice we are just reading in the Taml file, not adding it to the scene
  GlobalActionMap.bind( keyboard, "ctrl tilde", toggleConsole );
  
  // Network Prefs
  $pref::Master0 = "2:master.garagegames.com:28002";
  $pref::Net::LagThreshold = 400;
  $pref::Net::Port = 28000;
  $pref::Server::RegionMask = 2;
  $pref::Net::RegionMask = 2;
   
  $pref::Server::Name = "T2D Server";
  $pref::Player::Name = "T2D Player";
   
  // Set up networking
  setNetPort(0);
   
  // Gui Related scripts
  exec("./scripts/messageBox.cs");
  exec("./scripts/chatGui.cs");
  exec("./scripts/startServerGui.cs");
  exec("./scripts/joinServerGui.cs");
   
  // Network structure
  exec("./scripts/client/chatClient.cs");
  exec("./scripts/client/client.cs");
  exec("./scripts/client/message.cs");
  exec("./scripts/client/serverConnection.cs");
   
  exec("./scripts/server/chatServer.cs");
  exec("./scripts/server/clientConnection.cs");
  exec("./scripts/server/kickban.cs");
  exec("./scripts/server/message.cs");
  exec("./scripts/server/server.cs");
   
  // Load the Network Gui's
  TamlRead("./gui/NetworkMenu.gui.taml");
  TamlRead("./gui/startServer.gui.taml");
  TamlRead("./gui/joinServer.gui.taml");
  TamlRead("./gui/chatGui.gui.taml");
  TamlRead("./gui/waitingForServer.gui.taml");
  TamlRead("./gui/messageBoxOk.gui.taml");
  
  // Network Checkers
  ModuleDatabase.loadExplicit("NetCheckers");
   
  Canvas.pushDialog(NetworkMenu);
}

function T2DNetwork::destroy()
{
  // Destroy the scene window
  destroySceneWindow();
}