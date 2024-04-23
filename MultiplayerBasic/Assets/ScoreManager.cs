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
    public int scorePlayerOne;
    public List<GameObject> scoreImagePlayerOne;
    [Header("Player Two")]
    public int scorePlayerTwo;
    public List<GameObject> scoreImagePlayerTwo;
    
    private FixedString32Bytes playerName;
    private ulong ClientId { get; set; }

    private void Awake()
    {
        instance = this;
        UpdateScore();
    }
    
    public void Initialized(ulong clientId) //, FixedString32Bytes playerName
    {
        ClientId = clientId;
        if (ClientId == 0)
        {
            scorePlayerTwo += 1;
            
        }
        else if (ClientId == 1)
        {
            scorePlayerOne += 1;
            
        }

        UpdateScore();

    }
    
    private void UpdateScore()
    {
        switch (scorePlayerOne)
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
        
        switch (scorePlayerTwo)
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
