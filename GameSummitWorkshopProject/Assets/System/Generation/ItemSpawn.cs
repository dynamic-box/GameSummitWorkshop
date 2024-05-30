using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ItemSpawn : MonoBehaviour{
	
	[Header("Position")]
	
	public Vector3 randomPosition = Vector3.zero;
	public Vector3 randomRotation = Vector3.zero;
	public bool snapTo90Degrees = false;
	
	[Header("Items")]
	
	public GameObject[] items;
	
	// Methods
	
	void Awake(){
		
		foreach(Transform t in transform){ Destroy(t.gameObject); }
		
		transform.position += new Vector3(
		
			Random.Range(-randomPosition.x, randomPosition.x),
			Random.Range(-randomPosition.y, randomPosition.y),
			Random.Range(-randomPosition.z, randomPosition.z)
			
		);
			
		transform.eulerAngles += new Vector3(
		
			Random.Range(-randomRotation.x, randomRotation.x),
			Random.Range(-randomRotation.y, randomRotation.y),
			Random.Range(-randomRotation.z, randomRotation.z)
			
		);
			
		if(snapTo90Degrees){
			
			transform.eulerAngles = new Vector3(
			
				Mathf.Round(transform.eulerAngles.x / 90) * 90,
				Mathf.Round(transform.eulerAngles.y / 90) * 90,
				Mathf.Round(transform.eulerAngles.z / 90) * 90
			
			);
			
		}
		
		GameObject _item = Instantiate(items[Random.Range(0, items.Length)], transform);
		
	}

}