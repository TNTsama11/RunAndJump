using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float distanceAway = 1f;
    public float distanceUp =1f;
    public float smooth = 2f;
    private Vector3 targetPosition;
    private Transform target;
    void Awake ()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

	void Update ()
    {
        targetPosition =  target.position+Vector3.up *distanceUp - target.forward * distanceAway;
       // Debug.Log(targetPosition);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
        Vector3 tempPosition = target.position;       
        tempPosition += Vector3.up * 1f;
       
        //Debug.Log(tempPosition);
        transform.LookAt(tempPosition);

    }
}
