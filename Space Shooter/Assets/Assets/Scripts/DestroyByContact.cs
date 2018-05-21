using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {
	//The asteroid explosion effect
	public GameObject explosion;
	//The player explosion effect
	public GameObject playerExplosion;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Boundary") {
			return;
		}
		//Create the asteroid explosion effect
		Instantiate (explosion, transform.position, transform.rotation);

		if (other.tag == "Player") {
			//Create the player explosion effect
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
		}

		//Destroy the object that collides with the asteroid and the asteroid
		//Unless it is the boundary
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
