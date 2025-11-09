using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    //[SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Choices UI")]
    [SerializeField] private Button[] choices;
    private TextMeshProUGUI[] choicesText;

    [SerializeField]
    private TextAsset currentStoryJSON = null;
    private Story currentStory;

    [SerializeField] private Button Continue;
    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;
   // private const string SPEAKER_TAG = "speaker";

    private System.Action onStoryComplete; //callback when story finishes


    //[SerializeField] public Image portraitImage;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        DialogueManager.GetInstance().StartStory();
        // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int index = 0; index < choices.Length; index++)
        {
            choicesText[index] = choices[index].GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void StartStory() => StartStory(currentStoryJSON);
    public void StartStory(TextAsset inkJSON)
    {
        currentStoryJSON = inkJSON;
        currentStory = new Story(currentStoryJSON.text);

       // BindExternalFunctions();

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);


      //  displayNameText.text = "???";

        RefreshView();


       // portraitImage.enabled = true;
    }
    public void RefreshView()
    {

       // portraitImage.enabled = true;

        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            text = text.Trim();
            dialogueText.text = text;

            //HandleTags(currentStory.currentTags);
        }
        else
        {
            dialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";

          //  if (portraitImage != null)
              //  portraitImage.enabled = false; // hide portrait when dialogue ends

            onStoryComplete?.Invoke();
        }

        DisplayChoices();
    }
  /*  private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
                continue;
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;

                    var data = PassengerManager.Instance.GetPassengerByName(tagValue);
                    if (data != null && portraitImage != null)
                    {
                        portraitImage.sprite = data.portrait;
                        portraitImage.enabled = true;
                    }
                    else
                    {
                        Debug.LogWarning($"No portrait found for speaker: {tagValue}");
                        portraitImage.enabled = false;
                    }
                    break;

                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    } */


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        Continue.gameObject.SetActive(currentChoices.Count == 0);
        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError($"More choices were given than the UI can support. Number of choices given: {currentChoices.Count}");
        }

        for (int index = 0; index < choices.Length; index++)
        {
            if (index < currentChoices.Count)
            {
                // enable and initialize the choices up to the amount of choices for this line of dialogue
                choices[index].gameObject.SetActive(true);
                choicesText[index].text = currentChoices[index].text;
            }
            else
            {
                // go through the remaining choices the UI supports and make sure they're hidden
                choices[index].gameObject.SetActive(false);
            }

        }
    }
    public void MakeChoice(int choiceIndex)
    {
        Debug.Log($"Choice {choiceIndex}");
        currentStory.ChooseChoiceIndex(choiceIndex);
        RefreshView();
    }

  /*  public void SetPortrait(Sprite portrait)
    {
        if (portraitImage != null)
        {
            portraitImage.sprite = portrait;
            portraitImage.enabled = portrait != null;
        }
    }*/



   /* private void BindExternalFunctions()
    {
        currentStory.BindExternalFunction("SpawnPassenger", (string passengerName) =>
        {
            var data = PassengerManager.Instance.GetPassengerByName(passengerName);
            if (data != null)
                PassengerManager.Instance.SpawnPassenger(data);
            else
                Debug.LogWarning($"SpawnPassenger: Passenger '{passengerName}' not found!");
        });

        currentStory.BindExternalFunction("RemovePassenger", (string passengerName) =>
        {
            var data = PassengerManager.Instance.GetPassengerByName(passengerName);
            if (data != null)
                PassengerManager.Instance.RemovePassenger(data);
            else
                Debug.LogWarning($"RemovePassenger: Passenger '{passengerName}' not found!");
        });
    }*/
}