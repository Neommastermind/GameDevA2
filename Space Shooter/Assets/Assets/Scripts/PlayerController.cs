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
	//A reference to the players Audio Source component
	private AudioSource audioSource;
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
	//A count to keep track of which shot type to use
	private int shotCount;

	void Start() {
		//Get the Rigidbody component
		rb = GetComponent<Rigidbody>();
		//Get the Audio Source component
		audioSource = GetComponent<AudioSource>();
		//Initialize the shotCount
		shotCount = 0;
	}

	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			//Update the time of nextFire
			nextFire = Time.time + fireRate;

			//Change the shot type depending on the count
			if (shotCount == 0) {
				//Single shot
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			} 
			else if (shotCount == 1) {
				//Two shots
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 30, shotSpawn.rotation.z));
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 30, shotSpawn.rotation.z));
			} 
			else if (shotCount == 2) {
				//Three shots
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 30, shotSpawn.rotation.z));
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 30, shotSpawn.rotation.z));
			}

			//Update the shot count
			shotCount++;

			//Reset the shot count if it exceeds the maximum
			if (shotCount > 2) {
				shotCount = 0;
			}

			//Play the shooting sounds
			audioSource.Play();
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
