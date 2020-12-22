//Attached to the player prefab
using UnityEngine;
using System.Collections;

/// <summary>
/// This script manages the cursor lock or unlock system on the game.
/// 
/// This script gets data from the the Communication script.
/// 
/// This script gets data from the  ScoreTable script.
/// </summary>

public class CursorControl : MonoBehaviour {
	
	private GameObject multiplayerManager;
	private CommunicationScript commScript;
	
	private GameObject gameManager;
	private ScoreTable scoreScript;
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine == true){
			multiplayerManager = GameObject.Find("MultiplayerManager");
			
			commScript = multiplayerManager.GetComponent<CommunicationScript>();
			
			gameManager = GameObject.Find("GameManager");
			scoreScript = gameManager.GetComponent<ScoreTable>();
		}
		else{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(commScript.showDisconnectWindow == false && scoreScript.bagmatiTeamHasWon == false && scoreScript.sagarmathaTeamHasWon == false){
			Screen.lockCursor = true;
		}
		if(commScript.showDisconnectWindow == true || scoreScript.bagmatiTeamHasWon == true || scoreScript.sagarmathaTeamHasWon == true){
			Screen.lockCursor = false;
		}
	}
}
