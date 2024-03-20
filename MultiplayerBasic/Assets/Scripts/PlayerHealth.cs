using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [Header("Over Time")]
    public NetworkVariable<float> outAreaTime = new NetworkVariable<float>(3.5f);
    [field: SerializeField] public float maxOutAreaTime = 3.5f;
    [Header("Life")]
    public NetworkVariable<int> life = new NetworkVariable<int>(2);
    [field: SerializeField] public int maxLife = 2;
    [Header("Hit Percent")]
    public NetworkVariable<float> hitPercent = new NetworkVariable<float>(0);
    [field: SerializeField] public float maxHitPercent = 500;

    [Header("Ref")] 
    public PlayerKnockBack playerKnockBack;
    public TMP_Text overText;
    public Color lowHp,mediumHp,highHp;
    
    [Header("Setting")]
    private bool outOfAreaCheck;
    private bool firstInArea;

    public Action<PlayerHealth> OnDie;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        life.Value = maxLife;
        outAreaTime.Value = maxOutAreaTime;

        if (!IsClient) { return; }
        outAreaTime.OnValueChanged += HandleOutOfAreaChanged;
        HandleOutOfAreaChanged(maxOutAreaTime,outAreaTime.Value);
    }

    public override void OnNetworkDespawn()
    {
        outAreaTime.OnValueChanged -= HandleOutOfAreaChanged;
    }

    private void LateUpdate()
    {
        if (!IsClient)
        {
            return;
        }
        OutOfAreaHandle();
        
    }
    

    private void OutOfAreaHandle()
    {
        if (outOfAreaCheck)
        {
            outAreaTime.Value -= Time.deltaTime;
            if (outAreaTime.Value <= 0)
            {
                OnDie?.Invoke(this);
                outOfAreaCheck = false;
            }
        }
        
    }
    private void HandleOutOfAreaChanged(float oldPercent,float newOutArea)
    {
        float overCount = newOutArea;
        if (outAreaTime.Value < 1)
        {
            overCount = Mathf.Round(overCount * 10f) * 0.1f;
            overText.text = overCount.ToString();
        }
        else if (outAreaTime.Value < maxOutAreaTime)
        {
            overCount = (int)Math.Floor(overCount);
            overText.text = overCount.ToString();
        }

    }
    public void OutOfArea()
    {
        outOfAreaCheck = true;
    }

    public void InArea()
    {
        outOfAreaCheck = false;
        outAreaTime.Value = maxOutAreaTime;
        overText.text = string.Empty;
    }

    public void ReceiveDamage(float percent,Vector2 direction)
    {
        ModifyHealth(percent);
        playerKnockBack.ActiveKnockBack(direction);
    }

    private void ModifyHealth(float value)
    {
        float newHitPoint = hitPercent.Value + value;
        if (newHitPoint >= maxHitPercent)
        {
            newHitPoint = 0;
        }
        hitPercent.Value = Mathf.Clamp(newHitPoint, 0, maxHitPercent);
    }

    public void PlayerDie()
    {
        ProCamera2D.Instance.RemoveCameraTarget(transform);
        Destroy(gameObject);
    }
}
