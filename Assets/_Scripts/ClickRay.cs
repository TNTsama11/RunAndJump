using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRay : MonoBehaviour {
    private GameObject curObj;
	void Start () {
		
	}

	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point,new Color(255,0,0));
                GameObject obj = hitInfo.collider.gameObject;
                if (obj.tag=="Player") 
                {
                    PlayerShow.Instance.player = obj.gameObject;
                   Debug.Log(obj.gameObject.name);
                    PlayerShow.Instance.ClickPlayerShow();
                    if (PlayerShow.Instance.isShop) //如果打开了商店
                    {
                        if (!PlayerShow.Instance.isShopEnter)
                        {
                            //选定交互                        
                            if (curObj != obj)
                            {
                                if (curObj != null)
                                {
                                    curObj.GetComponentInChildren<SelectModel>().isSelected = false;
                                    obj.GetComponentInChildren<SelectModel>().isSelected = true;
                                    curObj = obj;
                                }
                                else
                                {
                                    curObj = obj;
                                    obj.GetComponentInChildren<SelectModel>().isSelected = true;
                                }
                            }
                            else
                            {
                                PlayerShow.Instance.ShowShopEnter();
                                PlayerShow.Instance.SelectModel();
                            }
                        }
                    }
                }
            }
        }
    }
}
