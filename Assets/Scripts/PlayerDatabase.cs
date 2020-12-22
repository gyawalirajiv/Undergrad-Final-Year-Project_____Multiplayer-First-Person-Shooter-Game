//It is attached to the GameManager.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerDatabase : MonoBehaviour {

	public List<PlayerDataClass> ListOfPlayers = new List<PlayerDataClass>();
	
	public NetworkPlayer networkPlayer;
	
	public bool isNameSet = false;
	public string playerName;
	
	public bool isScoreSet = false;
	public int playerScore;
	
	public bool isTeamJoined = false;
	public string playerTeam;
	
	public List<NetworkPlayer> networkPlayerList = new List<NetworkPlayer>();
	public bool isMatchRestared = false;
	public bool playersToBeAddedAgain = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isNameSet == true){
			networkView.RPC("EditPlayerLsitWithName", RPCMode.AllBuffered, Network.player, playerName);
			isNameSet = false;
		}
		
		if(isScoreSet == true){
			networkView.RPC("EditPlayerListWithScore", RPCMode.AllBuffered, Network.player, playerScore);
			isScoreSet = false;
		}
		
		if(isTeamJoined == true){
			networkView.RPC("EditPlayerListWithTeam", RPCMode.AllBuffered, Network.player, playerTeam);
			isTeamJoined = false;
		}
		
		if(Network.isServer == true && playersToBeAddedAgain == true){
			foreach(NetworkPlayer netPlayer in networkPlayerList){
				networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, netPlayer);
			}
			networkPlayerList.Clear();
			playersToBeAddedAgain = false;
		}
		
		if(Network.isClient == true && isMatchRestared == true){
			networkView.RPC("AddPlayerBack", RPCMode.Server, Network.player);
			isMatchRestared = false;
		}
		
	}
	
	void OnPlayerConnected(NetworkPlayer netPlayer){
		networkView.RPC("AddPlayerToList", RPCMode.AllBuffered, netPlayer);
	}
	
	void OnPlayerDisconnected(NetworkPlayer netPlayer){
		networkView.RPC("RemovePlayerFromList", RPCMode.AllBuffered, netPlayer);
	}
	
	
	[RPC]
	void AddPlayerToList(NetworkPlayer nPlayer){
		PlayerDataClass capture = new PlayerDataClass();
		capture.networkPlayer = int.Parse(nPlayer.ToString());
		ListOfPlayers.Add(capture);
	}
	
	[RPC]
	void RemovePlayerFromList(NetworkPlayer nPlayer){
		for(int i = 0; i < ListOfPlayers.Count; i++){
			if(ListOfPlayers[i].networkPlayer == int.Parse(nPlayer.ToString())){
				ListOfPlayers.RemoveAt(i);
			}
		}
	}
	
	[RPC]
	void EditPlayerLsitWithName(NetworkPlayer nPlayer, string pName){
		for(int i = 0; i < ListOfPlayers.Count; i++){
			if(ListOfPlayers[i].networkPlayer == int.Parse(nPlayer.ToString())){
				ListOfPlayers[i].playerName = pName;
			}
		}
	}
	
	[RPC]
	void EditPlayerListWithScore(NetworkPlayer nPlayer, int pScore){
		for(int i = 0; i < ListOfPlayers.Count; i++){
			if(ListOfPlayers[i].networkPlayer == int.Parse(nPlayer.ToString())){
				ListOfPlayers[i].playerScore = pScore;
			}
		}
	}
	
	[RPC]
	void EditPlayerListWithTeam(NetworkPlayer nPlayer, string pTeam){
		for(int i = 0; i < ListOfPlayers.Count; i++){
			if(ListOfPlayers[i].networkPlayer == int.Parse(nPlayer.ToString())){
				ListOfPlayers[i].playerTeam = pTeam;
			}
		}
	}
	
	[RPC]
	void AddPlayerBack(NetworkPlayer nPlayer){
		networkPlayerList.Add(nPlayer);
		playersToBeAddedAgain = true;
	}
	
}
