using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : SingleTon<MainSceneManager>
{

    [Header("¼àÌý")]
    public SceneLoadData_SO NeedLoadScene;
    [Header("¹ã²¥")]
    public VoidEvent_SO SceneHadLoad;

    public string currentSceneName;
    public Vector3 playerStayPosition;
    protected override void Awake()
    {
        base.Awake();
        currentSceneName = null;
        OwnLoadNewScene("Train", playerStayPosition,2);
        //NeedLoadScene.RaiseAction("Train", playerStayPosition, 2);
    }

    private void OnEnable()
    {
        NeedLoadScene.Action += LoadNewScene;
        
    }
    private void OnDisable()
    {
        NeedLoadScene.Action -= LoadNewScene;
    }

    private void LoadNewScene(string sceneName, Vector3 playerPosition, int index)
    {
        OwnLoadNewScene(sceneName, playerPosition,index);
    }
    public void OwnLoadNewScene(string SceneName,Vector3 position, int index)
    {
        //AsyncOperation isEndUnload;
        //SceneManager.LoadSceneAsync(SceneName);
        //isEndUnload =  SceneManager.UnloadSceneAsync(currentSceneName);
        //if (isEndUnload.isDone)
        //{

        //}
        playerStayPosition = position;
        if(currentSceneName != null)
        {
            Debug.Log("Start UnLoad");
            StartCoroutine(UnLoadCurrentScene(SceneName,index));

        }
        else
        {
            Debug.Log("Start Load");
            StartCoroutine(OwnLoadNewScene(SceneName));
        }

    }

    IEnumerator UnLoadCurrentScene(string SceneName,int index)
    {
        //TODO:½¥Èë½¥³ö
        yield return SceneManager.UnloadSceneAsync(currentSceneName);
        yield return OwnLoadNewScene(SceneName);
    }
    IEnumerator OwnLoadNewScene(string SceneName)
    {
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        currentSceneName = SceneName;
        PlayerStateManager.Instance.player.GetComponent<Rigidbody2D>().position = playerStayPosition;
        SceneHadLoad.RaiseAction();
    }
}
