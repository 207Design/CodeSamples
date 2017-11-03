using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectMovement : MonoBehaviour {

	bool isMoving;

	Transform startTransform;

    public Transform StartTransform
    {
        set
        {
            startTransform = value;
        }
    }

    PuzzleManager puzzleManager;

	// Time for lowVelocity and for lerping
	float lowVelocityTime;
	float moveTime = 0;

	// Move to end variables
	bool moveToPos = false;
	Vector3 endPos;
	Vector3 currentPos;

	// Move through teleport
	float currentVelocity; 
	Vector3 currentAngularVelocity;
	Transform targetObject;

    // Use this for initialization
    void Start() {
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Begin summary
        // Reset player object if it's speed has been too low for .5 seconds
        // End summary
        if (isMoving == true)
        {
            if (GetComponent<Rigidbody>().velocity.magnitude < 0.3)
            {
                lowVelocityTime += Time.deltaTime;
            }
            else
            {
                lowVelocityTime = 0f;
            }
            if (lowVelocityTime > .5f)
            {
                ResetObject(true);
                lowVelocityTime = 0f;
                return;
            }
        }

        // Begin summary
        // Prevent player object from moving faster than max speed
        // End summary
        if (Vector3.Magnitude(GetComponent<Rigidbody>().velocity) > 12f)
        {
            GetComponent<Rigidbody>().velocity = Vector3.Normalize(GetComponent<Rigidbody>().velocity) * 12f;
        }

        // Begin summary
        // Lerp object to end position
        // End summary
        if (moveToPos == true)
        {
            moveTime += Time.deltaTime * 3;
            transform.position = Vector3.Lerp(currentPos, endPos, moveTime);

            if (moveTime >= 1)
            {
                moveToPos = false;
                moveTime = 0f;

                ResetObject(true);
                puzzleManager.PlayerObjectGoalReached(gameObject);
            }
        }
    }


    // Begin summary
    // Reset cube to start pos, with or without falling again
    // End summary
    public void ResetObject(bool enable) {
		MoveToStart ();
		GetComponent<Animator> ().SetTrigger ("reset");
		if (enable == true) {
			moveTime = 0;
			isMoving = true;
            StartMovement();
		} else {
			isMoving = false;
		}
	}

    // Begin summary
    // Enable collider and rigid body gravity on the player object
    // End summary
    void StartMovement() {
		GetComponent<BoxCollider> ().enabled = true;
		GetComponent<Rigidbody> ().useGravity = true;
		SetStartVelocity ();
	}
		
    // Begin summary
	// Move to start position of the puzzle
    // End summary
	void MoveToStart() {
		if (transform != null) {
			transform.position = startTransform.position;

			ResetVelocity ();
			GetComponent<BoxCollider> ().enabled = false;
			GetComponent<Rigidbody> ().useGravity = false;
		}
	}

    // Begin summary
	// Move to end position of the puzzle
    // End summary
	public void MoveToEnd(Vector3 pos) {
		currentPos = transform.position;
		endPos = pos;
		endPos.z = 0f;

		moveToPos = true;
	}
		
    // Begin summary
	// Move through teleport to new position
    // Begin summary
	public void MoveWithTeleport(Transform toObject) {
		if (this.gameObject != null) {
			if (toObject != null) {
				targetObject = toObject;
				GetComponent<Animator> ().SetTrigger ("disappear");

				currentVelocity = Vector3.Magnitude (GetComponent<Rigidbody> ().velocity);
				currentAngularVelocity = GetComponent<Rigidbody> ().angularVelocity;
			}
		}
	}

    // Begin summary
	// Teleport animation done and ready to emerge on other side
    // End summary
	public void ReadyToTeleport() {
		if (isMoving == true) {
			if (transform != null) {
				GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
				transform.position = targetObject.position;

				GetComponent<Animator> ().SetTrigger ("appear");

				GetComponent<Rigidbody> ().velocity = targetObject.forward * currentVelocity;
				GetComponent<Rigidbody> ().angularVelocity = currentAngularVelocity;
			}
		}
	}

    // Begin summary
	// Reset the velocity of the player object
    // End summary
	void ResetVelocity() {
		GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		GetComponent<Rigidbody> ().angularVelocity = new Vector3 (0, 0, 0);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
	}

    // Begin summary
	// Set velocity at the start of the puzzle
    // End trigger
	void SetStartVelocity() {
		GetComponent<Rigidbody> ().angularVelocity = new Vector3(0, 0, 2);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		GetComponent<Rigidbody> ().velocity = startTransform.forward * 6.000f;
	}
}