using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Keyboard : MonoBehaviour
{
    [SerializeField]
    private Text inputField;

    //[SerializeField]
    //private Image playerTextMessageImage;

    [SerializeField]
    private AudioClip typeClip, sendClip;

    private string input;
    private AudioSource audioSource;

    public static event Action<string> InputEntered;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //playerTextMessageImage.enabled = false;
        //playerTextMessage.text = "";
        inputField.text = "";
    }

    public void AddLetterToInput( string letter)
    {
        input = input + letter;
        inputField.text = input;
        audioSource.PlayOneShot(typeClip);
        Debug.Log("Keyboard input:" + input);
    }

    public void SendMessage()
    {
        InputEntered?.Invoke(input);
        ResetInput();
    }

    public void ResetInput()
    {
        inputField.text = "";
        input = "";
        Debug.Log("keyboard input: " + input);
    }
}
