using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    #region Inspector Fields
    
    [SerializeField] private GameObject hpHolder;
    [SerializeField] private GameObject heart;

    #endregion

    #region Functions
    
    public void InitHp(int count)
    {
        while(count > 0)
        {
            Instantiate(heart, hpHolder.transform);
            count--;
        }
    }

    public void DecreaseHp(int value)
    {
        while (hpHolder.transform.childCount > 0 && value > 0)
        {
            Destroy(hpHolder.transform.GetChild(hpHolder.transform.childCount - 1).gameObject);
            value--;
        }
    }
    
    public void OnDamage(int value, Transform unused)
    {
        DecreaseHp(value);
        StartCoroutine(DamageEffect());
    }

    public void OnDie()
    {
        var childCount = hpHolder.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(hpHolder.transform.GetChild(childCount - (i+1)).gameObject);
        }
    }

    #endregion

    #region Coroutines
    
    IEnumerator DamageEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            var spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1f, 1f, 0.7f);
            }
            yield return new WaitForSeconds(0.1f);
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1f, 1f, 1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    #endregion
}
