using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    bool isTiming = false;
    float countDown;
    public AudioClip deathClip;
    public AudioClip jumoClip;
    float BGMVolume;
    static PlayerController _instance;
    public CharacterController playerController;
    public Animator playerAnimator;
    float runSpeed = 1f;
    float runSpeedDelta = 0.1f;
    float maxRunSoeed = 8f;
    float transverseSpeed = 2f;
    float jumpPower = 2f;
    float dropSpeed = -3f;
    public bool isJumpState=false;
    public bool isSlideState = false;
    bool isTurnLeftEnd = true;
    bool isTurnRightEnd = true;
    public bool isDeath=false;
    float createRoadLength;
    float distance; //跑过的距离
    float curRoadHeight; //脚下路面高度
    public Color color;
    Vector3 moveIncrement;
    GameObject currentRoad;
    public GameObject[] playerModels;
    public float Distance
    {
        get { return distance; }
    }

    public static PlayerController Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        color = new Color(1, 1, 1);
        _instance = this;
    }
    void Start ()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
            this.GetComponent<AudioSource>().volume = BGMVolume;
        }
        else
        {
            BGMVolume = 1f;
        }
      GameObject tempObj = Instantiate(playerModels[PlayerPrefs.GetInt("PlayerSelectModel")], transform.position, transform.rotation);
       tempObj.transform.parent = transform;
        playerAnimator = this.transform.GetChild(0).GetComponent<Animator>();        
    }

	void Update ()
    {


        ChangeColor();

        if (runSpeed < maxRunSoeed)
        {
            runSpeed += runSpeedDelta * Time.deltaTime;
        }
        //Debug.Log(runSpeed);
        moveIncrement = transform.forward * runSpeed * Time.deltaTime;
        if (isJumpState) //跳跃
        {
            moveIncrement.y= jumpPower * Time.deltaTime;
            Debug.Log(moveIncrement.y);
        }
        else
        {
            moveIncrement.y = playerController.isGrounded ? 0f : dropSpeed * Time.deltaTime;
        }
        //Debug.Log(playerController.velocity.magnitude);

        // moveIncrement = transform.forward * runSpeed * Time.deltaTime;//计算z轴移动
#if UNITY_STANDALONE_WIN
        float moveDir = Input.GetAxis("Horizontal");
        bool startTurnLeft = Input.GetKeyDown(KeyCode.Q);
        bool startTurnRight = Input.GetKeyDown(KeyCode.E);
        bool startSlide = Input.GetKeyDown(KeyCode.LeftControl);
        bool startJump = Input.GetKey(KeyCode.Space);
#elif UNITY_ANDROID
        float moveDir=Input.acceleration.x*2f;    
        bool startTurnLeft = (TouchMove() == TouchDir.Left);
        bool startTurnRight = (TouchMove()==TouchDir.Right);
        bool startJump = (TouchMove() ==TouchDir.Up);
        bool startSlide = (TouchMove() == TouchDir.Down);

#endif
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitDetection();
        }

        if (startJump && playerController.isGrounded && !isJumpState && !isSlideState)
        {
            isJumpState = true;
            playerAnimator.SetBool("isJump", true);
            AudioSource.PlayClipAtPoint(jumoClip, this.transform.position, 1f);
        }
        if (startSlide && !isSlideState && !isJumpState)
        {
            playerAnimator.SetBool("isSlide", true);
            isSlideState = true;
        }

        if (startTurnLeft && isTurnLeftEnd && isTurnRightEnd&&!isDeath)
        {
            StartTurnLeft();
        }
        else if (startTurnRight && isTurnRightEnd && isTurnLeftEnd && !isDeath)
        {
            StartTurnRight();
        }
        moveIncrement += transform.right * moveDir * transverseSpeed * Time.deltaTime;     //左右水平移动
        //moveIncrement.y = playerController.isGrounded ? 0f : dropSpeed * Time.deltaTime;

        if (!isDeath)
        {
            playerController.Move(moveIncrement);
        }
        playerAnimator.SetFloat("Speed", playerController.velocity.magnitude,20f,Time.deltaTime);
        createRoadLength += runSpeed * Time.deltaTime;
        if (createRoadLength > 1f&&!isDeath)
        {

            RoadManager.Instance.BuildRoad();  //构建道路
            createRoadLength -= 1f;
        }
        if (!isDeath)
        {
            distance += runSpeed * Time.deltaTime; //计算跑过的距离
        }        
        if (transform.position.y < curRoadHeight-2f)
        {
            isDeath = true;

        }
        if (isDeath)
        {
            Death();
        }
    }
    bool isDeathAudio=false;
    void Death()
    {
        if (!isDeathAudio)
        {
            AudioSource.PlayClipAtPoint(deathClip, transform.position, 1f);
            isDeathAudio = true;
        }
        playerAnimator.SetBool("isDeath", true);
        playerController.enabled = false;
        PlayerPrefs.SetInt("Score", RoadManager.Instance.GoldNumber * 5 + (int)distance);
        if (!PlayerPrefs.HasKey("BestScore")) //如果最高分不存在
        {
            PlayerPrefs.SetInt("BestScore", RoadManager.Instance.GoldNumber * 5 + (int)distance);
        }
        else
        {
            int bestScore = PlayerPrefs.GetInt("BestScore");
            int currentScore = RoadManager.Instance.GoldNumber * 5 + (int)distance;
            if (currentScore > bestScore) //比较当前分和最高分
            {
                PlayerPrefs.SetInt("BestScore", currentScore);
            }
        }
        UIManager.Instance.ShowGameOver();
        UIManager.Instance.ShowScore();
        runSpeed = 0f;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject != currentRoad)
        {
            currentRoad = hit.gameObject;
            curRoadHeight = hit.gameObject.transform.position.y; //当前脚下方块高度
            //Destroy(hit.gameObject, 1f);
            int a = FindIndex(hit.gameObject);
            for (int i = 0; i < a; i++)
            {
                Destroy(RoadManager.Instance.RoadNumberList[i], 2f);
                RoadManager.Instance.RoadNumberList.RemoveAt(i);
            }
        }
    }

    public int FindIndex(GameObject hitGame)
    {
        for (int i = 0; i < RoadManager.Instance.RoadNumberList.Count; i++)
        {
            if (RoadManager.Instance.RoadNumberList[i] == hitGame.gameObject)
            {
                return i;
            }
        }
        return 0;
    }

    //void JumpEnd()
    //{        
    //    isJumpState = false;
    //    playerAnimator.SetBool("isJump", false);
    //}
    //void SlideEnd()
    //{
    //    playerAnimator.SetBool("isSlide", false);
    //}
    void StartTurnLeft()
    {
        isTurnLeftEnd = false;
        transform.Rotate(Vector3.up, -90f);
        Quaternion endValue = transform.rotation;
        transform.Rotate(Vector3.up, 90f);
        Tween tween = transform.DORotateQuaternion(endValue, 0.3f);
        tween.OnComplete(TurnLeftEnd);
    }
    void TurnLeftEnd()
    {
        isTurnLeftEnd = true;
       // Debug.Log("L1");
    }
    void StartTurnRight()
    {
        isTurnRightEnd = false;
        transform.Rotate(Vector3.up, 90f);
        Quaternion endValue = transform.rotation;
        transform.Rotate(Vector3.up, -90f);
        Tween tween = transform.DORotateQuaternion(endValue, 0.3f);
        tween.OnComplete(TurnRightEnd);
    }
    void TurnRightEnd()
    {
        isTurnRightEnd = true;
    }

    public enum TouchDir
    {
        Null = 0,
        Left = 1,
        Right,
        Up,
        Down,
    }

    TouchDir CheckTouchDir(Vector2 beginPointPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Vector2 endPointPosition = Input.GetTouch(0).position;
            Vector2 offset = endPointPosition - beginPointPosition;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x) > 200f)
            {
                if (offset.x > 0)//向右
                {
                    return TouchDir.Right;
                }
                else //向左滑动
                {
                    return TouchDir.Left;
                }
            }
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x) && Mathf.Abs(offset.y) > 200f)
            {
                //向上滑动
                if (offset.y > 0)
                {
                    return TouchDir.Up;
                }
                else//向下滑动
                {
                    return TouchDir.Down;
                }
            }
            if (Mathf.Abs(offset.x) < 50f && Mathf.Abs(offset.y) < 50f)
            {
                return TouchDir.Null;
            }
        }
        return TouchDir.Null;
    }

    Vector2 beginPointPositon = Vector2.zero;
    TouchDir TouchMove()
    {
        if (Input.touchCount>0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                beginPointPositon = Input.GetTouch(0).position; //保存滑动起点
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                return CheckTouchDir(beginPointPositon);
            }
        }
       
        return TouchDir.Null;
    }

    void ExitDetection()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
#elif UNITY_ANDROID
        if (countDown == 0f)
            {
                countDown = Time.time;
                isTiming = true;
                ShowToast("再点就退出了哦");
            }
            else
            {
                Application.Quit();
            }
            if (isTiming)
            {
                if ((Time.time - countDown) > 2f)
                {
                    countDown = 0f;
                    isTiming = false;
                }
            }
#endif
    }

    void ShowToast(string text)
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"); ;
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
         AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", text.ToString());
        Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT")).Call("show");
        }
        ));
    }

    float colorSpeed = 0.05f;
    int c = 0;
    private void ChangeColor()
    {
        int score = (int)distance;
        if (color.r < 0f)
        {
            color.r = 0f;
        }
        if (color.g < 0f)
        {
            color.g = 0f;
        }
        if (color.b < 0f)
        {
            color.b = 0f;
        }
        if (color.r > 1)
        {
            color.r = 1;
        }
        if (color.g > 1)
        {
            color.g = 1;
        }
        if (color.b > 1)
        {
            color.b= 1;
        }
        if (color.r >= 0 && color.g >= 0 && color.b >= 0 && color.r <= 1 && color.g <= 1 && color.b <= 1)
        {
            if (score-c <= 200)
            {
                color.r -= colorSpeed * Time.deltaTime;
                color.b -= colorSpeed * Time.deltaTime;
            }
            else if (score -c<= 600)
            {
                color.r += colorSpeed * Time.deltaTime;
                color.g -= colorSpeed * Time.deltaTime;
                color.b = 0f;
            }
            else if (score-c <= 1500)
            {
                color.r -= colorSpeed * Time.deltaTime;
                color.g -= colorSpeed * Time.deltaTime;
                color.b += colorSpeed * Time.deltaTime;
            }
            else if (score -c> 2000)
            {
                color.r = 0f;
                color.g = 0f;
                color.b -= colorSpeed * Time.deltaTime;
            }
            else if (score - c > 2500)
            {
                c = score;
            }
        }
    }



}
