using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour{
	
	public bool slashing = false;
	
	private Quaternion lastRotation;
	
	void Update(){
		
		lastRotation = transform.rotation;
		
	}
	
	private void OnTriggerEnter(Collider other){
		
		if(!slashing){ return; }
		
		if(other.gameObject.tag == "Enemy"){
			
			Game.Particle(other.transform.position);
			
			Audio.Play("hurt");
			
			Destroy(other.gameObject);
			
		}
		
		if(other.GetComponent<Container>()){
			
			other.GetComponent<Container>().Break();
			
		}
		
	}

}
