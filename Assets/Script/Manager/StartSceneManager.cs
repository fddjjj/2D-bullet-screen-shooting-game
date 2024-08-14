using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton;
    private void Awake()
    {
        startButton.onClick.AddListener(GameStart);
    }
    public void GameStart()
    {
        SceneManager.LoadSceneAsync("Main");
        //SceneLoadEvent.RaiseAction("Train", playerStayPosition);
        //SceneManager.LoadSceneAsync("Train", LoadSceneMode.Additive);
    }
}
