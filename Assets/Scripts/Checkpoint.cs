using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void Start()
    {
        Color greyscale = new Color(0.3f, 0.4f, 0.6f);
        GetComponent<Renderer>().material.SetColor("_Color", greyscale);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerProperties>().spawnPosition.position = transform.position;

            Color defaultColor = new Color(1f, 1f, 1f);
            GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
        }
    }
}
