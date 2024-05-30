using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
	
	public bool alive = true;
	public float health = 100;
	
	[Header("Components")]
	
	public Transform model;
	public Transform equipment;
	
	[Header("Equipment")]
	
	public GameObject sword;
	public GameObject swordLarge;
	
	public GameObject shield;
	public GameObject shieldLarge;
	
	[Header("Interface")]
	
	public Image healthDisplay;
	public Text floorDisplay;
	
	// Power-ups
	
	private bool powerSword  = false;
	private bool powerShield = false;
	private bool powerSpeed  = false;
	
	// Private
	
	[HideInInspector] public Vector3 moveMomentum, velocityMomentum;
	private CharacterController controller;
	
	private float walkSpeed = 4f;
	private float walkSpeedDefault = 4f;
	
	private float gravity, gravityPrevious;
	
	private Vector3 initialPosition;
	private Quaternion initialRotation, equipmentRotation;
	
	private Plane plane = new Plane(Vector3.up, Vector3.zero);
	
	// Methods
	
	void Awake(){
		
		controller = GetComponent<CharacterController>();
		
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		
	}
	
	// Controls
	
	void Update(){
		
		Interface();
		
		if(!alive){ return; }
		
		Equipment();
		
		// Control
		
		Vector3 movementDirection = Vector3.zero;
		
		if(Input.GetKey(KeyCode.W)){ movementDirection.z += walkSpeed; movementDirection.x -= walkSpeed; }
		if(Input.GetKey(KeyCode.S)){ movementDirection.z -= walkSpeed; movementDirection.x += walkSpeed; }
		
		if(Input.GetKey(KeyCode.A)){ movementDirection.x -= walkSpeed; movementDirection.z -= walkSpeed; }
		if(Input.GetKey(KeyCode.D)){ movementDirection.x += walkSpeed; movementDirection.z += walkSpeed; }
		
		moveMomentum = Vector3.Lerp(moveMomentum, movementDirection, Time.deltaTime * 8f);
		
		// Rotation
		
		if(movementDirection != Vector3.zero){
		
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movementDirection), Time.deltaTime * 10f);
			
		}
		
		// Gravity
		
		float gravityPrevious = gravity;
		
		if(!controller.isGrounded){

			gravity += 0.3f;

		}else{

			if(gravity > -10.0f){ gravity = 1.0f; }

		}
		
		gravity = Mathf.Clamp(gravity, -100f, 20f);
		
		// Apply movement
		
		moveMomentum.y = -gravity;
		
		controller.Move(moveMomentum * Time.deltaTime);
		
		// Velocity and tilt
		
		Vector3 velocityController = controller.velocity * 5;
		Vector3 velocity = Vector3.zero;
		
		velocity.x = velocityController.z;
		velocity.z = velocityController.x;
		
		Vector3 velocityRotated = (transform.rotation * -velocity) / 2;
		
		model.localRotation = Quaternion.Lerp(model.localRotation, Quaternion.Euler(velocityRotated), Time.deltaTime * 16f);
		
		velocityMomentum = Vector3.Lerp(velocityMomentum, controller.velocity, Time.deltaTime * 10f);
		velocityMomentum.y = 0;
		
		// Camera
		
		View.instance.transform.position = Vector3.Lerp(View.instance.transform.position, transform.position + velocityMomentum, Time.deltaTime * 2f);
		
		// Reset
		
		if(transform.position.y < -20){ Reset(); }
		
	}
	
	void Equipment(){
		
		// Follow mouse
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float enter = 0f;

		if(plane.Raycast(ray, out enter)){
			
			Vector3 raycastPosition = ray.GetPoint(enter);
			
			Quaternion targetRotation = Quaternion.LookRotation(raycastPosition - equipment.position);
			equipment.rotation = Quaternion.Lerp(equipment.rotation, targetRotation, Time.deltaTime * 20f);
			
		}
		
		// Quickly switch
		
		if(Input.GetMouseButtonDown(0)){
			
			equipment.Rotate(0, 180, 0);
			
		}
		
		// Position
		
		equipment.position = Vector3.Lerp(equipment.position, transform.position, Time.deltaTime * 20f);
		
		// Slash speed
		
		int slashSpeed = Mathf.Abs(Mathf.RoundToInt((equipment.rotation.y - equipmentRotation.y) / Time.deltaTime));
		
		sword.GetComponent<Sword>().slashing = (slashSpeed > 1);
		swordLarge.GetComponent<Sword>().slashing = (slashSpeed > 1);
		
		equipmentRotation = equipment.rotation;
		
	}
	
	void Interface(){
		
		health = Mathf.Clamp(health, 0, 100);
		healthDisplay.fillAmount = Mathf.Lerp(healthDisplay.fillAmount, health / 100, Time.deltaTime * 10f);
		
		floorDisplay.text = string.Format("Floor {0}", Game.floor);
		
	}
	
	// Collisions
	
	private void OnTriggerEnter(Collider other){
		
		if(other.GetComponent<Item>()){
			
			other.GetComponent<Item>().Grab();
			
		}
		
		if(other.tag == "Ending"){
			
			alive = false;
			
			Audio.Play("levelEnd");
			
			Invoke("NextLevel", 2f);
			
		}
		
	}
	
	// Gameplay
	
	void NextLevel(){ Game.NextLevel(); }
	
	public void Hurt(Vector3 _origin){
		
		if(!alive){ return; }
		
		View.instance.Shake();
		health -= 10;
		
		Audio.Play("hurt");
		
		// Dead
		
		if(health <= 0){
			
			alive = false;
			
			Game.Particle(transform.position);
			
			Audio.Play("gameOver");
			
			Destroy(equipment.gameObject);
			Destroy(model.gameObject);
			
		}
		
	}
	
	public void GetPower(Type _type){
		
		switch(_type){
			
			case Type.Sword: if(!powerSword){
				
				sword.SetActive(false);
				swordLarge.SetActive(true);
				
				powerSword = true;
				Invoke("PowerOffSword", 10f);
				
			} break;
			
			case Type.Shield: if(!powerShield){
				
				shield.SetActive(false);
				shieldLarge.SetActive(true);
				
				powerShield = true;
				Invoke("PowerOffShield", 10f);
				
			} break;
			
			case Type.Speed:  if(!powerSpeed){
				
				walkSpeed = walkSpeedDefault * 2;
				
				powerSpeed = true;
				Invoke("PowerOffSpeed", 10f);
				
			} break;
			
		}
		
	}
	
	void PowerOffSword(){
		
		sword.SetActive(true);
		swordLarge.SetActive(false);
		
		powerSword = false;
		
	}
	
	void PowerOffShield(){
		
		shield.SetActive(true);
		shieldLarge.SetActive(false);
		
		powerShield = false;
		
	}
	
	void PowerOffSpeed(){
		
		walkSpeed = walkSpeedDefault;
		
		powerSpeed = false;
		
	}
	
	// Helper
	
	void Reset(){
		
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		
	}

}
