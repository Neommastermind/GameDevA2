using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	//The Bosses hit points
	public int hitPoints;
	//The enemy bolts
	public GameObject shot;
	//Where to spawn the shots
	public Transform shotSpawn;
	//How often to fire
	public float fireRate;
	//How long to wait initially
	public float delay;

	//The type of shot the boss will use
	private int shotType;
	//The bosses rigidbody
	private Rigidbody rb;
	//The ships audio
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		rb.velocity = new Vector3(0.0f, 0.0f, -7);
		//InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire() {
		shotType = Random.Range (0, 6);

		//Change the shot type depending on the count
		if (shotType == 0) {
			//6 shots in a 60 degree cone
			CreateShots(60, 6);
		} 
		else if (shotType == 1) {
			//6 shots in a 90 degree cone
			CreateShots(90, 6);
		} 
		else if (shotType == 2) {
			//3 shots in a 60 degree cone
			CreateShots(60, 3);
		}
		else if (shotType == 3) {
			//3 shots in a 90 degree cone
			CreateShots(90, 3);
		}
		else if (shotType == 4) {
			//6 shots in a 30 degree cone
			CreateShots(30, 6);
		}
		else if (shotType == 5) {
			//4 shots in a 30 degree cone
			CreateShots(30, 4);
		} 

		audioSource.Play ();
	}

	//Spawns x shots in a defined cone
	private void CreateShots(int degreeSpread, int shotCount) {
		float degreesPerShot = degreeSpread / (shotCount - 1);
		int direction = -1;

		for (int i = 0; i < shotCount; i++) {
			Instantiate (shot, new Vector3(shotSpawn.position.x, 0.0f, shotSpawn.position.z), Quaternion.Euler (shotSpawn.rotation.x, (shotSpawn.rotation.y + degreesPerShot * i + 90 + ((180 - degreeSpread)/2)) * direction, shotSpawn.rotation.z));
		}
	}

	public void Hit(int damage) {
		hitPoints -= damage;
	}
}
