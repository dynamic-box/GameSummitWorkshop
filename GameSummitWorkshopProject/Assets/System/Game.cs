using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour{
	
	public static int floor = 1;
	
	// Effects
	
	public static GameObject particleExplosion;
	public static GameObject particleItem;
	
	// Methods
	
	void Awake(){
		
		particleExplosion = Resources.Load<GameObject>("Effects/particleExplosion");
		particleItem = Resources.Load<GameObject>("Effects/particleItem");
		
	}
	
	// Gameplay
	
	public static void NextLevel(){
		
		Game.floor++;
		
		// Reload scene (generate new dungeon)
		
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	
	}
	
	// Effects
	
	public static void Particle(Vector3 _position){
		
		GameObject _particle = Instantiate(particleExplosion, null);
		_particle.transform.position = _position;
		
	}
	
	public static void ParticleItem(Vector3 _position, string _color = "#FFFFFF"){
		
		GameObject _particle = Instantiate(particleItem, null);
		_particle.transform.position = _position;
		
		// Color
		
		ParticleSystem _particleSystem  = _particle.GetComponent<ParticleSystem>();
		ParticleSystem.MainModule _main = _particleSystem.main;
		
		Color _hexColor;
		ColorUtility.TryParseHtmlString(_color, out _hexColor);
		
		_main.startColor = _hexColor;
		
	}

}
