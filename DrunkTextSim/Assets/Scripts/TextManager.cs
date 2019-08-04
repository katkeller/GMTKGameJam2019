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
    private Image playerTextMessageImage;

    [SerializeField]
    private Text playerTextMessage;

    [SerializeField]
    private AudioClip jordanTextClip, sentClip;

    [SerializeField]
    private Keyboard keyboard;

    private Story inkStory;
    private string[] letters = new string[26] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
    private string playerInput = "";
    private string correctInput;
    private string choice1, choice2;
    private bool makingChoice = false;
    private bool playerIsTyping = false;
    private AudioSource audioSource;
    private int[] choiceIndex = new int[2];

    //private string[,] letterAssociations = new string[,]
    //{
    //    {"q", "0"},
    //    {"w", "1"},
    //    {"e", "2"},
    //    {"r", "3"},
    //    {"t", "4"},
    //    {"y", "5"},
    //    {"u", "6"},
    //    {"i", "7"},
    //    {"o", "8"},
    //    {"p", "9"},
    //    {"a", "10"},
    //    {"s", "11"},
    //    {"d", "12"},
    //    {"f", "13"},
    //    {"g", "14"},
    //    {"h", "15"},
    //    {"j", "16"},
    //    {"k", "17"},
    //    {"l", "18"},
    //    {"z", "19"},
    //    {"x", "20"},
    //    {"c", "21"},
    //    {"v", "22"},
    //    {"b", "23"},
    //    {"n", "24"},
    //    {"m", "25"}
    //}; 

    void Awake()
    {
        inkStory = new Story(inkAsset.text);
        audioSource = GetComponent<AudioSource>();
        playerTextMessageImage.enabled = false;
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
            string dialogue = inkStory.currentText;
            string[] splitDialogue = dialogue.Split(':');

            if (splitDialogue[0] == "You")
            {
                correctInput = splitDialogue[1];
                playerIsTyping = true;

                foreach (char c in splitDialogue[1])
                {
                    string letter = c.ToString();
                    //string value = Array.Find(letters, s => s.Equals(letter));
                    //Array.Find(letterAssociations, s =>)
                    //Button button = Array.Find(letterButton, )
                    //letterAssociations[letter, 1];
                    int index = Array.IndexOf(letters, letter);
                    Debug.Log("index: " + index);
                    letterButton[index].gameObject.SetActive(true);
                }
            }
            else if (splitDialogue[0] == "Jordan")
            {
                jordanTextMessage.text = splitDialogue[1];
                audioSource.PlayOneShot(jordanTextClip);
                CheckForDialogueSpeaker();
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

    private void OnInputEntered(string input)
    {
        //playerInput = input + System.Environment.NewLine;
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
