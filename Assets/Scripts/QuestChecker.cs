using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestChecker : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox, finishedText, unfinishedText;
    [SerializeField] private int questGoal = 1;
    [SerializeField] private int levelToLoad;
    public bool levelIsLoading = false;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (FindAnyObjectByType<Treasure>().GetComponent<Treasure>().treasureOpened == questGoal)
            {
                dialogueBox.SetActive(true);
                finishedText.SetActive(true);
                unfinishedText.SetActive(false);
                Invoke("LoadNextLevel", 2f);
                levelIsLoading = true;
                //anim.SetTrigger("SetSail");
            }
            if (FindAnyObjectByType<Treasure>().GetComponent<Treasure>().treasureOpened == 0)
            {
                dialogueBox.SetActive(true);
                unfinishedText.SetActive(true);
                finishedText.SetActive(false);
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void OnTriggerExit2D(Collider2D other )
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            dialogueBox.SetActive(false);
            finishedText.SetActive(false);
            finishedText.SetActive(false);
        }
    }
}
