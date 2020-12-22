//Attached to the Player

using UnityEngine;
using System.Collections;

/// <summary>
/// This script allows the players to fire bullet that has their parameters.
/// 
/// This script get data from the SpawnScript.
/// 
/// This script gets the data from BulletScript that was just instantiated.
/// </summary>

public class FireBullet : MonoBehaviour {
	
	//The prefab of Bullet is attached on this variable.
	public GameObject bullet;
	public AudioClip bulletSound;
	
	//Quick references.
	private Transform myPosition;
	private Transform cameraHeadPosition;
	
	//The bullet instantiating point from which the bullet will be produced.
	private Vector3 bulletLaunchPosition = new Vector3();
	
	//Used to control the rate of fire.
	private float newFire = 0.2f;
	private float waitFire = 0;
	
	//To determine which layer this is on.
	private bool iAmOnTheSagarmathaTeam = false;
	private bool iAmOnTheBagmatiTeam = false;
	
	
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine == true){
			myPosition = transform;
			cameraHeadPosition = myPosition.FindChild("CameraHead");
			
			//This line finds the SpawnManager gameObject and finds out the team of the player
			GameObject spawnManager = GameObject.Find("SpawnManager");
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			
			if(spawnScript.amIOnTheSagarmathaTeam == true){
				iAmOnTheSagarmathaTeam = true;
			}
			
			if(spawnScript.amIOnTheBagmatiTeam == true){
				iAmOnTheBagmatiTeam = true;
			}
		}
		else{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire Button") && Time.time > waitFire  && Screen.lockCursor == true){
			
			waitFire = Time.time + newFire;
			
			//Here the instantiation of bullet is done just side of the cameraHead but not in the same position.
			bulletLaunchPosition = cameraHeadPosition.TransformPoint(0, 0, 0.2f);
			
			//RPC the bullet accross the network and rotate it acording to its scale and prefab position.
			if(iAmOnTheSagarmathaTeam == true){
				networkView.RPC("BulletSpawn", RPCMode.All, bulletLaunchPosition, Quaternion.Euler(cameraHeadPosition.eulerAngles.x + 90, myPosition.eulerAngles.y, 0), myPosition.name, "sagarmatha");
			}
			
			if(iAmOnTheBagmatiTeam == true){
				networkView.RPC("BulletSpawn", RPCMode.All, bulletLaunchPosition, Quaternion.Euler(cameraHeadPosition.eulerAngles.x + 90, myPosition.eulerAngles.y, 0), myPosition.name, "bagmati");
			}
			
			
		}
	}
	
	[RPC]
	void BulletSpawn(Vector3 position, Quaternion rotation, string originatorName, string team){
		//Get the BulletScripto of the recent bullet and provide the RPC of the player name and its team.
		GameObject go = Instantiate(bullet, position, rotation) as GameObject;
		
		BulletScript bScript = go.GetComponent<BulletScript>();
		bScript.myOriginator = originatorName;
		bScript.team = team;
		
		audio.PlayOneShot(bulletSound);
	}
	
}