using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour{
	
	private bool activated = false;
	
	void Start(){
		
		gameObject.SetActive(activated);
		
	}
	
	public void Activate(){ activated = true; }

}
