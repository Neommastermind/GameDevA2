using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {
	//The time for the object to live
	public float lifetime;

	void Start() {
		//Destroy the game object after a certain amount of time
		Destroy (gameObject, lifetime);
	}
}
