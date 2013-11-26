//How to use this example
//1. Launch two versions of T2D
//2. Click StartServer button. You should see a connected message (the server computer connects as a client to itself).
//3. In the console on your client click JoinServer button. Click QueryLan, then Join Server. You should see a connected message.
//4. In the console on the server run 'serversayhi("NAME")'. Where NAME is your name or whatever.
//5. Check the console on the client and you should see "NAME (server) says hi!"
//6. In the console on the client run 'clientsayhi("NAME")'. Where NAME is your name or whatever. 
//7. Check the console on the client and you should see "NAME (client) says hi!"
//8. To load up our chat gui, type 'loadChat' on the Server, then the Client.

//Create a server on this computer
function startserver()
{
   echo("creating server");
   createServer(true);
}

//Tell this client to connect to the server
function connect(%ip)
{  
   echo("connecting to server");
   connecttoserver(%ip);
}

//THIS IS CALLED ON THE CLIENT
//Tells the server to execute serverCmdHi with our name as an argument
function clientsayhi(%name)
{
//the function calls a command on the server called serverCmdHi below
//this converts to serverCmdHi function and executes on the server
   commandToServer('Hi', %name); 
}

//THIS EXECUTES ON THE SERVER FROM THE commandToServer CALL ABOVE
//we take the commandToServer 'Hi' argument from above and 
//append it to the serverCmd to create this callback
function serverCmdHi(%client, %name)
{
   echo(%name SPC "(client) says hi!");
}

//THIS EXECUTES ON THE SERVER
//this loops through all the clients and tells each to 
//execute the clientCmdHi function with our name as an argument
function serversayhi(%name)
{
   %count = ClientGroup.getCount();
   for(%i = 0; %i < %count; %i++)
   {
      %recipient = ClientGroup.getObject(%i);
      //this converts to clientCmdHi function and executes on the client
      commandToClient(%recipient, 'Hi', %name);
   }
}

//THIS EXECUTES ON EACH CLIENT FROM THE commandToClient CALL ABOVE
//we take the commandToClient 'Hi' argument from above and 
//append it to the ClientCmd to create this callback
function clientCmdHi(%name)
{
  echo(%name SPC "(server) says hi!");
}