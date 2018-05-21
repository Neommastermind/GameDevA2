using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {
	//The asteroids tumble
	public float tumble;
	//A reference to the asteroids rigidbody
	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody> ();

		//Set the angular velocity to a random vector3 of length 1 multiplied by some tumble value
		rb.angularVelocity = Random.insideUnitSphere * tumble;
	}
}
