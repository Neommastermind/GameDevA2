using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public Boundary boundary;

	private float currentSpeed;
	private float targetManeuver;
	private Rigidbody rb;
	private int evadeType;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		currentSpeed = rb.velocity.z;
		evadeType = Random.Range (0, 3);
		StartCoroutine (Evade ());
	}

	IEnumerator Evade() {
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));

		while (true) {
			//Random maneuver
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (evadeType == 0) {
			float newManuever = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);

			rb.velocity = new Vector3 (newManuever, 0.0f, currentSpeed);
		} 
		else if (evadeType == 1) {
			float newManuever = Mathf.MoveTowards (rb.velocity.x, dodge * Mathf.Cos(rb.position.z / Mathf.PI), Time.deltaTime * smoothing);

			rb.velocity = new Vector3 (newManuever, 0.0f, currentSpeed);
		}

		rb.position = new Vector3 (Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
