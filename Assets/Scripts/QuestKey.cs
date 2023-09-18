using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestKey : MonoBehaviour
{
    [SerializeField] private GameObject questFindKey;
    [SerializeField] private GameObject questFindTreasure;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            questFindKey.SetActive(false);
            questFindTreasure.SetActive(true);
        }
    }
}
