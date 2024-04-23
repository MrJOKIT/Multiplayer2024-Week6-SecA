using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class ScorePlayer
{
    
    public List<GameObject> scoreImage;
}

public class ScoreManager : NetworkBehaviour
{
    public static ScoreManager instance;
    [Header("Player One")]
    public NetworkVariable<int> scorePlayerOne;
    public List<GameObject> scoreImagePlayerOne;
    [Header("Player Two")]
    public NetworkVariable<int> scorePlayerTwo;
    public List<GameObject> scoreImagePlayerTwo;
    
    private FixedString32Bytes playerName;
    private ulong ClientId { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if(!IsClient) { return; }

        scorePlayerOne.OnValueChanged += HandleUpdateScore;
        scorePlayerTwo.OnValueChanged += HandleUpdateScore;
        HandleUpdateScore(0,scorePlayerOne.Value);
        HandleUpdateScore(0,scorePlayerTwo.Value);
    }

    public override void OnNetworkDespawn()
    {
        if(!IsClient) { return; }
        scorePlayerOne.OnValueChanged -= HandleUpdateScore;
        scorePlayerTwo.OnValueChanged -= HandleUpdateScore;
    }

    public void Initialized(ulong clientId) //, FixedString32Bytes playerName
    {
        ClientId = clientId;
        if (ClientId == 0)
        {
            scorePlayerTwo.Value += 1;
            
        }
        else if (ClientId == 1)
        {
            scorePlayerOne.Value += 1;
            
        }
    }
    
    private void HandleUpdateScore(int oldScore,int newScore)
    {
        switch (scorePlayerOne.Value)
        {
            case 0: scoreImagePlayerOne[0].SetActive(false);
                    scoreImagePlayerOne[1].SetActive(false);
                    scoreImagePlayerOne[2].SetActive(false);
                    break;
            case 1: scoreImagePlayerOne[0].SetActive(true);
                    scoreImagePlayerOne[1].SetActive(false);
                    scoreImagePlayerOne[2].SetActive(false);
                    break;
            case 2: scoreImagePlayerOne[0].SetActive(true);
                    scoreImagePlayerOne[1].SetActive(true);
                    scoreImagePlayerOne[2].SetActive(false);
                    break;
            case 3: scoreImagePlayerOne[0].SetActive(true);
                    scoreImagePlayerOne[1].SetActive(true);
                    scoreImagePlayerOne[2].SetActive(true);
                    break;
        }
        
        switch (scorePlayerTwo.Value)
        {
            case 0: scoreImagePlayerTwo[0].SetActive(false);
                    scoreImagePlayerTwo[1].SetActive(false);
                    scoreImagePlayerTwo[2].SetActive(false);
                    break;
            case 1: scoreImagePlayerTwo[0].SetActive(true);
                    scoreImagePlayerTwo[1].SetActive(false);
                    scoreImagePlayerTwo[2].SetActive(false);
                    break;
            case 2: scoreImagePlayerTwo[0].SetActive(true);
                    scoreImagePlayerTwo[1].SetActive(true);
                    scoreImagePlayerTwo[2].SetActive(false);
                    break;
            case 3: scoreImagePlayerTwo[0].SetActive(true);
                    scoreImagePlayerTwo[1].SetActive(true);
                    scoreImagePlayerTwo[2].SetActive(true);
                    break;
        }
    }
    
}
