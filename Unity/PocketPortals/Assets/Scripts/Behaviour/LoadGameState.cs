using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameState : StateMachineBehaviour {

    GameManager gameManager;

    [SerializeField]
    string newState;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        gameManager.GoToState(newState);
	}
}
