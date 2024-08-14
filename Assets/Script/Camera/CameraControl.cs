using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CameraControl : MonoBehaviour
{
    [Header("¼àÌý")]
    public VoidEvent_SO SceneHadLoad;

    private CinemachineConfiner confiner; 
    private CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    private void OnEnable()
    {
        SceneHadLoad.Action += RefreshBound;
    }
    private void OnDisable()
    {
        SceneHadLoad.Action -= RefreshBound;
    }

    private void RefreshBound()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("SelfSceneManager");
        if (obj == null)
            return;
        confiner.m_BoundingShape2D = obj.GetComponent<SelfSceneManager>().bound1.GetComponent<Collider2D>();
        confiner.InvalidatePathCache();
        //confiner2D.m_BoundingShape2D = obj.GetComponent<SelfSceneManager>().bound1.GetComponent<Collider2D>();
        //confiner2D.InvalidateCache();
        //var componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        //Debug.Log("componentBase = " + componentBase);
        //virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = obj.GetComponent<SelfSceneManager>().CameraDistance;
        virtualCamera.m_Lens.OrthographicSize = obj.GetComponent<SelfSceneManager>().CameraDistance;
    }
}
