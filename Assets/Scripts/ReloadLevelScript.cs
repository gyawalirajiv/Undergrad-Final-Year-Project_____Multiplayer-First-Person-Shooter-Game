//Attached to ReloadLevel GameObject.

using UnityEngine;
using System.Collections;


public class ReloadLevelScript : MonoBehaviour {
	
	public bool shouldWeReload = false;
	public bool isMatchRestarting = false;
	public float waitingTime = 0.1f;
	private static bool wasItCreated = false;
	
	
	void Awake(){
		if(wasItCreated == false){
			DontDestroyOnLoad(gameObject);
			
			wasItCreated = true;
		}
		else{
			Destroy(gameObject);
		}
	}
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if(shouldWeReload == true && Network.isServer){
			networkView.RPC("RestartMatch", RPCMode.All);
			shouldWeReload = false;
		}
		if(isMatchRestarting == true){
			
			GameObject gameManager = GameObject.Find("GameManager");
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			dataScript.isMatchRestared = true;
			
			
			GameObject spawnManager = GameObject.Find("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			spawnScript.isMatchRestarted = true;
			isMatchRestarting = false;
		}
	}
	
	[RPC]
	void RestartMatch(){
		Network.RemoveRPCs(Network.player);
		Network.SetSendingEnabled(0, false);
		Network.SetSendingEnabled(1, false);
		Network.isMessageQueueRunning = false;
		Application.LoadLevel("PlayMe");
		
		StartCoroutine(Delay());
	}
	
	IEnumerator Delay(){
		yield return new WaitForSeconds(waitingTime);
		Network.isMessageQueueRunning = true;
		Network.SetSendingEnabled(0, true);
		Network.SetSendingEnabled(1, true);
		isMatchRestarting = true;
	}
	
}
