using System.Collections;
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
            StartCoroutine(StartLoadScene());
    }

    IEnumerator StartLoadScene()
    {
        coverMaterial.DOFloat(0f, "_Progress", coveringTime).OnComplete(() =>
        {
            SceneManager.LoadScene(nextSceneName);
        });
        yield return null;
    }
}
