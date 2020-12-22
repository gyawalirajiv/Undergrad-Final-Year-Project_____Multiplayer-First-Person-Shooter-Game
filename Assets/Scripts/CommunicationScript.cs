//Attached to MultiplayerManager

using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the MultiplayerManager and it is
/// the foundation for our multiplayer system.
/// 
/// This script is accessed by the cursor control script.
/// 
/// This script accesses the ScoreTable script to inform it of the winning score criteria.
/// </summary>

public class CommunicationScript : MonoBehaviour {
	
	private string titleMessage = "FYP FPS Game";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26500;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 10; //Number of players allowed in the game
	public string playerName;
	public string serverName;
	public string serverNameForClient;
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	
	
	//The main starting window is defines by these parameters
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;
	public Texture2D newTexure;
	
	
	//The serverdiscooneect window is defined with these variables.
	private Rect serverDisWindowRect;
	private int serverDisWindowsWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;
	
	
	//The client disconnect window is defined by these parameters
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;
	public bool showDisconnectWindow = false; //This variable is accessed globally by CursorControl script.
	
	//Trhe winning scores are defined in here, yet the winning score are static till now
	public int winningScore = 20;
	private int scoreButtonWidth = 40;
	private GUIStyle plainStyle = new GUIStyle();
	
	
	void Start () {
		//Get the registry file name serverName, that contains the previous serverName information
		serverName = PlayerPrefs.GetString("serverName");
		
		if(serverName == ""){
			serverName = "Server";
		}
		
		//Load last used playerName from registry
		//If the playerName is blank then use "Player" as a default name.
		playerName = PlayerPrefs.GetString("playerName");
		
		if(playerName == ""){
			playerName = "Player";
		}
		
		plainStyle.alignment = TextAnchor.MiddleLeft;
		plainStyle.normal.textColor = Color.white;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			showDisconnectWindow = !showDisconnectWindow;
			
		}
	}
	
	void ConnectWindow(int windowID){
		//Leave a gap from the header
		GUILayout.Space(15);
		
		
		//As the game starts the option to create server and connect to a server are displayed via buttons.
		//When the player presses the start server button he sets the iWantToSetupAServer to true.
		//When the player presses the connect to the server button he sets the iWantToConnectToAServer to true.
		if(iWantToSetupAServer == false && iWantToConnectToAServer == false){
			
			GUI.color = Color.blue;
			
			GUILayout.BeginArea(new Rect(0, 0, 100, 100));
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal("");
			if(GUILayout.Button("Start a Server", GUILayout.Height(Screen.height / 2), GUILayout.Width((Screen.width / 2) - 50))){
				iWantToSetupAServer = true;
			}
			//GUILayout.Space(10);
			
			if(GUILayout.Button("Connect to a Server", GUILayout.Height(Screen.height / 2), GUILayout.Width((Screen.width / 2) - 50))){
				iWantToConnectToAServer = true;
			}
			GUILayout.EndVertical();
			
			if(Application.isWebPlayer == false && Application.isEditor == false){
				if(GUILayout.Button("Exit Game", GUILayout.Height(buttonHeight))){
					Application.Quit();
				}
			}
			GUILayout.EndArea();
			
		}
		
		if(iWantToSetupAServer == true){
			GUI.color = Color.blue;
			
			
			//The player inputs their name via this text field.
			GUILayout.Label("Server Name:");
			//The serveName in the parameters show the default values from the registery.
			serverName = GUILayout.TextField(serverName);
			GUILayout.Space(5);
			//The can type in the Port number for their server into textfield.
			//We defined a default value above in the variables as 26500.
			GUILayout.Label("Server Port");
			
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Start Server", GUILayout.Height(30))){
				//Start the server (donot know about useNAT(false))
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);
				
				//Save the serverName using PlayerPrefebs.(it is saved in the registery for windows users)
				PlayerPrefs.SetString("serverName", serverName);
				
				//Pass the winnign score parameter to ScoreTable
				TellEveryoneWinningCriteria(winningScore);
				
				
				iWantToSetupAServer = false;
			}
			if(GUILayout.Button("Go Back", GUILayout.Height(30))){
				iWantToSetupAServer = false;
			}
		}
		
		if(iWantToConnectToAServer == true){
			GUI.color = Color.blue;
			//The player inputs thir playername here
			GUILayout.Label("Player name");
			
			playerName = GUILayout.TextField(playerName);
			
			GUILayout.Space(5);
			
			//The player inputs the IP address of the server they want to connect to.
			GUILayout.Label("Server IP");
			
			connectToIP = GUILayout.TextField(connectToIP);
			
			GUILayout.Space(5);
			
			//The player types in th eport number of the server they want to connect to
			GUILayout.Label("Server Port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(5);
			
			//This is the button creation, the player presses it to connect.
			if(GUILayout.Button("Connect", GUILayout.Height(25))){
				//This makes the empty named player have a name
				if(playerName == ""){
					playerName = "Player";
				}
				
				//If the player has a name that isnot empty then accept to join the servre
				if(playerName != ""){
					//Connect to the server with the given IP and Port
					Network.Connect(connectToIP, connectionPort);
					
					//save the playername in the registery.
					PlayerPrefs.SetString("playerName", playerName);
				}
				
			}
			
			GUILayout.Space(5);
			if(GUILayout.Button("Go Back", GUILayout.Height(25))){
				iWantToConnectToAServer = false;
				//iWantToSetupAServer = false;
			}
		}
		
	}
	
	void ServerDisconnectWindow(int windowID){
		GUI.color = Color.blue;
		
		GUILayout.Label("Server Name: " + serverName);
		
		//This shows the number of players connected.
		GUILayout.Label("Number of players connected: " + Network.connections.Length);
		
		//Show the average ping of the connected layers, if players are more than one.
		if(Network.connections.Length >= 1){
			GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
		}
		
		//This button shutsdown the server.
		if(GUILayout.Button("Shutdown Server")){
			Network.Disconnect();
		}
	}
	
	void ClientDisconnectWindow(int windowID){
		GUI.color = Color.blue;
		
		//Show the name of the server connected and show the server average ping rate.
		GUILayout.Label("Connected to server: " + serverName);
		
		GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
		
		GUILayout.Space(7);
		
		//The player disconnects from the server when they press the disconnect button.
		if(GUILayout.Button("Disconnect", GUILayout.Height(25))){
			Network.Disconnect();
		}
		
		GUILayout.Space(5);
		
		//This button allows the user to resume the game while it had been paused.
		if(GUILayout.Button("Return To Gmae", GUILayout.Height(25))){
			showDisconnectWindow = false;
		}
	}
	
	//This is a internal function of Unity 3d
	void OnDisconnectedFromServer(){
		//If a player loses the connection or leaves the scene then the level is restarted
		Application.LoadLevel(Application.loadedLevel);
	}
	
	//This is a internal function of Unity 3d
	void OnPlayerDisconnected(NetworkPlayer networkPlayer){
		//When a player disconnects from the server remove his RPC so that noone sees him.
		Network.RemoveRPCs(networkPlayer);
		
		Network.DestroyPlayerObjects(networkPlayer);
	}
	
	
	void OnPlayerConnected(NetworkPlayer networkPlayer){
		//This updates all the new players with teh servername
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
		
		networkView.RPC("TellEveryoneWinningCriteria", networkPlayer, winningScore);
	}
	
	void OnGUI(){
		//When the player disconnects show the ConnectWindow function.
		if(Network.peerType == NetworkPeerType.Disconnected){
			//Draw the window acording to teh screen height and width.
			leftIndent = (Screen.width / 2) - connectionWindowWidth / 2;
			
			
			
			topIndent = (Screen.height / 2) - connectionWindowHeight / 2;
			
			
			//Graphics.DrawTexture(Rect(10, 10, 100, 100), newTexure);
			//connectionWindowRect = new Rect(leftIndent, topIndent, connectionWindowWidth, connectionWindowHeight);
			connectionWindowRect = new Rect(0, 0, Screen.width, Screen.height);
			
			// Here the 0 is the WindowID and ConnectWindow is a function that is placed on this same script that gets run.
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titleMessage);
			
			
		}
		
		//If the game is running as a server then run the ServerDisconnectWindow function
		if(Network.peerType == NetworkPeerType.Server){
			//Server disonnect button creates a Rect here.
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowsWidth, serverDisWindowHeight);
			
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
			
			//This allows the server to set the winning scroe via GUI
			//GUI.Box(new Rect(10, 190, 300, 70), "");
			//GUILayout.BeginArea(new Rect(20, 200, 280, 60));
			//GUILayout.BeginHorizontal();
			//GUILayout.Label("Winning Score", plainStyle, GUILayout.Width(100), GUILayout.Height(scoreButtonWidth));
			//GUILayout.Label(winningScore.ToString(), plainStyle, GUILayout.Width(40), GUILayout.Height(scoreButtonWidth));
			
			/*if(GUILayout.Button("+", GUILayout.Width(scoreButtonWidth), GUILayout.Height(scoreButtonWidth))){
				if(winningScore >= 10){
					winningScore = winningScore + 10;
				}
				if(winningScore < 10){
					winningScore = winningScore + 9;
				}
				networkView.RPC("TellEveryoneWinningCriteria", RPCMode.All, winningScore);
			}*/
			//GUILayout.EndHorizontal();
			//GUILayout.EndArea();
			
			/*if(GUILayout.Button("-", GUILayout.Width(scoreButtonWidth), GUILayout.Height(scoreButtonWidth))){
				winningScore = winningScore - 10;
				if(winningScore <= 0){
					winningScore = 1;
				}
			}*/
			
		}
		
		
		//The client players can see the window to disconnect from the server.
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true){
			clientDisWindowRect = new Rect(Screen.width / 2 - clientDisWindowWidth / 2, Screen.height / 2 - clientDisWindowHeight / 2, clientDisWindowWidth, clientDisWindowHeight);
			
			clientDisWindowRect = GUILayout.Window(1, clientDisWindowRect, ClientDisconnectWindow, "");
		}
		
	}
	
	//This tells the Comunication script to pass trhe servername through out the server network.
	[RPC]
	void TellPlayerServerName(string servername){
		serverName = servername;
	}
	
	//This RPC will send the winning criteria throughout the network..
	[RPC]
	void TellEveryoneWinningCriteria(int winScore){
		GameObject gameManager = GameObject.Find("GameManager");
		ScoreTable scoreScript = gameManager.GetComponent<ScoreTable>();
		scoreScript.winningScore = winScore;
	}
	
}