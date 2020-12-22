//Attached to Bullet Prefab

using UnityEngine;
using System.Collections;

/// <summary>
/// The bullets instantiated needs to be governed, so this script helps the bullet transform and detect if it hit any gameObject.0
/// 
/// This script attributes are called by FireBullet.cs script.
/// </summary>

public class BulletScript : MonoBehaviour {
	
	//The explosion that is produced when bullet hits something.
	public GameObject bulletEffect;
	
	private Transform myPosition;
	
	//The spped of transform og the bullet.
	private float bulletSpeed = 10;
	
	// This variable determines if the bullet has hit some gameObject if so then it spots if from hitting any more objects.
	private bool hitSomething = false;
	
	//This varible stores the instance of gameObject which was hit by bullet.
	private RaycastHit hit;
	
	//The range of the RayCast.
	private float rayDistance = 1.5f;
	
	//The life duration of the bullet.
	private float deathTime = 5;
	
	//The player which was hit and which team it was of.
	public string team;
	public string myOriginator;
	
	void Start () {
		myPosition = transform;
		
		//Start the countdown to destroy the bullet.
		StartCoroutine(SelfDestructTime());
	}
	
	// Update is called once per frame
	void Update () {
		//Change the position of bullet in each frame on upwards direction.
		myPosition.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
		
		//This code is execeted when the bullet hits something.
		if(Physics.Raycast(myPosition.position, myPosition.up, out hit, rayDistance) && hitSomething == false){
			//This is executed when it hits the Terrain
			if(hit.transform.tag == "Floor"){
				hitSomething = true;
				
				//Instantiate the Explosion effect when it hits some gameObject
				Instantiate(bulletEffect, hit.point, Quaternion.identity);
				
				//Disable the rendering of the bullet
				myPosition.renderer.enabled = false;
				
				//disable the light component of the bullet
				myPosition.light.enabled = false;
				
			}
			
			
			if(hit.transform.tag == "BagmatiTeamTrigger" || hit.transform.tag == "SagarmathaTeamTrigger"){
				hitSomething = true;
				
				//Instantiate the Explosion effect when it hits some gameObject
				Instantiate(bulletEffect, hit.point, Quaternion.identity);
				
				//Disable the rendering of the bullet
				myPosition.renderer.enabled = false;
				
				//disable the light component of the bullet
				myPosition.light.enabled = false;
				
				//This line acceses the Health script and informs who attacked the 
				//the player.
				if(hit.transform.tag == "BagmatiTeamTrigger" && team == "sagarmatha"){
					Health Hscript = hit.transform.GetComponent<Health>();
					Hscript.iJustGotAttacked = true;
					Hscript.whoAttackedMe = myOriginator;
					Hscript.iGotHitByBullet = true;
				}
				
				if(hit.transform.tag == "SagarmathaTeamTrigger" && team == "bagmati"){
					Health Hscript = hit.transform.GetComponent<Health>();
					Hscript.iJustGotAttacked = true;
					Hscript.whoAttackedMe = myOriginator;
					Hscript.iGotHitByBullet = true;
				}
			}
		}
	}
	
	IEnumerator SelfDestructTime(){
		//Start the counter to destroy this gameObject
		
		yield return new WaitForSeconds(deathTime);
		
		Destroy(myPosition.gameObject);
	}
}
