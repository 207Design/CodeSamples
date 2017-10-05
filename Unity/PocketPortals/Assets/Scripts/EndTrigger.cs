using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour {


	float delayTime;
	bool goalReached = false;

	// Update is called once per frame
	void Update () {
		if (goalReached == true) {
			delayTime += Time.deltaTime;
			if (delayTime >= 0.5f) {
				delayTime = 0f;
				goalReached = false;
			}
		}
	}

    // Begin summary
    // Player object colliding with trigger
    // End summary
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player") == true) {
			goalReached = true;
			other.GetComponent<PlayerObjectMovement> ().MoveToEnd (transform.position);
		}
	}
}
