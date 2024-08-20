using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelfSceneManager : MonoBehaviour
{

    [Header("¼àÌý")]
    public VoidEvent_SO SceneHadLoad;

    [Header("×é¼þ")]
    public GameObject bound1;
    public GameObject bound2;

    public float CameraDistance;
    public AudioClip BGM_Clip;

    private void OnEnable()
    {
        SceneHadLoad.Action += RefreshBGM;
    }

    private void OnDisable()
    {
        SceneHadLoad.Action -= RefreshBGM;
    }
    private void RefreshBGM()
    {
        if(BGM_Clip != null)
        {
            AudioManager.Instance.BGM_Source.clip = BGM_Clip;
            AudioManager.Instance.BGM_Source.Play();
        }
       
    }
}
