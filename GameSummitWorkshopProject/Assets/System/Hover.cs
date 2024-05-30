using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Hover

public class Hover : MonoBehaviour {

	public float amplitude = 0.5f;
	public float frequency = 1.0f;

	public Vector3 rotation = Vector3.zero;

	private Vector3 positionOffset    = new Vector3();
	private Vector3 positionTemporary = new Vector3();
    
	void Start(){

		positionOffset = transform.localPosition;

	}

	void Update(){

		transform.Rotate(rotation * Time.deltaTime);

		positionTemporary = positionOffset;
		positionTemporary.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.localPosition = positionTemporary;

	}
	
}