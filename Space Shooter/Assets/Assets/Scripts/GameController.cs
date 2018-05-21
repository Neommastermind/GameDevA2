using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	//The Asteroid obstacles
	public GameObject hazard;
	//The spawn location for our asteroids
	public Vector3 spawnValues;
	//The amount of asteroids to spawn
	public int hazardCount;
	//The wait time between hazard spawns
	public float spawnWait;
	//The time the program waits to start spawning waves
	public float startWait;
	//The time between each wave
	public float waveWait;

	void Start() {
		StartCoroutine(SpawnWaves());
	}

	//Spawns all of the asteroids in a wave
	IEnumerator SpawnWaves() {
		//Wait for a certain time before starting to spawn
		yield return new WaitForSeconds (startWait);
		//Start spawning
		while(true) {
			for(int i = 0; i < hazardCount; i++) {
				//Determine the spawn position from the spawnValues and a random x value
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				//A Quaternion with no rotation
				Quaternion spawnRotation = Quaternion.identity;
				//Spawn the hazard
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}
}
