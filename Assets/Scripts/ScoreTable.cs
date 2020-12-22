using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScoreTable : MonoBehaviour {
	
	public bool shouldIShowScore = false;
	public List<PlayerDataClass> PlayersSorted = new List<PlayerDataClass>();
	private GUIStyle myStyle = new GUIStyle();
	
	private GUIStyle sagarmathaHeaderStyle = new GUIStyle();
	private GUIStyle bagmatiHeaderStyle = new GUIStyle();
	public bool updateSagarmathaScore = false;
	public bool updateBagmatiScore = false;
	public int numberOfEnemiesDestroyedInOneHit;
	public bool shouldServerRefreshScore = false;
	public int sagarmathaTeamScore;
	public int bagmatiTeamScore;
	
	private GUIStyle winStyle = new GUIStyle();
	public bool sagarmathaTeamHasWon = false;
	public bool bagmatiTeamHasWon = false;
	public int winningScore;
	public int waitTime = 7;
	
	void Start () {
		myStyle.fontStyle = FontStyle.Bold;
		myStyle.normal.textColor = Color.white;
		
		sagarmathaHeaderStyle.fontSize = 16;
		sagarmathaHeaderStyle.fontStyle = FontStyle.Bold;
		sagarmathaHeaderStyle.normal.textColor = Color.red;
		
		bagmatiHeaderStyle.fontSize = 16;
		bagmatiHeaderStyle.fontStyle = FontStyle.Bold;
		bagmatiHeaderStyle.normal.textColor = Color.blue;
		
		winStyle.fontSize = 40;
		winStyle.normal.textColor = Color.white;
		winStyle.fontStyle = FontStyle.Bold;
		winStyle.alignment = TextAnchor.MiddleCenter;
		
		GameObject multiManager = GameObject.Find("MultiplayerManager");
		CommunicationScript commScript = multiManager.GetComponent<CommunicationScript>();
		winningScore = commScript.winningScore;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Show Player Scores")){
			shouldIShowScore = true;
		}
		
		if(Input.GetButtonUp("Show Player Scores")){
			shouldIShowScore = false;
		}
		
		if(updateSagarmathaScore == true){
			for(int i = 0; i < numberOfEnemiesDestroyedInOneHit; i++){
				networkView.RPC("UpdateSagarmathaTeamScore", RPCMode.All);
			}
			numberOfEnemiesDestroyedInOneHit = 0;
			updateSagarmathaScore = false;
		}
		
		if(updateBagmatiScore == true){
			for(int i = 0; i < numberOfEnemiesDestroyedInOneHit; i++){
				networkView.RPC("UpdateBagmatiTeamScore", RPCMode.All);
			}
			numberOfEnemiesDestroyedInOneHit = 0;
			updateBagmatiScore = false;
		}
		
		if(Network.isServer && shouldServerRefreshScore == true){
			networkView.RPC("ServerRefreshScore", RPCMode.AllBuffered, sagarmathaTeamScore, bagmatiTeamScore);
			shouldServerRefreshScore = false;
		}
		
		if(bagmatiTeamScore >= winningScore){
			bagmatiTeamHasWon = true;
		}
		if(sagarmathaTeamScore >= winningScore){
			sagarmathaTeamHasWon = true;
		}
	}
	
	void OnGUI(){
		if(shouldIShowScore == true){
			PlayersSorted.Clear();
			
			PlayerDatabase dataScript = transform.GetComponent<PlayerDatabase>();
			
			for(int i = 0; i < dataScript.ListOfPlayers.Count; i++){
				PlayersSorted.Add(dataScript.ListOfPlayers[i]);
			}
			
			PlayersSorted.Sort(delegate(PlayerDataClass player1, PlayerDataClass player2){
				return player1.playerScore.CompareTo(player2.playerScore);
			});
			
			//Display the scoreboard header.
			//*GUI.Box(new Rect(Screen.width / 2 - 260, 10, 520, 30), "");
			//*GUI.Label(new Rect(Screen.width / 2 - 150, 15, 300, 30), "The team that reaches a score of " + winningScore.ToString() + " wins", myStyle);
			
			//Start a new GUI area on the left portion of the screen. This area will be used for displaying red team scores.
			GUILayout.BeginArea(new Rect(Screen.width / 2 - 260, 50, 250, Screen.height - 10));
			
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal("");
			//Display header with the team name and score.
			GUILayout.Label("Sagarmatha Team", sagarmathaHeaderStyle, GUILayout.Width(200));
			GUILayout.Label(sagarmathaTeamScore.ToString(), sagarmathaHeaderStyle, GUILayout.Width(40));
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			
			
			//Go through the PlayersSorted in reverse and pick out each player that belongs to the red team and display their name and score.
			/*for(int i = PlayersSorted.Count - 1; i >= 0; i--){
				if(PlayersSorted[i].playerTeam == "sagarmatha"){
					GUILayout.BeginHorizontal("box");
					GUILayout.Label(PlayersSorted[i].playerName, myStyle, GUILayout.Width(200));
					GUILayout.Label(PlayersSorted[i].playerScore.ToString(), myStyle, GUILayout.Width(40));
					GUILayout.EndHorizontal();
				}
			}*/
			GUILayout.EndArea();
			
			//Start a new GUI area on the right portion of the screen. This area will be used for displaying blue team scores.
			GUILayout.BeginArea(new Rect(Screen.width / 2 + 10, 50, 250, Screen.height - 10));
			
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal("");
			//Display header with the team name and score.
			GUILayout.Label("Bagmati Team", bagmatiHeaderStyle, GUILayout.Width(200));
			GUILayout.Label(bagmatiTeamScore.ToString(), bagmatiHeaderStyle, GUILayout.Width(40));
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			
			/*
			//Go through the PlayersSorted in reverse and pick out each player that belongs to the blue team and display their name and score.
			for(int i = PlayersSorted.Count - 1; i >= 0; i--){
				if(PlayersSorted[i].playerTeam == "bagmati"){
					GUILayout.BeginHorizontal("box");
					GUILayout.Label(PlayersSorted[i].playerName, myStyle, GUILayout.Width(200));
					GUILayout.Label(PlayersSorted[i].playerScore.ToString(), myStyle, GUILayout.Width(40));
					GUILayout.EndHorizontal();
				}
			}*/
			GUILayout.EndArea();
			
			
			
		}
		
		//When a team wins display a box covering the screen and overlaying the winning message on top.
		//Only server has the authority to restart the match once the RestartMatch timer has gone to 0.
		if(bagmatiTeamHasWon == true){
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Bagmati Team has won the match", winStyle);
			
			if(Network.isServer){
				StartCoroutine(RestartMatch());
			}
		}
		
		if(sagarmathaTeamHasWon == true){
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Sagarmatha Team has won the match", winStyle);
			
			if(Network.isServer){
				StartCoroutine(RestartMatch());
			}
		}
	}
	
	[RPC]
	void UpdateSagarmathaTeamScore(){
		sagarmathaTeamScore++;
		shouldServerRefreshScore = true;
	}
	
	[RPC]
	void UpdateBagmatiTeamScore(){
		bagmatiTeamScore++;
		shouldServerRefreshScore = true;
	}
	
	[RPC]
	void ServerRefreshScore(int sagarmathaScore, int bagmatiScore){
		sagarmathaTeamScore = sagarmathaScore;
		bagmatiTeamScore = bagmatiScore;
	}
	
	//This is used on the server as a signal to restart the game.
	void RestartGame(){
		GameObject reload = GameObject.Find("ReloadLevel");
		ReloadLevelScript reloadScript = reload.GetComponent<ReloadLevelScript>();
		reloadScript.shouldWeReload = true;
	}
	
	IEnumerator RestartMatch(){
		yield return new WaitForSeconds(waitTime);
		
		RestartGame();
	}
	
}
