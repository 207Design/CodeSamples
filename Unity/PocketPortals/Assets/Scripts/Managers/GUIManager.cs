using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	[SerializeField]
	LevelManager levelManager;

	// GUI Buttons
	[SerializeField]
	GameObject buttonStart;

	[SerializeField]
	GameObject buttonWinscreenRandom;

	[SerializeField]
	GameObject buttonWinscreenNext;

	[SerializeField]
	GameObject buttonOptions;

	[SerializeField]
	GameObject screenOptions;

    [SerializeField]
    GameObject screenGameOver;

    string currentState;

	// Use this for initialization
	void Awake () {
		ClearGUI ();
	}


	// Begin summary
	// Set game, win and options GUI to false
	// End summary
	void ClearGUI() {
		screenOptions.SetActive (false);
        SetMainGUI(false, false, false); 
	}


	// Begin summary
	// Display gui depending on state
	// End summary
	public void SetState(string state)
	{
		currentState = state;

        switch (state)
        {
            case "menu":
                SetMainGUI(true, false, true);
                break;
            case "game":
                SetMainGUI(false, false, true);
                screenOptions.SetActive(false);
                break;
            case "gameover":
                SetMainGUI(false, true, true);
                break;
            case "restart":
                SetMainGUI(false, false, false);
                screenOptions.SetActive(false);
                break;
            default:
                Debug.LogError("State for setting visible GUI does not exist");
                break;
        }
	}
		

	// Begin summary
	// Toggle the options window
	// End summary
	public bool ToggleOptions() {
		if (screenOptions.activeSelf == false) {
            SetMainGUI(false, false, true);
			screenOptions.SetActive (true);
			return true;
		} else {
			switch (currentState) {
			case "menu":
                    SetMainGUI(true, false, true);
				break;
			case "game":
                    SetMainGUI(false, false, true);
				break;
			case "gameover":
                    SetMainGUI(false, true, true);
				break;
			default:
				Debug.LogError ("State for setting visible GUI does not exist");
				break;
			}
			screenOptions.SetActive (false);
			return false;
		}
	}
		

	// Begin summary
	// Set the big three - main menu, win screen and options
	// End summary
	public void SetMainGUI (bool start, bool win, bool options) {
        buttonWinscreenRandom.GetComponent<Animator>().SetBool("enable", win);

        if (win == true) {
			bool nextLevelButton = levelManager.GetNextLevelAvailable ();
			buttonWinscreenNext.SetActive (nextLevelButton);
			buttonWinscreenNext.GetComponent<Animator>().SetBool("enable", nextLevelButton);
		} else {
			buttonWinscreenNext.GetComponent<Animator>().SetBool("enable", win);
			//buttonWinscreenNext.SetActive (false);
		}

		buttonOptions.SetActive (options);
		buttonStart.SetActive (start);
        screenGameOver.SetActive(win);

    }
}
