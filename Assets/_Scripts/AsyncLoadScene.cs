using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Globe
{
    public static string nextSceneName;
}

public class AsyncLoadScene : MonoBehaviour
{
    public Slider loadingSlider;
    public Text loadingText;
    private float loadingSpeed = 1;
    private float targetValue;
    private AsyncOperation operation;
	void Start ()
    {
        loadingSlider.value = 0f;
        if (SceneManager.GetActiveScene().name=="Game01")
        {
            StartCoroutine(AsyncLoading());
        }
	}
    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);//加载目标场景
        operation.allowSceneActivation = false; //防止自动切换场景
        yield return operation;
    }
	
	void Update ()
    {
        targetValue = operation.progress;
        if (operation.progress >= 0.9f) //operation.progress 最大值0.9
        {
            targetValue = 1f;
        }
        if (targetValue != loadingSlider.value)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed);//插值过度
            if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
            {
                loadingSlider.value = targetValue;
            }
        }
        loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";
        if ((int)(loadingSlider.value * 100) == 100)
        {
            operation.allowSceneActivation = true;
        }
	}
}
