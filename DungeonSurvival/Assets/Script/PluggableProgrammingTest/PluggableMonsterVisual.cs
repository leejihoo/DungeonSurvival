using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.PluggableProgrammingTest
{
    public class PluggableMonsterVisual : MonoBehaviour
    {
        // [SerializeField] private Animator animator;
        // [SerializeField] private Image warningMark;
        // [SerializeField] private TMP_Text hp; 
        // private int _detectHash;
    
        // private void Awake()
        // {
        //     if (animator == null)
        //     {
        //         animator = GetComponent<Animator>();
        //     }
        //     _detectHash = Animator.StringToHash("IsDetectPlayer");
        // }

        public static IEnumerator Die(Action action, SpriteRenderer target)
        {
            Color current = target.color;
            while (current.a >= 0.3)
            {
                current.a -= 0.01f;
                target.color = current;
                yield return new WaitForSeconds(0.01f);
            }
        
            action.Invoke();
        }

        // public static void DecreaseHp(int damage, TMP_Text hpText, SpriteRenderer target)
        // {
        //     StartCoroutine(DamageEffect(target));
        //     ChangeHp(-damage, hpText);
        // }
    
        public static IEnumerator DamageEffect(SpriteRenderer target)
        {
            target.color = new Color(1,0.7f,0.7f,1);
            yield return new WaitForSeconds(0.1f);
            target.color = new Color(1, 1, 1, 1);
        }
    
        // public void PlayAttackAnimation()
        // {
        //     
        //     animator.SetBool(_detectHash,true);
        // }
        //
        // public void PlayMoveAnimation()
        // {
        //     
        //     animator.SetBool(_detectHash,false);
        // }

        public static void PlayAnimation(Animator animator,string stringToHash, bool setBool)
        {
            animator.SetBool(stringToHash, setBool);
        }

        public static void SetHp(int hp, TMP_Text hpText)
        {
            hpText.text = hp.ToString();
        }

        public static void ChangeHp(int value, TMP_Text hpText)
        {
            var currentHp = int.Parse(hpText.text);
            hpText.text = (currentHp + value).ToString();
        }

        // public void ChangeWarningMark(bool value)
        // {
        //     Debug.Log("마크변환작동");
        //     warningMark.enabled = value;
        // }
    
        public static void SetActiveImage(bool value, Image image)
        {
            image.enabled = value;
        }
    }
}
