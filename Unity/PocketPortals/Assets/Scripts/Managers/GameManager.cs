using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    Animator animator;

    [SerializeField]
	GUIManager guiManager;

	[SerializeField]
	LevelManager levelManager;

	[SerializeField]
	PuzzleManager puzzleManager;

    string nextState = "menu";

	// Use this for initialization
	void Start () {
        GoToState(nextState);
	}

    public void GoToState(string state) {
        switch (state)
        {
            case "menu":
                StateMenu();
                break;
            case "game":
                StateGame();
                break;
            case "gameover":
                StateGameOver();
                break;
            case "options":
                StateOptions();
                break;
            case "clearGame":
                ClearGame();
                break;
        }
    }
    // Begin summary
    // Intitalize the game menu
    // End summary
    void StateMenu()
	{
		guiManager.SetState("menu");
		Debug.Log ("Set state to 'Main menu'");
	}

	// Begin summary
	// Intitalize the game state
	// End summary
	void StateGame ()
	{
		guiManager.SetState("game");
		levelManager.StartLevel ();
		Debug.Log ("Set state to 'Game'");
	}

	// Begin summary
	// Intitalize the game over menu
	// End summary
	void StateGameOver()
	{
		guiManager.SetState("gameover");
        levelManager.IncreaseProgression();
		Debug.Log ("Set state to 'GameOver'");
	}

	// Begin summary
	// Intitalize the options menu
	// End summary
	void StateOptions()
	{
		bool optionsState = guiManager.ToggleOptions ();
		if (optionsState == true) {
		} else {
			
		}
	}

    // Begin summary
    // Clear game by removing level and player object assets
    // End summary
    void ClearGame() {
        puzzleManager.ResetPuzzleManager();
        levelManager.RemoveCurrentLevel();
    }

    // Begin summary
    // Control the game manager animator state machine
    // End summary
    public void ControlGameManagerAnimator(int newState, bool fade) {
        animator.SetBool("fadeScreen", fade);
        animator.SetInteger("newState", newState);
        animator.SetTrigger("goToState");
    }
}
