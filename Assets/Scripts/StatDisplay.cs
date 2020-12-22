//Attached to the player.
using UnityEngine;
using System.Collections;


public class StatDisplay : MonoBehaviour {
	
	public Texture healthTexture;
	
	private float health;
	private int healthToBeDisplayed;
	
	private int boxWidth = 160;
	private int boxHeight = 85;
	private int labelHeight = 20;
	private int labelWidth = 35;
	private int padding = 10;
	private int gap = 120;
	private float healthBarLength;
	private int healthBarHeight = 15;
	private GUIStyle healthStyle = new GUIStyle();
	private float commomLeft;
	private float commonTop;
	
	//A quick reference to the Health script.
	private Health HScript;
	
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine == true){
			//Access the Health script.
			Transform triggerTransform = transform.FindChild("Trigger");
			HScript = triggerTransform.GetComponent<Health>();
			
			//Set the GUIStyle.
			healthStyle.normal.textColor = Color.green;
			healthStyle.fontStyle = FontStyle.Bold;
		}
		else{
			enabled = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		health = HScript.health;
		
		healthToBeDisplayed = Mathf.CeilToInt(health);
		
		healthBarLength = (health / HScript.maxHealth * 100);
	}
	
	void OnGUI(){
		commomLeft = Screen.width / 2 + 180;
		commonTop = Screen.height / 2 + 50;
		
		GUI.Box(new Rect(commomLeft, commonTop, boxWidth, boxHeight), "");
		
		GUI.Box(new Rect(commomLeft + padding, commonTop + padding, 100, healthBarHeight), "");
		
		GUI.DrawTexture(new Rect(commomLeft + padding, commonTop + padding, healthBarLength, healthBarHeight), healthTexture);
		
		GUI.Label(new Rect(commomLeft + gap, commonTop + padding, labelWidth, labelHeight), healthToBeDisplayed.ToString(), healthStyle);
	}
}
