using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class View : MonoBehaviour{
	
	public static View instance { get; private set; }
	
	public Transform cameraRig;
	
	// Methods
	
	void Awake(){
		
		if(instance == null){ instance = this; }
		
	}
	
	void Update(){
		
		cameraRig.localPosition = Vector3.Lerp(cameraRig.localPosition, new Vector3(0, 0, -30), Time.deltaTime * 8f);
		
	}
	
	public void Shake(float _amount = 0.5f){
		
		cameraRig.localPosition += Random.insideUnitSphere * _amount;
		
	}

}
