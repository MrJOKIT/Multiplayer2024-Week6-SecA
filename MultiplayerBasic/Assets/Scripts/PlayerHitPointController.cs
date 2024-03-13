using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class PlayerInGame 
{
    public KabigonPlayer kabigonPlayer;
    public float hitPercent;
    public TMP_Text hitPointText;

    public PlayerInGame(KabigonPlayer kabigonPlayer, float hitPercent, TMP_Text hitPointText)
    {
        this.kabigonPlayer = kabigonPlayer;
        this.hitPercent = hitPercent;
        this.hitPointText = hitPointText;
    }
}
public class PlayerHitPointController : NetworkBehaviour, IDisposable
{
    public static PlayerHitPointController instance;
    
    [SerializeField] private List<PlayerInGame> playerInGames = new List<PlayerInGame>();
    [SerializeField] private List<TMP_Text> hitPointTexts;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        
    }

    public void AddPlayer(KabigonPlayer playerKabigon,float hitPercent)
    {
        PlayerInGame player = new PlayerInGame(playerKabigon, hitPercent, hitPointTexts[playerInGames.Count]);
        //Debug.Log(playerInGames.Count);
    }

    public void Dispose()
    {
        playerInGames.Clear();
    }
}
