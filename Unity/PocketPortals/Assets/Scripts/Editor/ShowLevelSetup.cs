using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ShowLevelSetup {


	[MenuItem("Tools/LevelCreation/SpawnLevelSetup")]
	private static void showLevel() {
		if (GameObject.FindGameObjectWithTag ("Level") != null) {
			LevelObject levelObject = GameObject.FindGameObjectWithTag ("Level").GetComponent<LevelObject> ();
			levelObject.ConfigLevel ();
		} else {
			Debug.Log ("Level can not be located in scene");
		}
	}


}
