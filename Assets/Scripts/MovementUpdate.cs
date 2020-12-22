//Attached to player prefab.
using UnityEngine;
using System.Collections;

public class MovementUpdate : MonoBehaviour {
	
	private Vector3 lastPosition;
	private Quaternion lastRotation;
	private Transform myTransform;
	
	
	
	// Use this for initialization
	void Start () {
		if(networkView.isMine == true){
			myTransform = transform;
			
			networkView.RPC("updateMyTransform", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
		else{
			enabled = false;
		}
	}
	
	void Update () {
		if(Vector3.Distance(myTransform.position, lastPosition) >= 0.1){
			lastPosition = myTransform.position;
			
			networkView.RPC("updateMyTransform", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
			
		}
		if(Quaternion.Angle(myTransform.rotation, lastRotation) >= 1){
			lastRotation = myTransform.rotation;
			
			networkView.RPC("updateMyTransform", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation);
		}
		
	}
	
	[RPC]
	void updateMyTransform(Vector3 newPosition, Quaternion newRotation){
		transform.position = newPosition;
		transform.rotation = newRotation;
	}
	
	
	
}
