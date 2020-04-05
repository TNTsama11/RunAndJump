using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public GameObject gameOver;
    public GameObject bestScore;
    public GameObject score;
    public GameObject exitButton;
    public GameObject retryButton;
    public GameObject PBA;
    public GameObject EBA;
    public GameObject STA;
    public GameObject BSTA;
    public GameObject GOA;
    public GameObject backGround;
    int scoreNum;
    int bestScoreNum;


    static UIManager _instance;
    public static UIManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;

        exitButton.GetComponent<Button>().onClick.AddListener(ClickHomeBtn);
        retryButton.GetComponent<Button>().onClick.AddListener(ClickRetryBtn);
    }

    void Start ()
    {
		
	}

	void Update ()
    {
		
	}

    public void ShowGameOver()
    {
        gameOverCanvas.SetActive(true);
        backGround.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 1f);
        Tween tween = gameOver.GetComponent<RectTransform>().DOAnchorPos(GOA.GetComponent<RectTransform>().anchoredPosition, 0.2f).SetEase(Ease.InBounce);
        tween.OnComplete(ShowScoreText);
    }
    public void ShowScoreText()
    {
        Tween tween = score.GetComponent<RectTransform>().DOAnchorPos(STA.GetComponent<RectTransform>().anchoredPosition, 0.2f).SetEase(Ease.InBounce);
        bestScore.GetComponent<RectTransform>().DOAnchorPos(BSTA.GetComponent<RectTransform>().anchoredPosition, 0.2f).SetEase(Ease.InBounce);
        tween.OnComplete(ShowButton);
    }
    public void ShowButton()
    {
        Tween tween = retryButton.GetComponent<RectTransform>().DOAnchorPos(PBA.GetComponent<RectTransform>().anchoredPosition, 0.5f);//.SetEase(Ease.InBounce);
        exitButton.GetComponent<RectTransform>().DOAnchorPos(EBA.GetComponent<RectTransform>().anchoredPosition, 0.5f);//.SetEase(Ease.InBounce);
    }

    public void ShowScore()
    {
        scoreNum = PlayerPrefs.GetInt("Score");
        if (!PlayerPrefs.HasKey("BestScore"))
        {
            bestScoreNum = scoreNum;
        }
        else
        {
            bestScoreNum= PlayerPrefs.GetInt("BestScore");
        }
        score.transform.Find("Text").GetComponent<Text>().text = scoreNum.ToString();
       bestScore.transform.Find("Text").GetComponent<Text>().text = bestScoreNum.ToString();

    }

    void ClickHomeBtn()
    {
        SceneManager.LoadScene("start");
        int tempMoney = PlayerPrefs.GetInt("Money") + RoadManager.Instance.GoldNumber; //计算钱
        PlayerPrefs.SetInt("Money", tempMoney);
        PlayerPrefs.SetInt("Again", 0);
    }
    void ClickRetryBtn()
    {
        int tempNum = PlayerPrefs.GetInt("Again");
        PlayerPrefs.SetInt("Again", tempNum + 1);
        Globe.nextSceneName = "Game02";
        SceneManager.LoadScene("Game01");
    }


}
