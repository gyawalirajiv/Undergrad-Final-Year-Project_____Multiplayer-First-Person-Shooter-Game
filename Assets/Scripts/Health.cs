//It is attached to the Trigger child of Players.
using UnityEngine;
using System.Collections;

/// <summary>
/// This script is responsible for manipulating the health of player across teh network and to provide the damage functionality if bulet hits the player.. 
/// 
/// List of players are gainde by this script from the PlayerDatabase script.
/// 
/// The spaenScript is accessed by this script to let the player know that they have been destroyed.
/// 
/// The player's score is incremented via this scripr by accessesing the PlayerScore script.
/// 
/// This script gets the data from the BulletScript.
/// 
/// The Stat Display script get the data from this script.
/// 
/// The PlayerLabel script gets its data from this script..
/// </summary>

public class Health : MonoBehaviour {
	
	private GameObject parent;
	
	//Thsi determines who player has been hit by bullet and which computer needs to process the damage effect..
	public string whoAttackedMe;
	public bool iJustGotAttacked;
	
	//These variables are used to define if the player jot hit by a bullet or not, and much damage to apply if hit.
	public bool iGotHitByBullet = false;
	private float bulletDamage = 30;
	
	//When the player is under going destruction this variable prevents the other players from hit further.
	private bool gotDestroyed = false;
	
	//These variable are the health point parameters.
	public float health = 100;
	public float maxHealth = 100;
	private float healthRegeneration = 1.3f;
	
	
	
	// Use this for initialization
	void Start () {
		//As this script is attached to the trigger gameObject of the player, it needs to destroy the parent gameObject to destroyt the player if it falss to 0.
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//When the bullet hits the trigger the variable iJustGotAttacked get to true.
		if(iJustGotAttacked == true){
			GameObject gameManager = GameObject.Find("GameManager");
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			
			//Shift through the player list and only carry out hit detection if the attacking
			//player is the one running this game instance.
			for(int i = 0; i < dataScript.ListOfPlayers.Count; i++){
				if(whoAttackedMe == dataScript.ListOfPlayers[i].playerName){
					if(int.Parse(Network.player.ToString()) == dataScript.ListOfPlayers[i].networkPlayer){
						//Here the script checks what hit the [pplayer and how much damage to apply.
						if(iGotHitByBullet == true && gotDestroyed == false){
							health = health - bulletDamage;
							
							//The RPC of the attacking player is send out inoreder in inform all the player to increse the score of the player that destroyed this player.
							networkView.RPC("LetEveryoneKnowMyAttacker", RPCMode.Others, whoAttackedMe);
							
							//This RPC is send to let know all players the new health of this player
							networkView.RPC("LetEveryoneKnowMyHealth", RPCMode.Others, health);
							
							iGotHitByBullet = false;
						}
						
						//When the player health get zero the gotDestroyed get true and the attacking player is awarded. Then the other players cannot attack this player as he is going destruction, and increases their point. 
						if(health <= 0 && gotDestroyed == false){
							health = 0;
							gotDestroyed = true;
							
							GameObject attacker = GameObject.Find(whoAttackedMe);
							
							PlayerScore scoreScript = attacker.GetComponent<PlayerScore>();
							scoreScript.justDestroyedEnemy = true;
							scoreScript.oneHitDestroyedEnemy++;
						}
						
					}
				}
			}
			
			iJustGotAttacked = false;
		}
		
		//The destruction of player is responmsible by itself
		if(health <= 0 && networkView.isMine == true){
			//Set the iAmDestroyed to true, and the player can respawn
			GameObject spawnManager = GameObject.Find("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			spawnScript.iAmDestroyed = true;
			
			
			//This is to destroy the player, else a avatar of player will remain to stay on the gameplay.
			Network.RemoveRPCs(Network.player);
			
			//RPC to destroy the player.
			networkView.RPC("SelfDestruct", RPCMode.All);
		}
		
		
		//Regeration of the health.
		if(health < maxHealth){
			health = health + healthRegeneration * Time.deltaTime;
		}
		
		//Check if the health exceed the max health if so reduce it ot 100.
		if(health > maxHealth){
			health = maxHealth;
		}
	
	}
	
	[RPC]
	void LetEveryoneKnowMyAttacker(string attacker){
		whoAttackedMe = attacker;
	}
	
	[RPC]
	void LetEveryoneKnowMyHealth(float myHealth){
		health = myHealth;
	}
	
	[RPC]
	void SelfDestruct(){
		Destroy(parent);
	}
}
