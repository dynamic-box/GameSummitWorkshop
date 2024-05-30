using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Dungeon : MonoBehaviour{
	
	// Properties
	
	public int size = 5;
	
	// Room types
	
	private List<Transform> roomsNorth = new List<Transform>();
	private List<Transform> roomsEast  = new List<Transform>();
	private List<Transform> roomsSouth = new List<Transform>();
	private List<Transform> roomsWest  = new List<Transform>();
	
	private Dictionary<Vector3, bool> grid = new Dictionary<Vector3, bool>();
	private Dictionary<string, Transform> roomsSpecial = new Dictionary<string, Transform>();
	
	// Methods
	
	private void Awake(){
		
		LoadRooms();
		
	}
	
	private void Start(){
		
		GenerateDungeon();
		
	}
	
	private void LoadRooms(){
		
		// Load special rooms
		
		Object[] specialRooms = Resources.LoadAll("Special rooms", typeof(GameObject));
		
		foreach(GameObject room in specialRooms){
			
			roomsSpecial.Add(room.name, room.transform);
			
		}
		
		// Load all room types
		
		Object[] rooms = Resources.LoadAll("Rooms", typeof(GameObject));
		
		foreach(GameObject room in rooms){
			
			RoomSpawn[] roomSpawns = room.GetComponentsInChildren<RoomSpawn>();
			RoomTemplate roomTemplate = room.GetComponent<RoomTemplate>();
			
			foreach(RoomSpawn roomSpawn in roomSpawns){
				
				for(int i = 0; i < roomTemplate.chance; i++){
				
					switch(roomSpawn.requiredDirection){
						
						case Direction.North: roomsSouth.Add(room.transform); break;
						case Direction.East:  roomsWest.Add(room.transform); break;
						case Direction.South: roomsNorth.Add(room.transform); break;
						case Direction.West:  roomsEast.Add(room.transform); break;
						
					}
				
				}
				
			}
			
		}
		
	}
	
	private void GenerateDungeon(){
		
		// Clear previous
		
		foreach(Transform t in transform){ Destroy(t.gameObject); }
		
		grid.Clear();
		
		// Start generation
		
		CreateRoom(Direction.None, Vector3.zero, 0);
		
		// Set ending
		
		var endings = FindObjectsOfType<Ending>();
		endings = endings.OrderBy((d) => (d.transform.position - Vector3.zero).sqrMagnitude).Reverse().ToArray();
		
		endings[0].GetComponent<Ending>().Activate();
		
	}
	
	private GameObject CreateRoom(Direction _direction, Vector3 _position, int _distance){
		
		if(grid.ContainsKey(_position)){ return null; }
		
		bool _forceEnding = (_distance > size);
		
		// Get random but fitting template
		
		GameObject _room = Instantiate(GetRoomTemplate(_direction, _forceEnding), transform);
		_room.transform.position = _position;
		
		grid.Add(_position, true);
		
		// Generate additional rooms
		
		RoomSpawn[] roomSpawns = _room.GetComponentsInChildren<RoomSpawn>();
	
		foreach(RoomSpawn roomSpawn in roomSpawns){
			
			switch(roomSpawn.requiredDirection){
				
				case Direction.North: CreateRoom(Direction.North, roomSpawn.transform.position, _distance + 1); break;
				case Direction.East:  CreateRoom(Direction.East,  roomSpawn.transform.position, _distance + 1); break;
				case Direction.South: CreateRoom(Direction.South, roomSpawn.transform.position, _distance + 1); break;
				case Direction.West:  CreateRoom(Direction.West,  roomSpawn.transform.position, _distance + 1); break;
				
			}
			
		}
		
		return _room;
		
	}
	
	private GameObject GetRoomTemplate(Direction _direction, bool _forceEnding = false){
		
		if(_forceEnding){
			
			switch(_direction){
			
				case Direction.North: return roomsNorth[0].gameObject;
				case Direction.East:  return roomsEast[0].gameObject;
				case Direction.South: return roomsSouth[0].gameObject;
				case Direction.West:  return roomsWest[0].gameObject;
				
			}
			
		}
		
		switch(_direction){
			
			case Direction.None:  return roomsSpecial["roomStart"].gameObject;
			
			case Direction.North: return roomsNorth[Random.Range(0, roomsNorth.Count)].gameObject;
			case Direction.East:  return roomsEast[Random.Range(0, roomsEast.Count)].gameObject;
			case Direction.South: return roomsSouth[Random.Range(0, roomsSouth.Count)].gameObject;
			case Direction.West:  return roomsWest[Random.Range(0, roomsWest.Count)].gameObject;
			
		}
		
		return null;
		
	}

}
