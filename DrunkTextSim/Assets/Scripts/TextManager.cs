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

    private Story inkStory;
    private string[] letters = new string[26] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };

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

        for (int i = 0; i < 26; i++)
        {
            letterButton[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    private void CheckForDialogueSpeaker()
    {
        string dialogue = inkStory.currentText;
        string[] splitDialogue = dialogue.Split(':');

        if (splitDialogue[0] == "You")
        {
            foreach (char c in splitDialogue[1])
            {
                string letter = c.ToString();
                //string value = Array.Find(letters, s => s.Equals(letter));
                //Array.Find(letterAssociations, s =>)
                //Button button = Array.Find(letterButton, )
                //letterAssociations[letter, 1];
                int index = Array.IndexOf(letters, c);
                letterButton[index].gameObject.SetActive(true);

            }
        }
        else if (splitDialogue[0] == "Jordan")
        {
            jordanTextMessage.text = splitDialogue[1];
        }
    }
}
