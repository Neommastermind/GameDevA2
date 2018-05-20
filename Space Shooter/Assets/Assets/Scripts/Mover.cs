using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
	//A reference to the bolts rigid body component
	private Rigidbody rb;
	//The bolts speed
	public float speed;

	// Use this for initialization
	void Start () {
		//Get the Rigidbody component
		rb = GetComponent<Rigidbody>();

		rb.velocity = transform.forward * speed;
	}
}
