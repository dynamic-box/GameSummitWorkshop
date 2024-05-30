using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Archer : MonoBehaviour{
	
	[Header("Components")]
	
	public Transform arrow;
	public Transform crossbow;
	
	// Private
	
	private Player player;
	private bool inRange = false;
	
	// Methods
	
	void Awake(){
		
		player = FindObjectOfType<Player>();
		
		InvokeRepeating("Shoot", 1f, Random.Range(0.9f, 1.1f));
		
	}
	
	void Update(){
		
		inRange = (Vector3.Distance(transform.position, player.transform.position) < 16f);
		
		if(!inRange){ return; }
		
		Quaternion rotationTarget = Quaternion.LookRotation((player.transform.position + (player.velocityMomentum / 6)) - transform.position);
		
		transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.deltaTime * 30f);
		
	}
	
	void Shoot(){
		
		if(!inRange || !player.alive){ return; }
		
		GameObject _arrow = Instantiate(arrow.gameObject, crossbow.transform);
		
		_arrow.transform.localPosition = new Vector3(0, Random.Range(0, 0.25f), 0.75f);
		_arrow.transform.SetParent(null);
		
		Audio.Play("arrowShoot");
		
	}

}
