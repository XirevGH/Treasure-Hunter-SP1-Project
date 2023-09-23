using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawbridge : MonoBehaviour
{
    private Animator anim;
    private bool hasPlayedAnimation = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasPlayedAnimation)
        {
            hasPlayedAnimation = true;
            anim.SetTrigger("CloseDrawbridge");
        }
    }

}
