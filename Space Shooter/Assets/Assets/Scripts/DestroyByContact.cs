using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
	//The asteroid explosion effect
	public GameObject explosion;
	//The player explosion effect
	public GameObject playerExplosion;
	//The value for destroying an asteroid
	public int scoreValue;
	//A reference to our gameController
	private GameController gameController;

	void Start() {
		//Find the first instance of the game controller
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		//Set our GameController if it was found
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}

		//Log the error if we get it after all of this
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Boundary")|| other.CompareTag("Enemy")) {
			return;
		}

		if (explosion != null) {
			//Create the asteroid explosion effect
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.CompareTag("Player")) {
			//Create the player explosion effect
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}

		//Add the asteroids value to the gameControllers score
		gameController.AddScore(scoreValue);
		//Destroy the object that collides with the asteroid and the asteroid
		//Unless it is the boundary
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
