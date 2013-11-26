// initClient - Initialize client
function initClient()
{
   //Create an object to act as our server check board
   %board = new CompositeSprite(ClientCheckerBoard){
     class = "CheckerBoard";};
   %newBoard = createCheckerBoard(%board);
   myScene.add(%newBoard);
}