using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void Awake()
    {
        DialogueManager.GetInstance().StartStory();




    }
    // Update is called once per frame
   }
