function startServerGui::onWake(%this)
{
   if($pref::Server::port $= "")
      $pref::Server::port = 28000;
      
   if($pref::Server::MaxPlayers $= "")
      $pref::Server::MaxPlayers = 32;

   startServerName.setText($pref::Server::Name);
   startServerPlayerName.setText($pref::Player::Name);
   startServerPort.setText($pref::Server::port);
   startServerMaxPlayers.setText($pref::Server::MaxPlayers);
}

function startServerGui::createServer()
{
   %serverName = startServerName.getValue();
   %playerName = startServerPlayerName.getValue();

   $pref::Player::Name = %playerName;
   $pref::Server::Name = %serverName;  

   createServer(true);
   canvas.popDialog(startServerGui);
}
