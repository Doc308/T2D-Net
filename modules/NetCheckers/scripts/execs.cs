// Execs.cs - Execute game scripts
exec("./checkerBoard.cs");
exec("./touch.cs");

// Server scripts
exec("./server/initServer.cs");
exec("./server/serverCheckers.cs");
exec("./server/serverCommands.cs");

// Client scripts
exec("./client/initClient.cs");
exec("./client/clientCheckers.cs");
exec("./client/clientCommands.cs");