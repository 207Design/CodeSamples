using UnityEngine;
using Manager;

namespace GameInput
{
    public class BoardTileInput : MonoBehaviour
    {
        [SerializeField]
        BoardManager boardManager;

        RaycastHit hit;

        // Update is called once per frame
        void Update()
        {
            if (Application.isEditor)
            {
                // If in editor with mouse
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.tag == "PlayableTile")
                        {
                            boardManager.SelectTile(hit.collider.gameObject);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    boardManager.CheckSelectedTiles();
                }
            }
            else
            {
                // On TouchPhase Begin
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject.tag == "PlayableTile")
                            {
                                boardManager.SelectTile(hit.collider.gameObject);
                            }
                        }
                    }
                    // On TouchPhase Moved
                    else if (Input.GetTouch(i).phase == TouchPhase.Moved)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.gameObject.tag == "PlayableTile")
                            {
                                boardManager.SelectTile(hit.collider.gameObject);
                            }
                        }
                    }
                    // On TouchPhase Ended
                    else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        if (Input.touchCount <= 1) {
                            boardManager.CheckSelectedTiles();
                        }
                    }
                }
            }
        }
    }
}