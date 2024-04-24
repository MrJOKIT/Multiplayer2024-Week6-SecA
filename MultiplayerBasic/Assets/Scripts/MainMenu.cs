using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
    [SerializeField] private Button joinButton;

    private void Start()
    {
        SoundManager.instancne.PlayMusic(1);
    }

    private void Update()
    {
        if (joinCodeField.text == string.Empty)
        {
            joinButton.interactable = false;
        }
        else
        {
            joinButton.interactable = true;
        }
    }

    public void PlaySound()
    {
        SoundManager.instancne.PlaySoundEffect(7);
    }
    public async void StartHost()
    {
        SoundManager.instancne.PlaySoundEffect(7);
        await HostSingleton.Instance.GameManager.StartHostAsynce();
    }

    public async void StartClient()
    {
        SoundManager.instancne.PlaySoundEffect(7);
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
}
