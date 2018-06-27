using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManeuver : MonoBehaviour {
	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public Boundary boundary;

	private float currentSpeed;
	private Vector2 targetManeuver;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		currentSpeed = rb.velocity.z;
		StartCoroutine (Evade ());
	}

	IEnumerator Evade() {
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));

		while (true) {
			targetManeuver = new Vector2(Random.Range (1, dodge) * -Mathf.Sign(transform.position.x), Random.Range (1, dodge) * -Mathf.Sign(transform.position.z - 11));
			yield return new WaitForSeconds (Random.Range(maneuverTime.x, maneuverTime.y));
			targetManeuver = new Vector2(0.0f, 0.0f);
			yield return new WaitForSeconds (Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 newManuever = new Vector2(Mathf.MoveTowards (rb.velocity.x, targetManeuver.x, Time.deltaTime * smoothing), Mathf.MoveTowards (rb.velocity.z, targetManeuver.y, Time.deltaTime * smoothing));

		rb.velocity = new Vector3 (newManuever.x, 0.0f, newManuever.y);

		rb.position = new Vector3 (Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

		//The ships x rotation for tilt
		float xRot = 0.0f;
		if (Mathf.Sign (rb.velocity.z) == 1)
			//If the ship is going upwards tilt the ship
			xRot = rb.velocity.z * tilt;
		
		rb.rotation = Quaternion.Euler (xRot, 0.0f, rb.velocity.x * -tilt);
	}
}
