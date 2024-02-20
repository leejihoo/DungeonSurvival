using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private GameObject hpHolder;
    [SerializeField] private GameObject heart;

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
        Debug.Log("사망");
    }

    IEnumerator DamageEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            //Debug.Log("데미지 비쥬얼 작동중");
            var spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1f, 1f, 0.7f);
            }
            //transform.GetComponent<SpriteRenderer>().color = new Color(1,1f,1f,0.7f);
            yield return new WaitForSeconds(0.1f);
            //transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            // transform.GetComponentsInChildren<SpriteRenderer>()
            //     .Select(child => child.color = new Color(1, 1f, 1f, 1f));
            foreach (var spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(1, 1f, 1f, 1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void AttackExit()
    {
        Debug.Log("공격 종료");
        GetComponent<Animator>().ResetTrigger("IsAttack");
    }
}
