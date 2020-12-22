//Attached to the player.
using UnityEngine;
using System.Collections;

public class PlayerLabel : MonoBehaviour {
	
	public Texture healthTexture;
	
	private Camera myCamera;
	private Transform myPosition;
	private Transform triggerTransfomr;
	private Health HScript;
	
	private Vector3 worldRelation = new Vector3();
	private Vector3 screenRelation = new Vector3();
	private Vector3 cameraRelation = new Vector3();
	private float minZ = 1.5f;
	
	private int labelTop = 18;
	private int labelWidth = 110;
	private int labelHeight = 15;
	private int barTop = 1;
	private int healthBarHeight = 5;
	private int healthBarLeft = 110;
	private float healthBarLength;
	private float adjustment = 1;
	
	public string playerName;
	private GUIStyle myStyle = new GUIStyle();
	
	
	void Awake(){
		if(networkView.isMine == false){
			myPosition = transform;
			myCamera = Camera.main;
			
			Transform triggerTransform = transform.FindChild("Trigger");
			HScript = triggerTransform.GetComponent<Health>();
			
			if(myPosition.tag == "BagmatiTeam"){
				myStyle.normal.textColor = Color.blue;
			}
			if(myPosition.tag == "SagarmathaTeam"){
				myStyle.normal.textColor = Color.red;
			}
			
			myStyle.fontSize = 12;
			myStyle.fontStyle = FontStyle.Bold;
			
			myStyle.clipping = TextClipping.Overflow;
		}
		else{
			enabled = false;
		}
	}
	
	
	
	// Update is called once per frame
	void Update () {
		cameraRelation = myCamera.transform.InverseTransformPoint(myPosition.position);
		
		if(HScript.health < 1){
			healthBarLength = 1;
		}
		if(HScript.health >= 1){
			healthBarLength = (HScript.health / HScript.maxHealth) * 100;
		}
	
	}
	
	void OnGUI(){
		if(cameraRelation.z > minZ){
			worldRelation = new Vector3(myPosition.position.x, myPosition.position.y + adjustment, myPosition.position.z);
			
			screenRelation = myCamera.WorldToScreenPoint(worldRelation);
			
			GUI.Box(new Rect(screenRelation.x - healthBarLeft / 2, Screen.height - screenRelation.y - barTop, 100, healthBarHeight), "");
			
			GUI.DrawTexture(new Rect(screenRelation.x - healthBarLeft / 2, Screen.height - screenRelation.y - barTop, healthBarLength, healthBarHeight), healthTexture);
			
			GUI.Label(new Rect(screenRelation.x - labelWidth / 2, Screen.height - screenRelation.y - labelTop, labelWidth, labelHeight), playerName, myStyle);
		}
	}
}
