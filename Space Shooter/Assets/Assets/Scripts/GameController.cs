using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	//The games score text
	public Text scoreText;
	//Our restart text element
	public Text restartText;
	//Our game over text element
	public Text gameOverText;
	//The games current score
	private int score;
	//A boolean that indicates if the game is over
	private bool gameOver;
	//A boolean that indicates if it is okay to restart the game
	private bool restart;

	void Start() {
		//Instantiate all of the private variables
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		//Start spawning asteroids
		StartCoroutine(SpawnWaves());
	}

	void Update() {
		//Restart the game if the flag is true and the player presses the 'R' Key.
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				//Close all current loaded scenes and load the main scene.
				SceneManager.LoadScene ("Main", LoadSceneMode.Single);
			}
		}
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

			//Check to see if the game is over and display the restart text
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				//Break us out of the while loop
				break;
			}
		}
	}

	void UpdateScore() {
		scoreText.text = "Score: " + score;
	}

	public void AddScore(int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	public void GameOver() {
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
		
}
