using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private AnimationEvent animationEvent;
    private Animator animator;
    [SerializeField] private AnimationClip animationClip;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animationEvent = animationClip.events[0];
        animationEvent.objectReferenceParameter = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // animation event function
    public void OnCharge()
    {
        Debug.Log("공격 발사");

    }

    public void Shoot()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
