//Attached to Player Prefab

using UnityEngine;
using System.Collections;

/// <summary>
/// This script is possesed by each Player which makes their individual camera to follow the CameraHead gameObject that is attached within them
/// </summary>

public class CameraMovementScript : MonoBehaviour {
	
	private Camera myCamera;
	private Transform cameraHeadTransform;
	
	
	void Start () {
		if(networkView.isMine == true){
			myCamera = Camera.main;
			cameraHeadTransform = transform.FindChild("CameraHead");
		}
		else{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Follow the transform of the cameraHead
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
		
	}
}