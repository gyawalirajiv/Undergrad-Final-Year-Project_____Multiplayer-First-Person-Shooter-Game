//Attached to players

using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {
	
	public string nameOfEnemyDestroyed;
	public bool justDestroyedEnemy = false;
	public int oneHitDestroyedEnemy;
	public int score;
	
	
	void Start () {
		if(networkView.isMine == true){
			GameObject gameManager = GameObject.Find("GameManager");
			
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			
			for(int i = 0; i < dataScript.ListOfPlayers.Count; i++){
				if(dataScript.ListOfPlayers[i].networkPlayer == int.Parse(Network.player.ToString())){
					score = dataScript.ListOfPlayers[i].playerScore;
					
					UpdateScoreInTheServerDatabase(score);
				}
			}
		}
		else{
			enabled = false;
		}
	}
	
	void Update () {
		if(justDestroyedEnemy == true){
			for(int i = 0; i < oneHitDestroyedEnemy; i++){
				score++;
				
				UpdateScoreInTheServerDatabase(score);
				
				UpdateTheScoreInScoreObject();
			}
			oneHitDestroyedEnemy = 0;
			
			justDestroyedEnemy = false;
		}
	}
	
	void UpdateScoreInTheServerDatabase(int score){
		GameObject gameManager = GameObject.Find("GameManager");
			
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
		
		dataScript.isScoreSet = true;
		dataScript.playerScore = score;
	}
	
	void UpdateTheScoreInScoreObject(){
		GameObject spawnManager = GameObject.Find("SpawnManager");
		SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
		GameObject gameManager = GameObject.Find("GameManager");
		ScoreTable tableScript = gameManager.GetComponent<ScoreTable>();
		
		if(spawnScript.amIOnTheBagmatiTeam == true){
			tableScript.updateBagmatiScore = true;
			tableScript.numberOfEnemiesDestroyedInOneHit = oneHitDestroyedEnemy;
		}
		
		if(spawnScript.amIOnTheSagarmathaTeam == true){
			tableScript.updateSagarmathaScore = true;
			tableScript.numberOfEnemiesDestroyedInOneHit = oneHitDestroyedEnemy;
		}
	}
	
}
