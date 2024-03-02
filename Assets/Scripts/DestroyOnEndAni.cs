using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEndAni : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator == null )
        {
            Debug.Log("You need Animator component to use DestroyOnEndAni");
            Destroy(this);
        }
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
