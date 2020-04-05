using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCollision : MonoBehaviour {
    public AudioClip coin;
	void Start ()
    {
		
	}

	void Update ()
    {
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
        transform.Rotate(Vector3.forward, 90f * Time.deltaTime);
        transform.Rotate(Vector3.left, 90f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RoadManager.Instance.GoldNumber++;
            AudioSource.PlayClipAtPoint(coin, this.transform.position,1f);
            Destroy(this.gameObject);
        }
    }


}
