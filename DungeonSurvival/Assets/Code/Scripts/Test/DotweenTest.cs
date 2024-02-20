using DG.Tweening;
using UnityEngine;

public class DotweenTest : MonoBehaviour
{
    [SerializeField] private Transform player; 
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(player.position, 1).SetEase(Ease.InQuad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
