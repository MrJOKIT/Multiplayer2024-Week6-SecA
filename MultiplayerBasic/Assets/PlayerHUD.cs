using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHUD : NetworkBehaviour
{
    public PlayerHealth playerHealth;
    [SerializeField] private TMP_Text hitText;
    [SerializeField] private TMP_Text lifeText;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) { return; }
        
        playerHealth.hitPercent.OnValueChanged += HandleHitPercentChanged;
        HandleHitPercentChanged(0,playerHealth.hitPercent.Value);
        playerHealth.life.OnValueChanged += HandleLifeChanged;
        HandleLifeChanged(0,playerHealth.life.Value);
    }
    
    public override void OnNetworkDespawn()
    {
        if (!IsClient) { return; }
        playerHealth.hitPercent.OnValueChanged -= HandleHitPercentChanged;
        playerHealth.life.OnValueChanged -= HandleLifeChanged;
        
    }

    void Update()
    {
        if (!IsClient)
        {
            return;
        }
        if (playerHealth.hitPercent.Value > 80)
        {
            hitText.color = playerHealth.highHp;
        }
        else if (playerHealth.hitPercent.Value > 40)
        {
            hitText.color = playerHealth.mediumHp;
        }
        else
        {
            hitText.color = playerHealth.lowHp;
        }
    }
    
    private void HandleHitPercentChanged(float oldPercent,float newPercent)
    {
        float calHitPercent = newPercent;
        calHitPercent = Mathf.Round(calHitPercent * 10f) * 0.1f;
        hitText.text = calHitPercent.ToString();
    }
    
    private void HandleLifeChanged(int oldCount,int newCount)
    {
        lifeText.text = "LIFE: " + newCount;
    }
}
