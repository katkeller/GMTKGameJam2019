using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System;

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset inkAsset;

    [SerializeField]
    private Text jordanTextMessage;

    [SerializeField]
    private Button[] letterButton = new Button[26];

    [SerializeField]
    private Image playerTextMessageImage, jordanTextMessageImage, currentlyTypingImage;

    [SerializeField]
    private Text playerTextMessage;

    [SerializeField]
    private AudioClip jordanTextClip, sentClip;

    [SerializeField]
    private Keyboard keyboard;

    [SerializeField]
    private float secondsBeforeJordanReply = 2.0f;

    private Story inkStory;
    private string[] letters = new string[26] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
    private string playerInput = "";
    private string correctInput;
    private string choice1, choice2;
    private bool makingChoice = false;
    private bool playerIsTyping = false;
    private AudioSource audioSource;
    private int[] choiceIndex = new int[2];

    private string[] splitDialogue;
    private string dialogue;

    void Awake()
    {
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();
        playerTextMessageImage.enabled = false;
        jordanTextMessageImage.enabled = false;
        currentlyTypingImage.enabled = false;
        playerTextMessage.text = "";
        jordanTextMessage.text = "";

        for (int i = 0; i < 26; i++)
        {
            letterButton[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        CheckForDialogueSpeaker();
    }

    void Update()
    {
        
    }

    private void CheckForDialogueSpeaker()
    {
        while (inkStory.canContinue)
        {
            makingChoice = false;
            Debug.Log(inkStory.Continue());
            //string dialogue = inkStory.currentText;
            dialogue = inkStory.currentText;
            //string[] splitDialogue = dialogue.Split(':');
            splitDialogue = dialogue.Split(':');

            if (splitDialogue[0] == "You")
            {
                correctInput = splitDialogue[1];
                playerIsTyping = true;

                foreach (char c in splitDialogue[1])
                {
                    string letter = c.ToString();
                    int index = Array.IndexOf(letters, letter);
                    Debug.Log("index: " + index);
                    letterButton[index].gameObject.SetActive(true);
                }
            }
            else if (splitDialogue[0] == "Jordan")
            {
                jordanTextMessage.text = splitDialogue[1];
                jordanTextMessageImage.enabled = true;
                audioSource.PlayOneShot(jordanTextClip);
                CheckForDialogueSpeaker();
                //StartCoroutine(WaitForJordanReply());
            }
        }

        if (inkStory.currentChoices.Count > 0)
        {
            makingChoice = true;
            playerIsTyping = true;
            for (int e = 0; e < inkStory.currentChoices.Count; e++)
            {
                Choice choice = inkStory.currentChoices[e];
                string[] splitChoiceText = choice.text.Split(':');
                Debug.Log("Choice " + (e + 1) + ". " + choice.text);
                splitChoiceText[1] = splitChoiceText[1].TrimStart(' ');
                Debug.Log("SplitChoicetext: " + splitChoiceText[1] + ".");

                foreach (char c in splitChoiceText[1])
                {
                    string letter = c.ToString();
                    int index = Array.IndexOf(letters, c);
                    letterButton[index].gameObject.SetActive(true);
                }

                choiceIndex[e] = choice.index;
            }
        }
    }

    //IEnumerator WaitForJordanReply()
    //{
    //    jordanTextMessageImage.enabled = false;
    //    currentlyTypingImage.enabled = true;
    //    yield return new WaitForSeconds(secondsBeforeJordanReply);
    //    jordanTextMessage.text = splitDialogue[1];
    //    jordanTextMessageImage.enabled = true;
    //    audioSource.PlayOneShot(jordanTextClip);
    //    CheckForDialogueSpeaker();
    //}

    private void OnInputEntered(string input)
    {
        playerInput = input;
        correctInput = correctInput.TrimEnd();
        Debug.Log("input: " + playerInput + ".");
        Debug.Log("correct input: " + correctInput + ".");

        if (input == correctInput)
        {
            Debug.Log("input choice is correct.");
            for (int j = 0; j < 26; j++)
            {
                letterButton[j].gameObject.SetActive(false);
            }

            playerIsTyping = false;
            playerTextMessage.text = input;
            playerTextMessageImage.enabled = true;
            keyboard.ResetInput();
            CheckForDialogueSpeaker();
        }

        if (playerIsTyping && makingChoice && input == choice1)
        {
            audioSource.PlayOneShot(sentClip);
            inkStory.ChooseChoiceIndex(choiceIndex[0]);
            playerTextMessage.text = input;
            playerTextMessageImage.enabled = true;
            makingChoice = false;
            playerIsTyping = false;
            CheckForDialogueSpeaker();
        }
        else if (playerIsTyping && makingChoice && input == choice2)
        {
            audioSource.PlayOneShot(sentClip);
            inkStory.ChooseChoiceIndex(choiceIndex[1]);
            playerTextMessage.text = input;
            playerTextMessageImage.enabled = true;
            makingChoice = false;
            playerIsTyping = false;
            CheckForDialogueSpeaker();
        }
    }

    private void OnEnable()
    {
        Keyboard.InputEntered += OnInputEntered;
    }

    private void OnDisable()
    {
        Keyboard.InputEntered -= OnInputEntered;
    }
}
