using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class SelectModel : MonoBehaviour {
    public bool isSelected=false;
    private Outline mOutline;
    public int index;
	void Start ()
    {
        mOutline = this.gameObject.GetComponentInChildren<Outline>();
        mOutline.enabled = false;
    }

	
	void Update ()
    {
        if (isSelected)
        {
            PlayerShow.Instance.curIndex = index;
            PlayerShow.Instance.ShowPrice();
            mOutline.enabled = true;
        }
        else
        {
            mOutline.enabled = false;
        }
	}
}
