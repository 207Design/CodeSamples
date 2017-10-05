using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelObject : MonoBehaviour {

	[SerializeField]
	LevelConfigData configData;

    PuzzleManager puzzleManager;


    List<GameObject> pieceList = new List<GameObject>();

	GameObject puzzlePiece;
	GameObject tutorialObject;
	GameObject tutorial;

	bool levelConfigured;

    int nextPuzzleID;

    PuzzlePiece puzzlePieceScript;

	// Begin summary
	// Get puzzle manager script
	// Begin setting up the level based on configData set in inspector
	// End summary
	void Start() {
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
		ConfigLevel ();
	}
		

	// Begin summary
	// Instantiate puzzle pieces based on configData
	// End summary
	public void ConfigLevel() {
        for (int i = 0; i < configData.StartPieces.Length; i++)
        {
            nextPuzzleID += 1;
            puzzlePiece = Instantiate(configData.StartPieces[i].puzzlePiece, configData.StartPieces[i].puzzlePiecePos.position, configData.StartPieces[i].puzzlePiecePos.rotation);
            puzzlePiece.transform.SetParent(transform);
            pieceList.Add(puzzlePiece);
            puzzlePieceScript = puzzlePiece.GetComponent<PuzzlePiece>();
            puzzlePieceScript.SetPuzzleID(nextPuzzleID);
            puzzlePieceScript.SetupRoom();
        }

        for (int i = 0; i < configData.EndPieces.Length; i++)
        {
            nextPuzzleID += 1;
            puzzlePiece = Instantiate(configData.EndPieces[i].puzzlePiece, configData.EndPieces[i].puzzlePiecePos.position, configData.EndPieces[i].puzzlePiecePos.rotation);
            puzzlePiece.transform.SetParent(transform);
            pieceList.Add(puzzlePiece);
            puzzlePieceScript = puzzlePiece.GetComponent<PuzzlePiece>();
            puzzlePieceScript.SetPuzzleID(nextPuzzleID);
            puzzlePieceScript.SetupRoom();
        }

        for (int i = 0; i < configData.PuzzlePieces.Length; i++)
        {
            nextPuzzleID += 1;
            puzzlePiece = Instantiate(configData.PuzzlePieces[i].puzzlePiece, configData.PuzzlePieces[i].puzzlePiecePos.position, configData.PuzzlePieces[i].puzzlePiecePos.rotation);
            puzzlePiece.transform.SetParent(transform);

            if (configData.PuzzlePieces[i].isStatic == true)
            {
                puzzlePiece.GetComponent<MoveablePiece>().restrictions = MoveablePiece.overrideMovement.noMovement;
            }
            else
            {
                tutorialObject = puzzlePiece;
            }

            pieceList.Add(puzzlePiece);
            puzzlePieceScript = puzzlePiece.GetComponent<PuzzlePiece>();
            puzzlePieceScript.SetPuzzleID(nextPuzzleID);
            puzzlePieceScript.SetupRoom();
        }

        if (configData.tutorial != null)
        {
            tutorial = Instantiate(configData.tutorial, tutorialObject.transform);
            tutorial.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            tutorial.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }

        puzzleManager.LevelSetupDone(pieceList);
    }
}


// Begin summary
// Arrays to be used for setting available puzzle pieces for each puzzle
// End summary
[Serializable]
public struct LevelConfigData {

	public puzzlePieceData[] StartPieces;
	public puzzlePieceData[] EndPieces;
	public puzzlePieceData[] PuzzlePieces;
	public GameObject tutorial;
}
	

[Serializable]
public struct puzzlePieceData {

	public GameObject puzzlePiece;
	public Transform puzzlePiecePos;
	public bool isStatic;
}
