using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCanvasController : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetCanvasCamera;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetCanvasCamera;
    }
    
    public void SetCanvasCamera(Scene scene, LoadSceneMode mode)
    {
        GetComponentInChildren<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
}
