using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	GameObject[] levels;

	int currentPlayerPrefsLevel = 0;
	int currentLevel = 0; 
	string levelKey = "currentLevel";

	//The level object spawned
	GameObject spawnedLevel;

    void Start() {
        currentLevel = PlayerPrefs.GetInt(levelKey);
    }

    // Begin summary
    // Spawn selected level
    // End summary
    public void StartLevel()
    {
        if (spawnedLevel != null)
        {
            Destroy(spawnedLevel);
        }
        spawnedLevel = Instantiate(levels[currentLevel]);
    }

    // Begin summary
    // On level completion, add 1 to current level variable
    // End summary
    void LevelCompleted() {
		if (currentLevel + 1 < levels.Length) {
			PlayerPrefs.SetInt (levelKey, currentLevel + 1);
		}
	}

    // Begin summary
    // Set next level
    // End summary
    public void NextLevel()
    {
        currentLevel = PlayerPrefs.GetInt(levelKey);
    }

    // Begin summary
    // Set a random level
    // End summary
    public void RandomLevel()
    {
        currentPlayerPrefsLevel = PlayerPrefs.GetInt(levelKey);
        currentLevel = Random.Range(0, currentPlayerPrefsLevel += 1);
    }

    // Begin summary
    // Return if there are levels beyond the current level or if the final level has just been completed
    // End summary
    public bool GetNextLevelAvailable()
    {
        if (currentLevel + 1 < levels.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Begin summary
    // Reset stored player progression
    // End summary
    public void ResetProgression() {
		PlayerPrefs.SetInt (levelKey, 0);
        currentLevel = 0;
	}

    // Begin summary
    // Remove currently spawned level
    // End summary
    public void RemoveCurrentLevel() {
        if (spawnedLevel != null)
        {
            Destroy(spawnedLevel);
        }
    }

    // Begin summary
    // Increase progression by 1 if current level is the last unlocked level and if there are more levels to unlock
    // End summary
    public void IncreaseProgression() {
        if (currentLevel == PlayerPrefs.GetInt(levelKey) && currentLevel + 1 < levels.Length)
        {
            currentLevel += 1;
            PlayerPrefs.SetInt(levelKey, currentLevel);
        }
        else {
            Debug.Log("Last level reached");
        }
    }
}
