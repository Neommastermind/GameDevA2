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
		if (other.CompareTag("Boundary")|| other.CompareTag("Enemy") || other.CompareTag("EnemyShip") || other.CompareTag("Boss") || other.CompareTag("PowerUp I") || other.CompareTag("PowerUp SU")) {
			return;
		}

		if (explosion != null && !gameObject.CompareTag ("Boss")) {
			//Create the asteroid explosion effect
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.CompareTag ("Player") && !gameController.player.isInvincible) {
			//Create the player explosion effect
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}
			
		//If the owner of this script is an enemy, then make sure that we inform the gamecontroller
		if (gameObject.CompareTag ("EnemyShip")) {
			gameController.enemyKilled ();
		}

		if (!gameObject.CompareTag ("Boss")) {
			//Add the asteroids value to the gameControllers score
			gameController.AddScore (scoreValue);
		}

		if (other.CompareTag("Player") && gameController.player.isInvincible) {
			if(!gameObject.CompareTag("Boss"))
				//Destroy the object the player collided with and return.
				Destroy (gameObject);
			
			return;
		}

		if (gameObject.CompareTag ("Boss")) {
			BossController boss = gameObject.GetComponent<BossController> ();
			//Take points off of the boss's hp, depending on the kind of shot
			if (other.CompareTag ("SuperShot")) {
				boss.Hit (3);
			} 
			else if(!other.CompareTag("Player")) {
				//The player cannot ram into the boss
				//Only bolts should do damage really
				boss.Hit (1);
			}

			if (boss.hitPoints <= 0) {
				//Trigger the boss explosion, get points, destroy the objects, and win!
				Instantiate (explosion, transform.position, transform.rotation);
				gameController.AddScore (scoreValue);
				Destroy (gameObject);
				gameController.Winner ();
			}
		}

		//Destroy the object that collides with the asteroid and the asteroid
		//Unless it is the boundary
		Destroy (other.gameObject);

		if (!gameObject.CompareTag ("Boss")) {
			Destroy (gameObject);
		}
	}
}
