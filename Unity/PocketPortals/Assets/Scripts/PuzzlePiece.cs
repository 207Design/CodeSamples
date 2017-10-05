using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour {

	[SerializeField]
	GameObject teleportObjectPrefab;

	PuzzleManager puzzleManager;

    int puzzlePieceID;

    void Start() {
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
    }

    // Begin summary
    // Instantiate teleport objects
    // End summary
    public void SetupRoom() {
        if (puzzleManager == null)
        {
            puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        }
        foreach (Transform child in transform) {
			if (child.gameObject.tag == "TeleportPoint") {
				GameObject spawnedTeleport = Instantiate (teleportObjectPrefab, child.position, child.rotation);
				spawnedTeleport.transform.SetParent (child);

                spawnedTeleport.GetComponent<TeleporterObject>().SetRoomID(puzzlePieceID);
                puzzleManager.AddAvailableTeleport(spawnedTeleport);
            }
		}
		if (GetComponent<MoveablePiece> () != null) {
			if (GetComponent<MoveablePiece> ().restrictions == MoveablePiece.overrideMovement.noMovement) {
				ChangeWallColor ();
			}
		}
	}

	// Begin summary
	// Change the material of the walls
	// End summary
	private void ChangeWallColor() {
        if (puzzleManager == null) {
            puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        }
		MeshRenderer[] wallRenderers = transform.FindChild ("Walls").GetComponentsInChildren<MeshRenderer> ();
		for (int i = 0; i < wallRenderers.Length; i++) {
			if (wallRenderers [i].gameObject.CompareTag ("wallBack") == false) {
				wallRenderers [i].material = puzzleManager.staticWallMaterial;
			} else {
				wallRenderers [i].material = puzzleManager.staticBackWallMaterial;;
			}
		}
	}

    // Begin summary
    // Set puzzle piece ID
    // End summary
    public void SetPuzzleID(int id) {
        puzzlePieceID = id;
    }
}
