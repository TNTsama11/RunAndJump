using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStop2 : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
		
	}

    void JumpEnd()
    {
        PlayerController.Instance.isJumpState = false;
     PlayerController.Instance.playerAnimator.SetBool("isJump", false);
    }
    void SlideEnd()
    {
        PlayerController.Instance.isSlideState = false;
        PlayerController.Instance.playerAnimator.SetBool("isSlide", false);
    }
}
