using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	//The super shot to shoot
	public GameObject superShot;
	//the spawn object for the bolts
	public Transform shotSpawn;
	//When the player can shoot another bolt
	private float nextFire;
	//A count to keep track of which shot type to use
	private int shotCount;
	//A boolean to tell whether the player is invincible or not
	public bool isInvincible;
	//Keeps track of how long invincibility should last
	public float invincibleDuration;
	//Keeps track of how much time has passed since the invincibility began
	private float invincibleTime;
	//A boolean to tell whether the player has an upgraded gun or not
	public bool isUpgraded;
	//Keeps track of how long an upgrade is supposed to last
	public float upgradeDuration;
	//Keeps track of how much time has passed since the upgrade began
	private float upgradeTime;
	//The players animator
	private Animator playerAnimator;
	//The charge duration
	public float chargeDuration;
	//The charge time so far
	private float chargeTime;
	//The amount of super shots you can fire
	public int superAmount;

	void Start() {
		//Get the Rigidbody component
		rb = GetComponent<Rigidbody>();
		//Get the Audio Source component
		audioSource = GetComponent<AudioSource>();
		//Get the players animator
		playerAnimator = GetComponent<Animator>();
		//Initialize the shotCount
		shotCount = 0;
		chargeTime = 0;
	}

	void Update() {
		//Toggle godmode
		if (Input.GetKeyDown (KeyCode.G)) {
			GodModeToggle ();
		}

		if (isInvincible && invincibleDuration > invincibleTime) {
			//Update the time
			invincibleTime += Time.deltaTime;

			//Reset the invincibility data
			if (invincibleTime >= invincibleDuration) {
				isInvincible = false;
				invincibleTime = 0.0f;
				//Turn the shield off
				playerAnimator.SetBool("isInvincible", false);
			}
		}

		//Start charging up a shot if the user is holding down the button
		if (Input.GetButton ("Fire1") && superAmount > 0 && Time.time > nextFire) {
			//Start charging
			chargeTime += Time.deltaTime;

			//Shoot the super shot if the charge threshold has been reached
			if (chargeTime >= chargeDuration) {
				//Update the time of nextFire
				nextFire = Time.time + fireRate;
				//You can use 1 less super shot now
				superAmount -= 1;
				//Create a super shot
				Instantiate (superShot, shotSpawn.position, shotSpawn.rotation);
				//Reset the chargeTime
				chargeTime = 0;
				//Play the shooting sounds
				audioSource.Play();
			}
		}

		//Shoot normal bolts when you let go of the mouse button
		if (((superAmount == 0 && Input.GetButton("Fire1")) || (superAmount > 0 && Input.GetMouseButtonUp(0) && chargeTime < chargeDuration)) && Time.time > nextFire) {
			//Reset the charge time
			chargeTime = 0;
			//Update the time of nextFire
			nextFire = Time.time + fireRate;

			if (isUpgraded && upgradeDuration > upgradeTime) {
				//Update the time
				upgradeTime += Time.deltaTime;

				//Reset the upgrade data if we have reached the time limit
				if (upgradeTime >= upgradeDuration) {
					isUpgraded = false;
					upgradeTime = 0.0f;
					//Reset the shot counter as well to get us back to default firing
					shotCount = 0;
				}

				//Three shots
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 30, shotSpawn.rotation.z));
				Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 30, shotSpawn.rotation.z));
			} 
			else {
				//Change the shot type depending on the count
				if (shotCount == 0) {
					//Single shot
					Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				} else if (shotCount == 1) {
					//Two shots
					Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y + 30, shotSpawn.rotation.z));
					Instantiate (shot, shotSpawn.position, Quaternion.Euler (shotSpawn.rotation.x, shotSpawn.rotation.y - 30, shotSpawn.rotation.z));
				} else if (shotCount == 2) {
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

	public void upgrade() {
		isUpgraded = true;
		upgradeTime = 0.0f;
	}

	public void invincible() {
		isInvincible = true;
		invincibleTime = 0.0f;

		//Turn on the shield
		playerAnimator.SetBool("isInvincible", true);
	}

	private void GodModeToggle() {
		if (invincibleDuration != 0) {
			isInvincible = true;
			invincibleDuration = 0;
			//Turn on the shield
			playerAnimator.SetBool("isInvincible", true);
		}
		else {
			isInvincible = false;
			invincibleDuration = 5;
			//Turn on the shield
			playerAnimator.SetBool("isInvincible", false);
		}
	}
}
