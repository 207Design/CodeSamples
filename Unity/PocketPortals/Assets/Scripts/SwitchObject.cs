using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour {

	[SerializeField]
	GameObject controlledObject;

	PuzzleManager puzzleManager;

	void Start() {
		puzzleManager = GameObject.Find ("PuzzleManager").GetComponent<PuzzleManager> ();
	}

	// Begin summary
	// Reset the switch
	// End summary
	public void ResetSwitch() {
		controlledObject.SetActive (true);

		GetComponentInChildren<MeshRenderer>().material = puzzleManager.switchDisabled;
	}

	// Begin summary
	// On player object colliding with the trigger, set the switch to enabled
	// End summary
	void OnTriggerEnter(Collider other) {
		if (controlledObject.activeSelf == true) {
			if (other.gameObject != null) {
				if (other.gameObject.tag == "Player") {
					controlledObject.SetActive (false);

					GetComponentInChildren<MeshRenderer>().material = puzzleManager.switchEnabled;
				}
			}
		}
	}
}
