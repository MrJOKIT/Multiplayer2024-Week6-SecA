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
    public NetworkVariable<float> outAreaTime = new NetworkVariable<float>();
    [field: SerializeField] public float maxOutAreaTime = 3.5f;
    [Header("Life")]
    public NetworkVariable<int> life = new NetworkVariable<int>();
    [field: SerializeField] public int maxLife = 2;
    [Header("Hit Percent")]
    public NetworkVariable<float> hitPercent = new NetworkVariable<float>();

    [Header("Ref")] 
    public TMP_Text overText;
    public TMP_Text lifeText;
    public TMP_Text hitText;
    public Color lowHp,mediumHp,highHp;
    
    [Header("Setting")]
    private float outAreaTimeCounter;
    private bool outOfAreaCheck;
    private bool firstInArea;
    
    

    public override void OnNetworkSpawn()
    {
        life.Value = maxLife;
        outAreaTime.Value = maxOutAreaTime;
        HandleHitPercentChanged(0,hitPercent.Value);

        if (!IsOwner)
        {
            return;
        }
        HandleLifeChanged(0,life.Value);
    }

    private void Update()
    {
        OutOfAreaHandle();
        hitPercent.OnValueChanged += HandleHitPercentChanged;
        life.OnValueChanged += HandleLifeChanged;
    }

    private void OutOfAreaHandle()
    {
        if (outOfAreaCheck)
        {
            outAreaTimeCounter -= Time.deltaTime;
            if (outAreaTimeCounter < 0)
            {
                PlayerDie();
                outAreaTimeCounter = outAreaTime.Value;
                outOfAreaCheck = false;
            }
            else if (outAreaTimeCounter < 1)
            {
                float overCount = outAreaTimeCounter;
                overCount = Mathf.Round(overCount * 10f) * 0.1f;
                overText.text = overCount.ToString();
            }
            else if (outAreaTimeCounter < outAreaTime.Value)
            {
                int overCount = (int)Math.Floor(outAreaTimeCounter);
                overText.text = overCount.ToString();
            }
        }
    }
    
    private void HandleHitPercentChanged(float oldPercent,float newPercent)
    {
        float hitPercent = newPercent;
        hitPercent = Mathf.Round(hitPercent * 10f) * 0.1f;
        hitText.text = hitPercent.ToString(CultureInfo.InvariantCulture);
        if (hitPercent > 40)
        {
            hitText.color = mediumHp;
        }
        else if (hitPercent > 80)
        {
            hitText.color = highHp;
        }
        else
        {
            hitText.color = lowHp;
        }

        
    }
    
    private void HandleLifeChanged(int oldCount,int newCount)
    {
        lifeText.text = "LIFE: " + newCount;
    }
    
    /*private void HandleOverTimeChanged(float oldTime,float newTime)
    {
        overText.text = newTime.ToString(CultureInfo.InvariantCulture);
    }*/

    public void OutOfArea()
    {
        outOfAreaCheck = true;
    }

    public void InArea()
    {
        outOfAreaCheck = false;
        outAreaTimeCounter = outAreaTime.Value;
        overText.text = string.Empty;
    }

    public void PlayerDie()
    {
        ProCamera2D.Instance.RemoveCameraTarget(transform);
        Destroy(gameObject);
    }
}
