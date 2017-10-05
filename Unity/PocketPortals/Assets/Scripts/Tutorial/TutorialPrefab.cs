using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrefab : MonoBehaviour {

	[SerializeField]
	Animator animator;

	[SerializeField]
	bool rotateTutorial = false;

	[SerializeField]
	bool dragTutorial = false;


	void OnEnable()
	{
		MoveablePiece.OnDragEvent += DisableTutorialDrag;
		MoveablePiece.OnRotateEvent += DisableTutorialRotate;
	}

	void OnDisable()
	{
		MoveablePiece.OnDragEvent -= DisableTutorialDrag;
		MoveablePiece.OnRotateEvent -= DisableTutorialRotate;
	}
		

	// Use this for initialization
	void Start () {
		if (dragTutorial == true) {
			animator.SetTrigger ("enableDrag");
		}

		if (rotateTutorial == true) {
			animator.SetTrigger ("enableRotate");
		}
	}

	// Begin summary
	// Disable rotate tutorial on rotate event
	// End summary
	public void DisableTutorialRotate() {
		if (rotateTutorial == true)
			animator.transform.parent.gameObject.SetActive (false);
	}

	// Begin summary
	// Disable move tutorial on move even
	// End summary
	public void DisableTutorialDrag() {
		if (dragTutorial == true) 
			animator.transform.parent.gameObject.SetActive (false);
	}
}
