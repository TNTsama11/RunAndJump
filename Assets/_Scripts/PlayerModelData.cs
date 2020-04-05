using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 存储替换模型信息
/// </summary>
[System.Serializable]

public class PlayerModelData
{   
  public  GameObject modelPrefabs;
  public  int price;

    public GameObject ModelPrefabs
    {
        get { return modelPrefabs; }
        set { modelPrefabs = value; }
    }
    public int Price
    {
        get { return price; }
        set { price = value; }
    }

}
