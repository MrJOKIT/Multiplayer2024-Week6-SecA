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
    public int hitPercent;
    public TMP_Text hitPointText;

    public PlayerInGame(KabigonPlayer kabigonPlayer, int hitPercent, TMP_Text hitPointText)
    {
        this.kabigonPlayer = kabigonPlayer;
        this.hitPercent = hitPercent;
        this.hitPointText = hitPointText;
    }
}
public class PlayerHitPointController : NetworkBehaviour, IDisposable
{
    [SerializeField] private List<PlayerInGame> playerInGames;
    [SerializeField] private List<TMP_Text> hitPointTexts;

    public void AddPlayer(KabigonPlayer playerKabigon)
    {
        playerInGames.Add(new PlayerInGame(playerKabigon,0,hitPointTexts[playerInGames.Count]));
    }
    
    public void Dispose()
    {
        playerInGames.Clear();
    }
}
