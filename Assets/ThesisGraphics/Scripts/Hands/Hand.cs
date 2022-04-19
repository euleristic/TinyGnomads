using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{
    public float speed;

    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private float pointTarget;
    private float gripCurrent;
    private float triggerCurrent;
    private float pointCurrent;
    private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "Trigger";
    private string animatorPointParam = "Point";

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }


    void Update()
    {
        AnimateHand();
    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    internal void SetPoint(float v)
    {
        pointTarget = v;
    }

    void AnimateHand() 
    {
        if (gripCurrent != gripTarget) 
        {
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGripParam, gripCurrent);
        }

        if (triggerCurrent != triggerTarget)
        {
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorTriggerParam, triggerCurrent);
        }

        if (pointCurrent != pointTarget)
        {
            pointCurrent = Mathf.MoveTowards(pointCurrent, pointTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorPointParam, pointCurrent);
        }
    }
}
