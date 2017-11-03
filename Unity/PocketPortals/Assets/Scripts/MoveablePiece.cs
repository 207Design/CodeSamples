using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveablePiece : MonoBehaviour{

	public delegate void DraggingObject();
	public static event DraggingObject OnDragEvent;

	public delegate void RotatingObject();
	public static event RotatingObject OnRotateEvent;

	[SerializeField]
	GameObject GUICanvas;

	bool moveable;

    public bool Moveable
    {
        get
        {
            return moveable;
        }
        set
        {
            moveable = value;
        }
    }

	public enum overrideMovement {noRestrictions, noMovement};
	public overrideMovement restrictions = overrideMovement.noRestrictions;

	bool rotate;

	Vector3 screenPointOrigin;
	Vector3 screenPointNew;
	Vector3 previousRotation;
    float rotateDistance;

	float newRotationZ;
	float newRotation;

	GameObject blockingImage;

	PuzzleManager puzzleManager;

    float doubleClickDelay = 0.5f;
    float doubleClickTime;

    float lerpPos = 1;
    float lerpPosModifier = 20;

    bool doNewRotation;
    float lerpRot;
    float lerpRotModifier = 5;

    Vector3 previousPos;
    Vector3 previousRot;

    Vector3 screenPoint;
    Vector3 offset;

    bool beingDragged;
    bool isColliding;

    // Use this for initialization
    void Start () {
		if (GUICanvas.transform.Find ("BlockIndicatorImage") != null) {
			blockingImage = GUICanvas.transform.Find ("BlockIndicatorImage").gameObject;
			blockingImage.SetActive (false);
		}
			
		if (restrictions == overrideMovement.noRestrictions) {
			moveable = true;
		} else {
			transform.GetComponent<BoxCollider> ().enabled = false;
			GUICanvas.SetActive (false);
		}

		puzzleManager = GameObject.Find ("PuzzleManager").GetComponent<PuzzleManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (moveable == true) {
			if (restrictions != overrideMovement.noMovement) {

				if (rotate == true) {
					doubleClickTime += Time.deltaTime;
				}

				if (doubleClickTime >= doubleClickDelay) {
					rotate = false;
					doubleClickTime = 0f;
				}
			}

			if (doNewRotation == true) {
				lerpRot += Time.deltaTime * lerpRotModifier;
				transform.rotation = Quaternion.Euler (Vector3.Lerp (previousRot, new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newRotationZ), lerpRot));
				if (lerpRot >= 1f) {
					doNewRotation = false;
					lerpRot = 0f;
				}
			}
				

			if (beingDragged == true) {
				Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offset;
				transform.position = new Vector3 (cursorPosition.x, cursorPosition.y, 0f);

				blockingImage.SetActive (isColliding);
			} else if (isColliding == true) {
				if (lerpPos < 1)
					lerpPos += Time.deltaTime * lerpPosModifier;

				transform.position = Vector3.Lerp(previousPos, new Vector3 ((Mathf.Round(transform.position.x / 2) * 2), (Mathf.Round(transform.position.y / 2) * 2), 0f), lerpPos);;
				blockingImage.SetActive (isColliding);
			} else if (isColliding == false) {
				if (lerpPos < 1)
					lerpPos += Time.deltaTime * lerpPosModifier;

				transform.position = Vector3.Lerp(previousPos, new Vector3 ((Mathf.Round(transform.position.x / 2) * 2), (Mathf.Round(transform.position.y / 2) * 2), 0f), lerpPos);
				blockingImage.SetActive (isColliding);
			}

		}
		if (moveable == true) {
            bool thisObjectIsMoving = puzzleManager.ObjectExistsWithingMovingList(gameObject);

            if (beingDragged == true) {
				if (thisObjectIsMoving == false) {
					puzzleManager.MovingObjects (this.gameObject, true);
				}
			} else if (doNewRotation == true) {
				if (thisObjectIsMoving == false) {
					puzzleManager.MovingObjects (this.gameObject, true);
				}
			} else if (isColliding == true) {
				if (thisObjectIsMoving == false) {
					puzzleManager.MovingObjects (this.gameObject, true);
				}
			} else {
				if (thisObjectIsMoving == true) {
					puzzleManager.MovingObjects (this.gameObject, false);					
				}
			}
		}
	}

    // Begin summary
    // On Mouse button down, calculate offset to this objects position
    // End summary
	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}


    // Begin summary
    // Set being dragged to true
    // End summary
	public void OnDrag(){
		if (restrictions != overrideMovement.noMovement) {
				beingDragged = true;

			if (OnDragEvent != null) {
				OnDragEvent ();
			}
		}
	}

    // Begin summary
    // On end drag, set position of piece and set being dragged to false
	public void OnDragEnd() {
		if (restrictions != overrideMovement.noMovement) {
			previousPos = transform.position;
			lerpPos = 0;
			beingDragged = false;
		}
	}

    // Begin summary
    // Double click on piece to rotate
    // End summary
	public void ClickToRotate (){
		if (restrictions != overrideMovement.noMovement) {
			if (moveable == true) {
				if (doNewRotation == false) {
					if (rotate == true) {

						doNewRotation = true;
						newRotationZ = transform.rotation.eulerAngles.z + 90;
						previousRot = transform.rotation.eulerAngles;
						rotate = false;
						doubleClickTime = 0f;

						if (OnRotateEvent != null) {
							OnRotateEvent ();
						}
					} else {
						rotate = true;
					}
				}
			}
		}
	}

	List<Collider> collidersColliding = new List<Collider>();

    // Begin summary
    // Set isColliding to true if this object is colliding with another object
    // End summary
    void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag ("Player") == false) {
			collidersColliding.Add (collider);
			isColliding = true;
		}
	}

    // Begin summary
    // Set isColliding to false if this object is no longer colliding with another object
    // End summary
	void OnTriggerExit(Collider collider) {
		if (collidersColliding.Contains (collider) == true) {
			collidersColliding.Remove (collider);
		}
		if (collidersColliding.Count < 1) {
			isColliding = false;
		}
	}
}
