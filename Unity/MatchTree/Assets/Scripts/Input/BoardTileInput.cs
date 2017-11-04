using UnityEngine;
using Manager;

namespace GameInput
{
    public class BoardTileInput : MonoBehaviour
    {
        [SerializeField]
        BoardManager boardManager;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    if (Input.touchCount <= 1)
                    {
                        boardManager.CheckSelectedTiles();
                        return;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                boardManager.CheckSelectedTiles();
            }
        }
    }
}