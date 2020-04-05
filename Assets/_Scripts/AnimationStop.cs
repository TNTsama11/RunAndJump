using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Wati01End()
    {
       PlayerShow.Instance.player.GetComponent<Animator>().SetBool("isWait01", false);
        PlayerShow.Instance.isShowing = false;
    }
    void Wati02End()
    {
        PlayerShow.Instance.player.GetComponent<Animator>().SetBool("isWait02", false);
        PlayerShow.Instance.isShowing = false;
    }
    void Wati03End()
    {
        PlayerShow.Instance.player.GetComponent<Animator>().SetBool("isWait03", false);
        PlayerShow.Instance.isShowing = false;
    }
    void Wati04End()
    {
        PlayerShow.Instance.player.GetComponent<Animator>().SetBool("isWait04", false);
        PlayerShow.Instance.isShowing = false;
    }
}
