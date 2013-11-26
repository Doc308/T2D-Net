//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function initChat(%name)
{
      $chatStarted = true;
  if(%name $= "")
		%name = "Torque2D Chat";
      new SimSet(chatGroup);
	chatGroup.name = %name;
}

function onClientConnected(%client)
{ 
}

function onServerCreated()
{
}

function onServerDestroyed()
{
}

function getConnectionList(%client)
{
   $serverConnectionList = "";
   $serverNameList = "";
   $serverConnectionCount = 0;

   %connectionCount = ClientGroup.getCount();

   %count = 0;
   for(%i=0;%i<%connectionCount;%i++)
   {
	%connection = ClientGroup.getObject(%i);
      %name = %connection.name;

      $serverConnectionList = $serverConnectionList @ %connection;
      $serverConnectionList = $serverConnectionList @ "|";

      $serverNameList = $serverNameList @ %name;
      $serverNameList = $serverNameList @ "|";

      %count++;
   }

   $serverConnectionCount = %count;

   commandToClient(%client, 'passConnectionCount', $serverConnectionCount);
   commandToClient(%client, 'passConnectionList', $serverConnectionList);
   commandToClient(%client, 'passNameList', $serverNameList);
}

function getChatConnectionList(%client)
{
   $serverChatConnectionList = "";
   $serverChatNameList = "";
   $serverChatConnectionCount = 0;

   %chatCount = ChatGroup.getCount();

   %count = 0;
   for(%i=0;%i<%chatCount;%i++)
   {
	%chat = chatGroup.getObject(%i);
      %name = %chat.name;
      
      $serverChatConnectionList = $serverChatConnectionList @ %chat;
      $serverChatConnectionList = $serverChatConnectionList @ "|";

      $serverChatNameList = $serverChatNameList @ %name;
      $serverChatNameList = $serverChatNameList @ "|";

      %count++;
   }

   $serverChatConnectionCount = %count;

   commandToClient(%client, 'passChatConnectionCount', $serverChatConnectionCount);
   commandToClient(%client, 'passChatConnectionList', $serverChatConnectionList);
   commandToClient(%client, 'passChatNameList', $serverChatNameList);
}

function serverCmdGetConnectionList(%client)
{
   getConnectionList(%client);
}

function serverCmdGetChatConnectionList(%client)
{
   getChatConnectionList(%client);
}

function serverCmdisChatting(%client)
{
   chatGroup.add(%client);
   updateChatClient(%client.name);
}

function serverCmdleftChat(%client)
{
   chatGroup.remove(%client);
   removeChatClient(%client.name);
}

function serverCmdupdateChatText(%client, %text)
{
   updateChatText(%client.name, %text);
}

function updateChatText(%clientName, %text)
{
   %count = chatGroup.getCount();

   for(%i=0;%i<%count;%i++)	
   {
      %client = chatGroup.getObject(%i);
      commandToClient(%client, 'updateChatText', %clientName, %text);      
   }
}

function updateChatClient(%clientName)
{
   %count = chatGroup.getCount();

   for(%i=0;%i<%count;%i++)
   {
      %client = chatGroup.getObject(%i);
      commandToClient(%client, 'updateChatClient', %clientName);      
   }
}

function removeChatClient(%clientName)
{
   %count = chatGroup.getCount();

   for(%i=0;%i<%count;%i++)
   {
      %client = chatGroup.getObject(%i);
      commandToClient(%client, 'removeChatClient', %clientName);      
   }
}

function onClientDropped(%client)
{  
   if($chatStarted && isObject(ChatGroup))
   { 
      ChatGroup.remove(%client);
      removeChatClient(%client.name);
   }
}

function sendChatLoaded()
{
   $ServerChatLoaded = true;
}

function serverCmdisChatLoaded(%client)
{
   commandToClient(%client, 'isChatLoaded', $ServerChatLoaded);
}

function sendChatClosed(%clientName)
{
   %count = chatGroup.getCount();

   for(%i=0;%i<%count;%i++)
   {
      %client = chatGroup.getObject(%i);
      commandToClient(%client, 'chatClosed');      
   }

   chatGroup.clear();
}
