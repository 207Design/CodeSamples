using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    public List<GameObject> puzzlePieces = new List<GameObject>();

    [SerializeField]
    List<GameObject> allTeleporters;

    [SerializeField]
    List<GameObject> playerObjects = new List<GameObject>();

    List<bool> playerObjectInGoal = new List<bool>();

    [SerializeField]
    GameObject playerObject;

    [SerializeField]
    GameManager gameManager;

    public Material staticWallMaterial;
    public Material staticBackWallMaterial;

    public Material switchDisabled;
    public Material switchEnabled;

    bool stateIsGame = false;

    List<GameObject> objectsMoving = new List<GameObject>();

    // Begin summary
    // From LevelObject when level has been setup
    // End summary
    public void LevelSetupDone(List<GameObject> puzzlePieceList) {
        puzzlePieces = puzzlePieceList;
        ResetPlayerObjects(true);
        PlayerMovement(true);
        stateIsGame = true;
    }

    // Begin summary
    // Add or remove object form list based on if it is being moved or not
    // End summary
    public void MovingObjects(GameObject objectMoving, bool action) {
        if (action == true) {
            objectsMoving.Add(objectMoving);
        } else {
            objectsMoving.Remove(objectMoving);
        }

        if (stateIsGame == true) {
            if (objectsMoving.Count == 0) {
                PlayerMovement(true);
            } else {
                PlayerMovement(false);
                for (int i = 0; i < playerObjectInGoal.Count; i++) {
                    playerObjectInGoal[i] = false;

                }
                ResetSwitch();
            }
        }
    }

    // Begin summary
    // Remove previous player objects if any and instantiate new one at StartPoint positions across the level
    // End summary
    public void ResetPlayerObjects(bool instantiateNew) {
        for (int i = 0; i < playerObjects.Count; i++) {
            Destroy(playerObjects[i].gameObject);
        }
        playerObjects.Clear();
        playerObjectInGoal.Clear();

        if (instantiateNew)
        {
            GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
            for (int i = 0; i < startPoints.Length; i++)
            {
                GameObject spawnedPlayerObject = Instantiate(playerObject, startPoints[i].transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                spawnedPlayerObject.GetComponent<PlayerObjectMovement>().StartTransform = startPoints[i].transform;

                playerObjects.Add(spawnedPlayerObject);
                playerObjectInGoal.Add(false);
            }
        }
    }

    // Begin summary
    // Set player object to having reached a goal
    // End summary
    public void PlayerObjectGoalReached(GameObject playerObject)
    {
        if (stateIsGame)
        {
            int i = playerObjects.IndexOf(playerObject);
            playerObjectInGoal[i] = true;
            for (int x = 0; x < playerObjectInGoal.Count; x++)
            {
                if (playerObjectInGoal[x] == false)
                {
                    return;
                }
            }
            AllPlayerObjectsInGoal();
        }
    }

    // Begin summary
    // Puzzle has been successfully solved
    // End summary
    public void AllPlayerObjectsInGoal()
    {
        stateIsGame = false;
        gameManager.ControlGameManagerAnimator(2, false);
        for (int x = 0; x < puzzlePieces.Count; x++)
        {
            if (puzzlePieces[x].GetComponent<MoveablePiece>())
            {
                puzzlePieces[x].GetComponent<MoveablePiece>().Moveable = false;
            }
        }
    }
		
    // Begin summary
    // Tell player object to reset and intialize movement
    // End summary
	void PlayerMovement(bool status) {
        if (playerObjects.Count > 0) {
            for (int i = 0; i < playerObjects.Count; i++) {
                playerObjects[i].GetComponent<PlayerObjectMovement>().ResetObject(status);
            }
        }
	}

    // Begin summary
    // Tell switch objects in scene to reset
    // End summary
    void ResetSwitch()
    {
        for (int i = 0; i < puzzlePieces.Count; i++) {
            if (puzzlePieces[i].GetComponentInChildren<SwitchObject>()) {
                puzzlePieces[i].GetComponentInChildren<SwitchObject>().ResetSwitch();
            }
        }
    }

    // Begin summary
    // Return all available teleporters to caller
    // End summary
    public List<GameObject> GetAvailableTeleporters()
    {
        return allTeleporters;
    }

    // Begin summary
    // Add teleporter object to list of teleporters
    // End summary
    public void AddAvailableTeleport(GameObject teleporter) {
        allTeleporters.Add(teleporter);
    }

    // Begin summary
    // Clear list of all teleporters
    // End summary
    void ResetAvailableTeleporters() {
        allTeleporters.Clear();
    }

    // Begin summary
    // Send amount of objects currently being moved to the caller
    // End summary
    public int GetObjectsMovingAmount() {
        return objectsMoving.Count;
    }

    // Begin summary
    // Return of the sent object exists within the list of moving objects
    // End summary
    public bool ObjectExistsWithingMovingList(GameObject gameObject) {
        return objectsMoving.Contains(gameObject);
    }

    // Begin summary
    // Reset player object and available teleporters
    // End summary
    public void ResetPuzzleManager() {
        ResetPlayerObjects(false);
        ResetAvailableTeleporters();
    }
}
