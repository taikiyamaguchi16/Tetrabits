StrixActionGameSample
=====================

This project is a online action game sample using StrixCloud.
Synchronous processing is realized using StrixUnitySDK components and RPC.

To check the operation, open the main scene in Assets/Scenes/SampleScene.

Next, set the destination server for communication.
Create a server from the Web console screen of StrixCloud.

If you select StrixConnectUI > StrixConnectPanel from the hierarchy window, 
there is a component called Strix Connect GUI in the inspector window.
Set the host address and application ID of the server created on the Web Console in this Host and ApplicationId.

You can now play and connect to the server.
If you want to check multiplayer synchronization, activate multiple processes of the project build and check.


A more detailed explanation can be found in the StrixCloud documentation.

https://www.strixcloud.net/private/docs/manual/en/unity/html/servers.html
https://www.strixcloud.net/private/docs/manual/en/unity/html/unitysdk/connect.html

