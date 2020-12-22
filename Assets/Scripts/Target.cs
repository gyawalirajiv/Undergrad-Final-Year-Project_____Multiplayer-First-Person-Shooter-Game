//Attached to player

using UnityEngine;
using System.Collections;



public class Target : MonoBehaviour {
	
	public Texture targetTexture;
	private float targetDimension = 256;
	private float halfTargetWidth = 128;
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine == false){
			enabled = false;
		}
	}
	
	
	void OnGUI(){
		if(Screen.lockCursor == true){
			GUI.DrawTexture(new Rect(Screen.width / 2 - halfTargetWidth, Screen.height / 2 - halfTargetWidth, targetDimension, targetDimension), targetTexture);
		}
	}
}
