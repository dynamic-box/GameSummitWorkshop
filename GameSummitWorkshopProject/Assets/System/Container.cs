using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Container : MonoBehaviour{
	
	public GameObject[] items;
	
	public void Break(){
		
		GameObject _item = Instantiate(items[Random.Range(0, items.Length)]);
		_item.transform.position = transform.position;
		
		Game.Particle(transform.position);
		
		Audio.Play("containerBreak");
		
		Destroy(gameObject);
		
	}

}
