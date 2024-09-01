using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : SingleTon<MainSceneManager>
{

    [Header("监听")]
    public SceneLoadData_SO NeedLoadScene;
    [Header("广播")]
    public VoidEvent_SO SceneHadLoad;

    public string currentSceneName;
    public Vector3 playerStayPosition;
    public TransData currentTransData;
    public TransData Trans;
    protected override void Awake()
    {
        base.Awake();
        currentSceneName = null;
        OwnLoadNewScene("Train", playerStayPosition,Trans);
        //NeedLoadScene.RaiseAction("Train", playerStayPosition);
    }

    private void OnEnable()
    {
        NeedLoadScene.Action += LoadNewScene;
        
    }
    private void OnDisable()
    {
        NeedLoadScene.Action -= LoadNewScene;
    }

    private void LoadNewScene(string sceneName, Vector3 playerPosition,TransData transData)
    {
        OwnLoadNewScene(sceneName, playerPosition,transData);
    }
    public void OwnLoadNewScene(string SceneName,Vector3 position,TransData transData)
    {
        //AsyncOperation isEndUnload;
        //SceneManager.LoadSceneAsync(SceneName);
        //isEndUnload =  SceneManager.UnloadSceneAsync(currentSceneName);
        //if (isEndUnload.isDone)
        //{

        //}
        currentTransData = transData;
        playerStayPosition = position;
        if(currentSceneName != null)
        {
            Debug.Log("Start UnLoad");
            StartCoroutine(UnLoadCurrentScene(SceneName));

        }
        else
        {
            Debug.Log("Start Load");
            StartCoroutine(OwnLoadNewScene(SceneName));
        }

    }

    IEnumerator UnLoadCurrentScene(string SceneName)
    {
        //TODO:渐入渐出
        yield return SceneManager.UnloadSceneAsync(currentSceneName);
        yield return OwnLoadNewScene(SceneName);
    }
    IEnumerator OwnLoadNewScene(string SceneName)
    {
        yield return FadeStart(0.5f);
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        currentSceneName = SceneName;
        PlayerStateManager.Instance.player.GetComponent<Rigidbody2D>().position = playerStayPosition;
        if (currentTransData.hasBoss)
        {
            EnemyHealthCanvasControl.Instance.gameObject.SetActive(true);
            StopCanvasControl.Instance.RefightButton.gameObject.SetActive(true);
        }
        else
        {
            EnemyHealthCanvasControl.Instance.gameObject.SetActive(false);
            StopCanvasControl.Instance.RefightButton.gameObject.SetActive(false);
        }
        if (currentTransData.needPlayerHealth)
        {
            HealthCanvasControl.Instance.gameObject.SetActive(true);
            PlayerStateManager.Instance.playHealth = PlayerStateManager.Instance.playerMaxHealth;
            HealthCanvasControl.Instance.RefreshHealth();

        }else
        {
            HealthCanvasControl.Instance.gameObject.SetActive(false);
        }
        SceneHadLoad.RaiseAction();
        yield return FadeEnd(0.5f);
        yield break;
    }

    public IEnumerator FadeStart(float time)
    {
        FadeCanvasControl.Instance.gameObject.SetActive(true);
        Color tmpColor = FadeCanvasControl.Instance.background.color;
        //while (elapsedTime < time)
        //{
        //    elapsedTime += Time.deltaTime;
        //    tmpColor.a = Mathf.Clamp01(elapsedTime / time);
        //    FadeCanvasControl.Instance.background.color = tmpColor;
        //    yield return null;
        //}
        tmpColor.a = 1f;
        FadeCanvasControl.Instance.background.color = tmpColor;
        yield return new WaitForSeconds(time/2);
        Debug.Log("渐入");
        yield break;

    }
    public IEnumerator FadeEnd(float time)
    {
        //FadeCanvasControl.Instance.gameObject.SetActive (true);
        Color tmpColor = FadeCanvasControl.Instance.background.color;
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            tmpColor.a = Mathf.Clamp01(1.0f - (elapsedTime / time));
            FadeCanvasControl.Instance.background.color = tmpColor;
            yield return null;
        }
        Debug.Log("渐出");
        FadeCanvasControl.Instance.gameObject.SetActive(false);
    }
}
