using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : MonoBehaviour
{
    private Animator anim;
    private bool hasPlayedAnimation = false;
    public bool bossCanMove1 = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasPlayedAnimation)
        {
            bossCanMove1 = true;
            hasPlayedAnimation = true;
            anim.SetTrigger("CloseDrawbridge");   
        }
    }

}
