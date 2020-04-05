using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadManager : MonoBehaviour
{
    public bool isStartRoad = false;
    public GameObject roadGuidePrefab;
    public GameObject roadTemplatePrefab;
    public Text goldNumberText;
    public Text scoreText;
    public PlayerController playerController;
    GameObject roadGuide;
    Transform roadGuideTrans;
    int startRoadLength = 20;
    bool isBuildDirectRoad;
    int directRoadNumber = 10;
    int curDirectRoadType;
    int turnRoadLimit;//相邻转向道路之间的最小间隔
    int goldNumber;
    List<GameObject> roadList = new List<GameObject>(); //保存解体后的路块
    static RoadManager _instance;
    public static RoadManager Instance
    {
        get { return _instance; }
    }
    public int GoldNumber
    {
        get { return goldNumber; }
        set { goldNumber = value; }
    }

    List<GameObject> roadNumberList = new List<GameObject>();

    public List<GameObject> RoadNumberList
    {
        get { return roadNumberList; }
        set { roadNumberList = value; }
    }
    private GameObject tempRoad;
    public int roadName;
    public GameObject TempRoad
    {
        get { return tempRoad; }
        set { tempRoad = value; }
    }

    public void BuildTempRoad()
    {
        tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        tempRoad.name = "RoadTemplate" + roadName.ToString();
        RoadNumberList.Add(tempRoad);
        roadName += 1;
    }

    void Awake ()
    {
        _instance = this;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
    void Start()
    {
        roadGuide = Instantiate(roadGuidePrefab, Vector3.zero, Quaternion.identity);
        roadGuideTrans = roadGuide.transform;
        roadGuideTrans.name = "RoadGuide";
        for (int i=0;i<startRoadLength;++i)
        {
            isStartRoad = true;
            BuildTempRoad();
            //Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
            roadGuideTrans.position += Vector3.forward;
        }
        isStartRoad = false;
    }

    void Update ()
    {
        goldNumberText.text = goldNumber.ToString();
        int score = (int)playerController.Distance;
        scoreText.text = score.ToString();
	}
    public void BuildGeneralRoad()//直路
    {
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
        ShowSubRoadBlockBreakupEffect(tempRoad);
        ShowSubRoadBlockCombinationEffect(tempRoad);
    }
    public void BuildRoad()
    {
        //BuildGeneralRoad();
        if (isBuildDirectRoad && directRoadNumber > 0)
        {
            switch (curDirectRoadType)
            {
                case (int)DirectRoadType.Up:
                    BuildUpRoad();
                    directRoadNumber--;
                    break;
                case (int)DirectRoadType.Down:
                    BuildDownRoad();
                    directRoadNumber--;
                    break;
                case (int)DirectRoadType.Left:
                    BuildLeftRoad();
                    directRoadNumber--;
                    break;
                case (int)DirectRoadType.Right:
                    BuildRightRoad();
                    directRoadNumber--;
                    break;
            }
            if (directRoadNumber <= 0) //偏移道路是否完成
            {
                isBuildDirectRoad = false;
            }
        }    
        else //构建非偏移
        {
             int index = Random.Range(1, 11);           
            if (index == (int)RoadType.Direct)
            {
                isBuildDirectRoad = true;
                directRoadNumber = 10;
                int directRoadType = Random.Range(0, 4);
                curDirectRoadType = directRoadType;
                //switch (directRoadType)
                //{
                //    case (int)DirectRoadType.Up:
                //        BuildUpRoad();
                //        break;
                //    case (int)DirectRoadType.Down:
                //        BuildDownRoad();
                //        directRoadNumber--;
                //        break;
                //    case (int)DirectRoadType.Left:
                //        BuildLeftRoad();
                //        directRoadNumber--;
                //        break;
                //    case (int)DirectRoadType.Right:
                //        BuildRightRoad();
                //        directRoadNumber--;
                //        break;
                //}
            }
            else if (index==(int)RoadType.Swerve&&turnRoadLimit<=0) //转向道路
            {
                turnRoadLimit = 10;
                int swerveRoadType = Random.Range(1, 3);
                switch (swerveRoadType)
                {
                    case (int)SwerveRoadType.TurnLeft:
                        BuildTurnLeftRoad();
                        break;
                    case (int)SwerveRoadType.TurnRight:
                        BuildTurnRightRoad();
                        break;
                }              
            }
            else if (index==(int)RoadType.Trap)
            {
                BuildTrapRoad();
            }
           BuildGeneralRoad();
            turnRoadLimit--;
        }
    }

    public enum RoadType //道路方向类型
    {
        Direct=1,
        Swerve,
        Trap,
    }
    public enum DirectRoadType //偏移的类型
    {
        Up,
        Down,
        Left,
        Right,
    }
    public enum SwerveRoadType //转向道路的类型
    {
        TurnLeft=1,
        TurnRight,
    }
    public enum TrapRoadType //陷阱道路类型
    {
        Left = 1,
        Center,
        Right,
    }


    public void BuildUpRoad() //上移道路
    {
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
        roadGuideTrans.position += roadGuideTrans.up * 0.2f;
    }
    public void BuildDownRoad() //下移道路
    {
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
        roadGuideTrans.position -= roadGuideTrans.up * 0.2f;
    }
    public void BuildLeftRoad() //左移道路
    {
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
        roadGuideTrans.position -= roadGuideTrans.right * 0.2f;
    }
    public void BuildRightRoad() //右移道路
    {
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
        roadGuideTrans.position += roadGuideTrans.right * 0.2f;
    }
    public void BuildTurnLeftRoad()
    {
        for(int i = 0; i < 3; i++)
        {
            BuildTempRoad();
            //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
            roadGuideTrans.position += roadGuideTrans.forward;
        }
        roadGuideTrans.position -= roadGuideTrans.forward * 2f;
        roadGuideTrans.Rotate(Vector3.up, -90f);
        roadGuideTrans.position += roadGuideTrans.forward * 2f;
        BuildTempRoad();
        //Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
    }
    public void BuildTurnRightRoad()
    {
        for (int i = 0; i < 3; i++)
        {
            BuildTempRoad();
            //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
            roadGuideTrans.position += roadGuideTrans.forward;        
        }
        roadGuideTrans.position -= roadGuideTrans.forward * 2f;
        roadGuideTrans.Rotate(Vector3.up, 90f);
        roadGuideTrans.position += roadGuideTrans.forward * 2f;
        BuildTempRoad();
        //Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        roadGuideTrans.position += roadGuideTrans.forward;
    }
    public void BuildTrapRoad()
    {
        roadGuideTrans.position += roadGuideTrans.forward;
        BuildTempRoad();
        //GameObject tempRoad = Instantiate(roadTemplatePrefab, roadGuideTrans.position, roadGuideTrans.rotation);
        tempRoad.transform.Rotate(Vector3.up, 90f);
        int index = Random.Range(1, 4);
        switch (index)
        {
            case (int)TrapRoadType.Left:
                tempRoad.transform.position -= tempRoad.transform.forward;
                break;
            case (int)TrapRoadType.Center:
                break;
            case (int)TrapRoadType.Right:
                tempRoad.transform.position += tempRoad.transform.forward;
                break;
        }
        roadGuideTrans.position += roadGuideTrans.forward * 2f;
    }
    public void ShowSubRoadBlockBreakupEffect(GameObject parentRoad)
    {
        RoadTemplate tempRoad = parentRoad.GetComponent<RoadTemplate>();
        if (null != tempRoad)
        {
            tempRoad.SetSubRoadBreakupEffect();
            roadList.Add(parentRoad);
        }
    }
    public void ShowSubRoadBlockCombinationEffect(GameObject parentRoad)
    {
        RoadTemplate tempRoad = roadList[0].GetComponent<RoadTemplate>();
        if (null != tempRoad)
        {
            tempRoad.SetSubRoadCombinationEffect();
        }
        roadList.RemoveAt(0);
    }
}
