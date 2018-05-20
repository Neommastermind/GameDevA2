using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	//The play area contstraints
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	//A reference to the players rigid body component
	private Rigidbody rb;
	//The players speed
	public float speed;
	//The play area boundary
	public Boundary boundary;
	//The players tilt
	public float tilt;
	//The players firerate
	public float fireRate;
	//The bolt to shoot
	public GameObject shot;
	//the spawn object for the bolts
	public Transform shotSpawn;
	//When the player can shoot another bolt
	private float nextFire;

	void Start() {
		//Get the Rigidbody component
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3 (Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
