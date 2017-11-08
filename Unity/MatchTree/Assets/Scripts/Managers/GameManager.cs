using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMenu;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        BoardManager boardManager;

        [SerializeField]
        GridSizeInput gridSizeMenu;

        [SerializeField]
        List<GameObject> MenuStateObjects = new List<GameObject>();

        [SerializeField]
        List<GameObject> GameStateObjects = new List<GameObject>();

        [SerializeField]
        GameObject noAvailableSolution;

        [SerializeField]
        Vector2 maxGridSize = new Vector2(8, 6);

        public Vector2 MaxGridSize
        {
            get
            {
                return maxGridSize;
            }
        }

        [SerializeField]
        Vector2 minGridSize = new Vector2(4, 4);

        public Vector2 MinGridSize
        {
            get
            {
                return minGridSize;
            }
        }

        // Use this for initialization
        public void Start()
        {
            SetStateMenu();
        }

        // <summary>
        // Set grid size in board manager the same size defined in main menu
        // Tell board manager to initialize board
        // </summary>
        public void OnStartButtonPressed()
        {
            SetStateGame();
            Vector2 _gridSize = gridSizeMenu.GridSize;
            boardManager.GridSize = _gridSize;
            boardManager.InitializeGameBoard();
        }

        // <summary>
        // Tell board manager to destroy board, and then display main menu
        // </summary>
        public void OnMenuButtonPressed()
        {
            StartCoroutine(GoToMenuWhenReady());
        }

        // <summary>
        // Move to manu when game state has finished up
        // </summary>
        IEnumerator GoToMenuWhenReady()
        {
            while (!boardManager.CanSelectTiles)
            {
                yield return null;
            }
            boardManager.RemoveBoard();
            SetStateMenu();
        }

        // <summary>
        // Menu state is game
        // </summary>
        private void SetStateGame()
        {
            for (int i = 0; i < MenuStateObjects.Count; i++) {
                MenuStateObjects[i].SetActive(false);
            }
            for (int i = 0; i < GameStateObjects.Count; i++)
            {
                GameStateObjects[i].SetActive(true);
            }

            noAvailableSolution.SetActive(false);
        }

        // <summary>
        // Menu state is menu
        // </summary>
        private void SetStateMenu()
        {
            for (int i = 0; i < MenuStateObjects.Count; i++)
            {
                MenuStateObjects[i].SetActive(true);
            }
            for (int i = 0; i < GameStateObjects.Count; i++)
            {
                GameStateObjects[i].SetActive(false);
            }

            noAvailableSolution.SetActive(false);
        }

        // <summary>
        // Display GUI when no more tiles can be connected and removed
        // </summary
        public void SolutionNotAvailable()
        {
            noAvailableSolution.SetActive(true);
        }
    }
}