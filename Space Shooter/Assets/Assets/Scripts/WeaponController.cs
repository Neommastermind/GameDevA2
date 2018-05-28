using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public float delay;

	private AudioSource audioSource;
	//The type of shots this enemy will use
	private int shotType;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		shotType = Random.Range (0, 3);
		InvokeRepeating ("Fire", delay, fireRate);
	}

	void Fire() {
		//Change the shot type depending on the count
		if (shotType == 0) {
			//Single shot
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		} 
		else if (shotType == 1) {
			//Two shots
			Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 210, shotSpawn.rotation.z));
			Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 210, shotSpawn.rotation.z));
		} 
		else if (shotType == 2) {
			//Three shots
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 210, shotSpawn.rotation.z));
			Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 210, shotSpawn.rotation.z));
		}

		audioSource.Play ();
	}

}
