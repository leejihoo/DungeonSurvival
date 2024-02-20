using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private Transform respondPos;
    [SerializeField] private Material coverMaterial;
    private float coveringTime;
    private void Awake()
    {
        coveringTime = 2f;
        var player = GameObject.Find("Player");
        player.transform.position = respondPos.position;
        Debug.Log("작동");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(StartLoadScene());
    }

    IEnumerator StartLoadScene()
    {
        // float currentTime = 0;
        // while (currentTime < coveringTime)
        // {
        //     currentTime += Time.deltaTime;
        //     //coverMaterial.SetFloat("_Progress",Mathf.cla);
        //     
        // }
        coverMaterial.DOFloat(0f, "_Progress", coveringTime).OnComplete(() =>
        {
            SceneManager.LoadScene(nextSceneName);
        });
        yield return null;
    }
    
    
}
