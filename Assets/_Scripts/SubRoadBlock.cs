using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SubRoadBlock : MonoBehaviour
{

    bool isRotate = false;
    float resetTime = 2f;
    public bool IsRotate
    {
        get { return isRotate; }
        set { isRotate = value; }
    }
    
    Vector3 originPosition;
    Quaternion orginRotation;

    void Awake()
    {

        this.GetComponent<Renderer>().material.color = PlayerController.Instance.color;
        originPosition = transform.localPosition;
        orginRotation = transform.localRotation;
        CreateCoin(); //生成金币
        if (!RoadManager.Instance.isStartRoad)
        {
            CreateObstacle();
        }
        
    }

	void Start ()
    {
        
    }

	void Update ()
    {
        if (isRotate)
        {
            transform.RotateAround(transform.parent.position, transform.parent.forward, 30f * Time.deltaTime);
        }
	}

    public void ChangePosition()
    {
        int xChangeValue = 1;
        while (Mathf.Abs(xChangeValue) <= 4)
        {
            xChangeValue = Random.Range(-10, 10);
        }
        int yChangeValue = 1;
        while (Mathf.Abs(yChangeValue) <= 4)
        {
            yChangeValue = Random.Range(10 ,-10);
        }
        transform.position += transform.right * xChangeValue;
        transform.position += transform.up * yChangeValue;
    }
    public void ChangeRotation()
    {
        transform.Rotate(transform.right, Random.Range(-180f, 180f));
        transform.Rotate(transform.up, Random.Range(-180f, 180f));
        transform.Rotate(transform.forward, Random.Range(-180f, 180f));
    }
    public void RestRoad()
    {
        transform.DOLocalMove(originPosition, resetTime).SetEase(Ease.InOutElastic);
         Tween tween=  transform.DORotateQuaternion(orginRotation, resetTime);
        isRotate = false;
        //tween.OnComplete(CreateCoin);
    }
    public void CreateCoin()
    {
        bool isCreateCoin = (Random.Range(0, 20) == 10 ? true:false);
        if (isCreateCoin)
        {
            GameObject coin = Resources.Load("Gold") as GameObject;
            Vector3 coinPosition = transform.position + transform.up * Random.Range(1.4f,3f);
            Instantiate(coin, coinPosition, Quaternion.identity, transform);
        }
    }
    public void CreateObstacle() //生成障碍
    {
        bool isCreateObstacle = (Random.Range(0, 100) == 10 ? true : false);
        if (isCreateObstacle)
        {
            GameObject obstacle = Resources.Load("Crystals") as GameObject;
            Vector3 obstaclePosition = transform.position + transform.up * 1.3f;
            Instantiate(obstacle, obstaclePosition, Quaternion.identity, transform);
        }       
    }
}
