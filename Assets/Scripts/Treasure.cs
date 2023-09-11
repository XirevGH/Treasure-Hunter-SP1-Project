using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private Animator anim;
    public int treasureOpened = 0;
    [SerializeField] private GameObject questCompleted;
    [SerializeField] private GameObject activeQuest;
    [SerializeField] private GameObject treasureParticles;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Base Layer.TreasureClosed");
    }

    private void Update()
    {
        if (treasureOpened == 0)
        {
            questCompleted.SetActive(false);
        }

        if (treasureOpened == 1)
        {
            questCompleted.SetActive(true);
            activeQuest.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerMovement>().keysCollected > 0)
        {
            if (other.CompareTag("Player"))
            {
                anim.SetTrigger("OpenTreasure 0");
                treasureOpened++;
                Instantiate(treasureParticles, transform.position, Quaternion.identity);
            }
        }
        else
        {
            return;
        }
    }
}
