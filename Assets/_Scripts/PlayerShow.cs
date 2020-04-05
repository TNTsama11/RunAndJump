using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerShow : MonoBehaviour
{
    public GameObject playerModel;
    public bool isShopEnter = false;
    public GameObject shopEnter;
    public Text payText;
    public Button yesButton;
    public Button noButton;
    private int curPrice;
    public int curIndex;
    public Text PriceText;
    public Text moneyText;
    private int playerMoney;
    public Button startButton;
    public Button settingButton;
    public Button shopButton;
    private Animator playerAnimator;
    public AudioClip wait01Audio;
    public AudioClip wait02Audio;
    public AudioClip wait03Audio;
    public AudioClip wait04Audio;
    public Button exitButton;
    public GameObject sittingPanle;
    public Text soundValueText;
    public Slider soundValueSlider;
    public GameObject mainPanle;
    private AudioSource playerAudio;
    public GameObject player;
    public Button closeButton;
    public AudioSource bgmAduioSource;
    public Transform[] CP;
    public GameObject shopPanle;
    public Button backButton;
    public bool isShop = false;
    public int modelIndex;
    static PlayerShow _instance;
    public PlayerModelData[] playerModerlData;
    bool isClickExitBtn = false;
    bool isTiming=false;
    float countDown;
    public static PlayerShow Instance
    {
        get { return _instance; }
    }

    void Awake ()
    {
        
        _instance = this;
        GetPlayerModel();
        ShowModel();
        //player = GameObject.FindGameObjectWithTag("Player");       
        closeButton.onClick.AddListener(CloseSitting);
         startButton.onClick.AddListener(ChangeScene);
        settingButton.onClick.AddListener(OpenSettings);
        shopButton.onClick.AddListener(OpenShop);
        exitButton.onClick.AddListener(ClickExitBtn);
        backButton.onClick.AddListener(CloseShop);
        yesButton.onClick.AddListener(BuySomething);
        noButton.onClick.AddListener(CloseShopEnter);
        //playerAnimator = player.GetComponent<Animator>();
        //playerAudio =player.GetComponent<AudioSource>();
       GetMoney();
        if (!PlayerPrefs.HasKey("PlayerSelectModel"))
        {
            PlayerPrefs.SetInt("PlayerSelectModel", 0);
        }

    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerPrefs.SetInt("Money", 1000);
            GetMoney();
        }

        ExitDetection();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isClickExitBtn = true;
        }
    }
 public   bool isShowing = false;
    private enum ShowType
    {
        Wait01=1,
        Wait02,
        Wait03,
        Wait04
    }
    
 public   void ClickPlayerShow()
    {
        playerAnimator = player.GetComponent<Animator>();
        playerAudio =player.GetComponent<AudioSource>();
        if (!isShowing&&!isShopEnter)
        {
            isShowing = true;
            int index = Random.Range(1, 5);
            switch (index)
            {
                case (int)ShowType.Wait01:
                    //playerAnimator.SetBool("isWait01", true);
                    playerAnimator.SetTrigger("isWait01");
                    playerAudio.clip = wait01Audio;
                    playerAudio.Play();
                    break;
                case (int)ShowType.Wait02:
                    //playerAnimator.SetBool("isWait02", true);
                    playerAnimator.SetTrigger("isWait02");
                    playerAudio.clip = wait02Audio;
                    playerAudio.Play();
                   // isShowing = false;
                    break;
                case (int)ShowType.Wait03:
                    // playerAnimator.SetBool("isWait03", true);
                    playerAnimator.SetTrigger("isWait03");
                    playerAudio.clip = wait03Audio;
                    playerAudio.Play();
                    break;
                case (int)ShowType.Wait04:
                    //playerAnimator.SetBool("isWait04", true);
                    playerAnimator.SetTrigger("isWait04");
                    playerAudio.clip = wait04Audio;
                    playerAudio.Play();
                    break;
                default:
                    break;
            }
        }
    }
    //void Wati01End()
    //{
    //    playerAnimator.SetBool("isWait01", false);
    //    isShowing = false;
    //}
    //void Wati02End()
    //{
    //    playerAnimator.SetBool("isWait02", false);
    //    isShowing = false;
    //}
    //void Wati03End()
    //{
    //    playerAnimator.SetBool("isWait03", false);
    //    isShowing = false;       
    //}
    //void Wati04End()
    //{
    //    playerAnimator.SetBool("isWait04", false);
    //    isShowing = false;
    //}

    public   void ChangeScene() //切换场景
    {
        Globe.nextSceneName = "Game02";
        SceneManager.LoadScene("Game01");
    }

    public void OpenSettings() //设置面板
    {
        MainPanleHide();
        sittingPanle.GetComponent<RectTransform>().DOScaleY(1, 0.5f).SetEase(Ease.InExpo);
    }

     void OpenShop() //商店面板
    {
        MainPanleHide();
        Camera.main.transform.DOMove(CP[1].transform.position, 1f).SetEase(Ease.InOutQuad);
        Quaternion endValue = CP[1].transform.rotation;
    Tween tween= Camera.main.transform.DORotateQuaternion(endValue, 1f).SetEase(Ease.InOutQuad);
        tween.OnComplete(ShowShopPanle);
    }
     void CloseShop()
    {
        HideShopPanle();
       Camera.main.transform.DOMove(CP[0].transform.position, 1f).SetEase(Ease.InOutQuad);
        Quaternion endValue = CP[0].transform.rotation;
        Tween tween = Camera.main.transform.DORotateQuaternion(endValue, 1f).SetEase(Ease.InOutQuad);
        tween.OnComplete(MainPanleShow);
    }

    void HideShopPanle()
    {
        shopPanle.SetActive(false);
        isShop = false;
    }
    void ShowShopPanle()
    {
        shopPanle.SetActive(true);
        isShop = true;
    }
    void ClickExitBtn() //退出
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
#elif UNITY_ANDROID
        isClickExitBtn = true;
#endif
        //Application.Quit();

    }
    void ExitDetection()
    {

        if (isClickExitBtn)
        {
            if (countDown==0f)
            {
                countDown = Time.time;
                isTiming = true;
                isClickExitBtn = false;
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
        }
       
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

  public void SoundValue()
    {
        bgmAduioSource.volume = soundValueSlider.value;
        PlayerPrefs.SetFloat("BGMVolume", soundValueSlider.value);
        soundValueText.text = ((int)(soundValueSlider.value*100)).ToString() + "%";
    }

    void CloseSitting()
    {
      Tween tween=  sittingPanle.GetComponent<RectTransform>().DOScaleY(0, 0.5f).SetEase(Ease.InOutFlash);
        tween.OnComplete(MainPanleShow);        
    }
    void MainPanleShow()
    {
        mainPanle.SetActive(true);
    }
   void MainPanleHide()
    {
        mainPanle.SetActive(false);
    }
    void GetMoney()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            playerMoney = PlayerPrefs.GetInt("Money");
            moneyText.text = playerMoney.ToString();
        }
        else
        {
            playerMoney = 0;
            PlayerPrefs.SetInt("Money", playerMoney);
            moneyText.text = playerMoney.ToString();
        }
    }


   public void ShowPrice() //显示价格
    {
        curPrice = playerModerlData[curIndex].Price;
        PriceText.text = curPrice.ToString();
        if (playerModerlData[curIndex].Price>playerMoney)
        {
            PriceText.color = Color.red;
        }
        else
        {
            PriceText.color = Color.green;
        }
    }
    public void ShowShopEnter()
    {
        if (playerMoney >= curPrice&& curPrice != 0) //玩家钱大于价格
        {
            isShopEnter = true;
            shopEnter.SetActive(true);
            payText.text = "Pay " + curPrice.ToString();
        }
    }
    public void SelectModel()
    {
        if (curPrice==0) //价格为0的就是买过的
        {
         GameObject tempObj= playerModel.transform.GetChild(0).gameObject;
            Destroy(tempObj);
          tempObj= Instantiate(playerModerlData[curIndex].ModelPrefabs, playerModel.transform.position, playerModel.transform.rotation);
            tempObj.transform.parent = playerModel.transform;
                PlayerPrefs.SetInt("PlayerSelectModel",curIndex);
            //Debug.Log(tempObj.name);
        }
    }

    void ShowModel() //每次启动刷新看板人物
    {
        if (PlayerPrefs.HasKey("PlayerSelectModel"))
        {
            GameObject tempObj = playerModel.transform.GetChild(0).gameObject;
            Destroy(tempObj);
            curIndex = PlayerPrefs.GetInt("PlayerSelectModel");
            tempObj = Instantiate(playerModerlData[curIndex].ModelPrefabs, playerModel.transform.position, playerModel.transform.rotation);
            tempObj.transform.parent = playerModel.transform;
        }
    }

     void BuySomething()
    {
        isShopEnter = false;
        playerMoney -= curPrice;
        PlayerPrefs.SetInt("Money", playerMoney);
        playerModerlData[curIndex].Price = 0;
        string tempKey = "Model" + curIndex.ToString();
        PlayerPrefs.SetInt(tempKey, 1); //如果买了数值写1
        GetMoney();
        GetPlayerModel();
        CloseShopEnter();
    }
    void CloseShopEnter()
    {
        shopEnter.SetActive(false);
        isShopEnter = false;
    }
    void GetPlayerModel()
    {
        for (int i = 0; i < 6;i++) //遍历六个模型是否购买
        {
            string tempKey = "Model" + i.ToString();
            if (PlayerPrefs.HasKey(tempKey))
            {
                if(PlayerPrefs.GetInt(tempKey)==1)
                    {
                    playerModerlData[i].Price = 0; //如果购买过价格写0
                }                
            }
            else
            {
                PlayerPrefs.SetInt(tempKey, 0);
            }
        }
    }
}
