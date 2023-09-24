using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip regularBackgroundMusic, bossMusic1;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource.PlayOneShot(regularBackgroundMusic);
    }

    void Update()
    {
        if (FindAnyObjectByType<Drawbridge>().GetComponent<Drawbridge>().bossCanMove1 == true)
        {
            audioSource.PlayOneShot(bossMusic1);
        }
        else
        {
            return;
        }
    }
}
