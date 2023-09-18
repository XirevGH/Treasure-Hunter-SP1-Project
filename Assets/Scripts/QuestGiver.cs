using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private GameObject textPopUp;
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject questFindKey;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopUp.SetActive(true);
            questUI.SetActive(true);
            questFindKey.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textPopUp.SetActive(false);
        }
    }
}
