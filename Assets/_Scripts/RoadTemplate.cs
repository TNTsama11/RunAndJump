using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTemplate : MonoBehaviour
{
    SubRoadBlock[] subRoadBlocks;

    void Awake()
    {
        subRoadBlocks = GetComponentsInChildren<SubRoadBlock>();
    }

	void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public void SetSubRoadBreakupEffect()
    {
        for(int i = 0; i < 3; i++)
        {
            subRoadBlocks[i].ChangePosition();
            subRoadBlocks[i].ChangeRotation();
            subRoadBlocks[i].IsRotate = true;
        }
    }
    public void SetSubRoadCombinationEffect() //设置组合效果
    {
        for(int i = 0; i < 3; i++)
        {
            subRoadBlocks[i].RestRoad();
        }
    }
    

}
