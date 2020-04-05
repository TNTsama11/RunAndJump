using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotate : MonoBehaviour {

    public float speed = 5;
	void Start ()
    {
		
	}
	
	
	void Update ()
    {
        this.gameObject.transform.Rotate(Vector3.up * speed * Time.deltaTime);
	}
}
