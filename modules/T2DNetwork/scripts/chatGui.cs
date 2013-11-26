function chatGui::onWake(%this)
{
   if(isObject(chatVectorText))
   {
      chatMessageText.detach();
      chatVectorText.delete();
   }

   new MessageVector(chatVectorText){};
   chatMessageText.attach(chatVectorText);
   
   chatVectorText.clear();
   
   chatClientList.clear();

   echo("getting clients");

   %this.getClients(); 
   commandToServer('isChatting');
}

function chatGui::onSleep(%this)
{   
   if(serverData.local)
   { 
      //If we are the server we want to send "chatClosed" to the client to close out their chats
      sendChatClosed();
   } else
   {
      //Tell the server we left the chat  
      commandToServer('leftChat');
   }
}

function chatGui::send(%this)
{
   %text = chatEditText.getValue();
   commandToServer('updateChatText', %text);
   chatEditText.setValue("");
}

function chatGui::getClients(%this)
{
   $waitingForList = true;
   commandToServer('getChatConnectionList');

}

function chatGui::onGetList(%this)
{
   $waitingForList = false;
   %count = $clientChatConnectionCount;

   for(%i=0;%i<%count;%i++)
   {
      clientCmdupdateChatClient($clientChatNamesList.contents[%i]);    

   }
}

function chatGui::quit(%this)
{
   Canvas.popDialog(ChatGui);
}
