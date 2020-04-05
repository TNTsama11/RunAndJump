using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Crystals : MonoBehaviour {
    public AudioClip breakSound;
    bool isDes=false;
	void Start ()
    {
		
	}
	
	void Update ()
    {
       
	}
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            if (PlayerController.Instance.isSlideState)
            {
                isDes = true;
                other.transform.position = this.transform.position - Vector3.up * 0.5f;
                AudioSource.PlayClipAtPoint(breakSound, transform.position, 1f);
                 Tween tween = this.transform.DOLocalMove(other.transform.forward * 2f, 0.5f);
                  tween.OnComplete(Des);
            }
            else
            {
                other.transform.position = this.transform.position-Vector3.up*0.5f;
                if (!isDes)
                {
                    PlayerController.Instance.isDeath = true;
                }
                //Tween tween = this.transform.DOLocalMove(other.transform.forward*5f, 1f);
            }
        }
    }
    void Des()
    {
        RoadManager.Instance.GoldNumber += 5;
        isDes = false;
        Destroy(this.gameObject);       
    }
}
