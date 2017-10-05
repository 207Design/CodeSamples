using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour {

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    int roomID;

    PuzzleManager puzzleManager;

    [SerializeField]
    GameObject closestTarget;

    [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField]
    Material connectedMat;

    [SerializeField]
    Material disconnectedMat;


    bool teleportActive;

    float maxDistance = 7f;
    float distanceCheck;
    GameObject currentlyClosestTarget;

    List<GameObject> targets = new List<GameObject>();

    // Begin summary
    // On Start, get TeleportManager from scene
    // End summary
    void Start()
    {
        puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
        SetAvailableTeleportersList();
    }

    // Begin summary
    // Check distance to every teleport that does not belong in the same room as this
    // If another teleport is within max distance and is has this teleport as its closest one, attach line renderer
    // End summary
    void Update()
    {
        if (targets.Count > 0)
        {
            distanceCheck = maxDistance;
            currentlyClosestTarget = null;

            // Begin summary
            // Find the teleporter is closest to this one and which is within the max distance
            // End summary
            for (int i = 0; i < targets.Count; i++)
            {
                if (Vector3.Distance(gameObject.transform.position, targets[i].transform.position) < distanceCheck)
                {
                    distanceCheck = Vector3.Distance(gameObject.transform.position, targets[i].transform.position);
                    currentlyClosestTarget = targets[i];
                }
            }
            closestTarget = currentlyClosestTarget;

            // Begin summary
            // If there are no target close by, do not extend lineRenderer and set material to disconnectedMaterial
            // End summary
            if (closestTarget == null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
                teleportActive = false;
                GetComponentInChildren<MeshRenderer>().material = disconnectedMat;
                return;
            }
            // Begin summary
            // If the closest target has this object as its closest target, extend lineRenderer and set material to connectedMaterial
            // End summary
            if (closestTarget.GetComponent<TeleporterObject>().ReturnCurrentTarget() == gameObject)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, closestTarget.transform.position);
                GetComponentInChildren<MeshRenderer>().material = connectedMat;
                teleportActive = true;
            }
            // Begin summary
            // If the closest target does not have this object as its closest target, do not extend lineRenderer and set material to disconnectedMaterial
            // End summary
            else
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
                GetComponentInChildren<MeshRenderer>().material = disconnectedMat;
                teleportActive = false;
            }
        }
        else
        {
            SetAvailableTeleportersList();
        }
    }

    // Begin summary
    // Get all teleporters from PuzzleManager and create list with all teleporters that are not part of the same room as this teleporter
    // End summary
    public void SetAvailableTeleportersList()
    {
        List<GameObject> allTeleporters = puzzleManager.GetAvailableTeleporters();
        for (int i = 0; i < allTeleporters.Count; i++)
        {
            if (roomID != allTeleporters[i].GetComponent<TeleporterObject>().ReturnRoomID())
            {
                targets.Add(allTeleporters[i]);
            }
        }
    }

    // Begin summary
    // Return current room ID to caller
    // End summary
    public int ReturnRoomID() {
            return roomID;
    }

    // Begin summary
    // Set roomID
    // End summary
    public void SetRoomID(int id)
    {
        roomID = id;
    }

    // Begin summary
    // Return current teleport target to caller
    // End summary
    public GameObject ReturnCurrentTarget()
    {
        return closestTarget;
    }

    // Begin summary
    // Return the target for teleportation if teleport is active
    // End summary
    public GameObject TeleportTargetIfActive()
    {
        if (teleportActive)
        {
            return closestTarget;
        }
        else {
            return null;
        }
    }

    // Begin summary
    // On trigger by player, send message to player object to move to connected teleport
    // End summary
    void OnTriggerEnter(Collider other) {
        if (teleportActive)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerObjectMovement>().MoveWithTeleport(closestTarget.transform);
                closestTarget.GetComponent<TeleporterObject>().StartCoroutine("TeleporterBeingUsed");
            }
        }
    }

    // Begin summary
    // When teleport has been used, set as inactive for a short period of time
    // End summary
    IEnumerator TeleporterBeingUsed() {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(.3f);
        boxCollider.enabled = true;
    }
}