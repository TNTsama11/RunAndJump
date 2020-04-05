using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clud : MonoBehaviour {
    public Sprite cludeTexture;
    public Text againText;
	void Start ()
    {
        if (PlayerPrefs.HasKey("Again"))
        {
            if (PlayerPrefs.GetInt("Again")>=3)  
            {
                this.GetComponent<Image>().sprite = cludeTexture;
                againText.gameObject.SetActive(true);
            }
            else
            {
                againText.gameObject.SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Again", 0);
        }
	}
	

	void Update ()
    {
		
	}
}
