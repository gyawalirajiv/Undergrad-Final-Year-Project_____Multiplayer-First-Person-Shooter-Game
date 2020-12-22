//Attached to SpawnManager.

using UnityEngine;
using System.Collections;


public class SpawnScript : MonoBehaviour {
	
	private bool recentlyConnectedToServer = false;
	
	public bool amIOnTheSagarmathaTeam = false;
	public bool amIOnTheBagmatiTeam = false;
	
	private Rect joinTeamRect;
	private string joinTeamWindowTitle = "Team Selection";
	private int joinTeamWindowWidth = 330;
	private int joinTeamWindowHeight = 100;
	private int joinTeamleftIndent;
	private int joinTeamTopIndent;
	private int buttonHeight = 40;
	
	public Transform sagarmathaTeamPlayer;
	public Transform bagmatiTeamPlayer;
	private int sagarmathaTeamGroup = 0;
	private int bagmatiTeamGroup = 1;
	
	private GameObject[] sagarmathaSpawnPoints;
	private GameObject[] bagmatiSpawnPoints;
	
	public bool iAmDestroyed = false;
	
	public bool isMatchRestarted = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Unity Internal function
	void OnConnectedToServer(){
		recentlyConnectedToServer = true;
	}
	
	void JoinTeamWindow(int windowID){
		
		
		if(recentlyConnectedToServer == true || isMatchRestarted == true){
			
			if(GUILayout.Button("Join Sagarmatha Team", GUILayout.Height(buttonHeight))){
				amIOnTheSagarmathaTeam = true;
				recentlyConnectedToServer = false;
				
				isMatchRestarted = false;
				
				SpawnSagarmathaTeamPlayer();
			}
			
			if(GUILayout.Button("Join Bagmati Team", GUILayout.Height(buttonHeight))){
				amIOnTheBagmatiTeam = true;
				recentlyConnectedToServer = false;
				
				isMatchRestarted = false;
				
				SpawnBagmatiTeamPlayer();
			}
		}
		
		if(iAmDestroyed == true){
			if(GUILayout.Button("Respawn", GUILayout.Height(buttonHeight * 2))){
				if(amIOnTheSagarmathaTeam == true){
					SpawnSagarmathaTeamPlayer();
				}
				if(amIOnTheBagmatiTeam == true){
					SpawnBagmatiTeamPlayer();
				}
				iAmDestroyed = false;
			}
		}
	}
	
	
	void OnGUI(){
		if(recentlyConnectedToServer == true || iAmDestroyed == true || isMatchRestarted == true && Network.isClient){
			Screen.lockCursor = false;
			
			joinTeamleftIndent = Screen.width / 2 - joinTeamWindowWidth/2;
			joinTeamTopIndent = Screen.height / 2 - joinTeamWindowHeight/2;
			
			joinTeamRect = new Rect(joinTeamleftIndent, joinTeamTopIndent, joinTeamWindowWidth, joinTeamWindowHeight);
			
			joinTeamRect = GUILayout.Window(0, joinTeamRect,  JoinTeamWindow, joinTeamWindowTitle);
		}
	}
	
	
	
	void SpawnSagarmathaTeamPlayer(){
		sagarmathaSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnSagarmathaTeam");
		
		GameObject randomSagarmathaSpawn = sagarmathaSpawnPoints[Random.Range(0, sagarmathaSpawnPoints.Length)];
		
		Network.Instantiate(sagarmathaTeamPlayer, randomSagarmathaSpawn.transform.position, randomSagarmathaSpawn.transform.rotation, sagarmathaTeamGroup);
		
		GameObject gameManager = GameObject.Find("GameManager");
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		dataScript.isTeamJoined = true;
		dataScript.playerTeam = "sagarmatha";
	}
	
	void SpawnBagmatiTeamPlayer(){
		bagmatiSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnBagmatiTeam");
		
		GameObject randomBagmatiSpawn = bagmatiSpawnPoints[Random.Range(0, bagmatiSpawnPoints.Length)];
		
		Network.Instantiate(bagmatiTeamPlayer, randomBagmatiSpawn.transform.position, randomBagmatiSpawn.transform.rotation, bagmatiTeamGroup);
		
		GameObject gameManager = GameObject.Find("GameManager");
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		dataScript.isTeamJoined = true;
		dataScript.playerTeam = "bagmati";
	}
	
}
