using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Arrow : MonoBehaviour{
	
	private bool flying = true;
	
	// Methods
	
	void Awake(){ Invoke("Remove", 5f); }
	
	void Update(){
		
		if(!flying){ return; }
		
		// Move arrow forward
		
		transform.Translate(Vector3.forward * 20f * Time.deltaTime);
		
	}
	
	void Remove(){ Destroy(gameObject); }
	
	// Collision detection
	
	private void OnTriggerEnter(Collider other){
		
		if(!flying){ return; }
		
		if(other.transform.name == "shield"){
			
			transform.SetParent(other.transform);
			
			flying = false;
			
			Audio.Play("arrowInShield");
			
		}
		
		if(other.transform.name == "sword" && other.GetComponent<Sword>().slashing){
			
			transform.Rotate(0, 180, 0);
			
			Audio.Play("arrowReturn");
			
		}
		
		if(other.transform.name == "player"){
			
			other.transform.GetComponent<Player>().Hurt(transform.position);
			
			Destroy(gameObject);
			
		}
		
		if(other.gameObject.tag == "Enemy"){
			
			Game.Particle(gameObject.transform.position);
			
			Audio.Play("hurt");
			
			Destroy(other.gameObject);
			Destroy(gameObject);
			
		}
		
	}

}