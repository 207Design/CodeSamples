using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameManagerAnimatorTriggers : MonoBehaviour {

    [SerializeField]
    Animator animator;

    [SerializeField]
    int newState = 0;

    [SerializeField]
    bool fadeScreen = false;

    // Begin summary
    // Trigger animator with custom setting for which state is next and the fade should occur inbetween
    // End summary
    public void TriggerAnimator() {
        animator.SetBool("fadeScreen", fadeScreen);
        animator.SetInteger("newState", newState);
        animator.SetTrigger("goToState");
    }

}
