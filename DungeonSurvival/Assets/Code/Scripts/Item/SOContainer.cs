using UnityEngine;

public class SOContainer : MonoBehaviour
{
    public ItemSO item;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
