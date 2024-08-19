using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class StartSceneManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
