﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	//The Asteroid obstacles
	public GameObject[] hazards;
	//The games power ups
	public GameObject[] powerUps;
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
	//The time the program waits to spawn a power up
	public float powerUpWait;
	//The area power ups are allowed to spawn in
	public Boundary powerUpBounds;
	//The games score text
	public Text scoreText;
	//Our restart text element
	public Text restartText;
	//Our game over text element
	public Text gameOverText;
	//Our pick up text element
	public Text pickUpText;
	//The games current score
	private static int score = 0;
	//A boolean that indicates if the game is over
	private bool gameOver;
	//A boolean that indicates if it is okay to restart the game
	private bool restart;
	//A boolean that indicates if we are a winner
	private bool winner;
	//A reference to the player script
	public PlayerController player;
	//The enemy kill count
	private int enemyKC;
	//The number of enemies you need to kill to move onto the boss fight
	public int enemyGoal;
	//A boolean that indicates whether or not we are in a boss fight
	private static bool bossFight = false;
	//The canvas for the win ui
	public Canvas winCanvas;
	//The win score text
	public Text winScoreText;

	void Start() {
		//Instantiate all of the private variables
		gameOver = false;
		restart = false;
		winner = false;
		restartText.text = "";
		gameOverText.text = "";
		pickUpText.text = "";
		enemyKC = 0;
		
		UpdateScore ();

		//Start spawning stuff
		StartCoroutine(SpawnWaves());
		StartCoroutine (spawnPowerUps ());
	}

	void Update() {
		//Restart the game if the flag is true and the player presses the 'R' Key.
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				//Make sure we can reset even in the boss fight
				if (bossFight) {
					bossFight = false;
				}
				//Make sure to reset the score when we die
				score = 0;
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
			if (!bossFight) {
				for (int i = 0; i < hazardCount; i++) {
					//Pick a random hazard
					GameObject hazard = hazards [Random.Range (0, hazards.Length)];
					//Determine the spawn position from the spawnValues and a random x value
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					//A Quaternion with no rotation
					Quaternion spawnRotation = Quaternion.identity;
					//Spawn the hazard
					Instantiate (hazard, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (spawnWait);
				}
			}
			yield return new WaitForSeconds (waveWait);

			//Check to see if the game is over and display the restart text
			if (gameOver) {
				restartText.text = "Press 'R' for Restart";
				restart = true;
				//Break us out of the while loop
				break;
			}
			else if (winner) {
				//Wait for 5 seconds before transitioning to the main menu
				yield return new WaitForSeconds(5);
				SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
				break;
			}
		}
	}

	IEnumerator spawnPowerUps() {
		//Wait for the wave to start
		yield return new WaitForSeconds (startWait);

		//Start looping
		while (true) {
			yield return new WaitForSeconds (powerUpWait);
			//Randomly pick a power up
			int powerUp = new System.Random().Next(0, powerUps.Length);
			//A random position in our players bounds
			Vector3 spawnPosition = new Vector3 (Random.Range (powerUpBounds.xMin, powerUpBounds.xMax), 0, Random.Range (powerUpBounds.zMin, powerUpBounds.zMax));
			//A Quaternion with no rotation
			Quaternion spawnRotation = Quaternion.identity;

			//Spawn the power up!
			Instantiate (powerUps [powerUp], spawnPosition, spawnRotation);

			if (gameOver || winner) {
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

	public void Winner() {
		//You can only win if the game isn't over
		if (!gameOver) {
			winner = true;
			//turn on the win ui, and disable the player
			winCanvas.gameObject.SetActive(true);
			winScoreText.text = "Your final score was: " + score;
			player.enabled = false;
			//Make sure to reset the score and bossFight
			score = 0;
			bossFight = false;
		}
	}

	public void enemyKilled() {
		enemyKC++;

		if (enemyKC >= enemyGoal && !gameOver) {
			bossFight = true;
			pickUpText.text = "";
			SceneManager.LoadScene ("Boss", LoadSceneMode.Single);
		}

	}

		
}
