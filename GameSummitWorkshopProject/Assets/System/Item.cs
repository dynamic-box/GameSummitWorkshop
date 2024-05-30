using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Item : MonoBehaviour{
	
	public Type type;
	
	public void Grab(){
		
		Player player = FindObjectOfType<Player>();
		
		string particleColor = "#FF2041";
		
		switch(type){
			
			case Type.Potion: player.health += 25; break;
			
			case Type.Sword:  player.GetPower(type); particleColor = "#2ED573"; break;
			case Type.Shield: player.GetPower(type); particleColor = "#1E90FF"; break;
			case Type.Speed:  player.GetPower(type); particleColor = "#FFA502"; break;
			
		}
		
		Game.ParticleItem(transform.position, particleColor);
		
		Audio.Play("item");
		
		Destroy(gameObject);
		
	}

}

public enum Type { Potion, Sword, Shield, Speed }
