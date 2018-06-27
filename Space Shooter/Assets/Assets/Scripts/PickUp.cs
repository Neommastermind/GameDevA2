using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

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
		//Only trigger for the player
		if (other.CompareTag ("Player")) {
			//Apply the appropriate pick up upgrade to the player
			if (gameObject.CompareTag ("PowerUp I")) {
				gameController.pickUpText.text = "Invincibility upgrade acquired";
				gameController.player.invincible ();
			} 
			else if (gameObject.CompareTag ("PowerUp SU")) {
				gameController.pickUpText.text = "Firerate upgrade acquired";
				gameController.player.upgrade ();
			}

			//Destroy the pick up now that we are done
			Destroy(gameObject);
		}
	}
}
