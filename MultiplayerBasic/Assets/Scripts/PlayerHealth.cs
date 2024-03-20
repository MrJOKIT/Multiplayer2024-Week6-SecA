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
    [field: SerializeField] public float maxHitPercent = 500;

    [Header("Ref")] 
    public Rigidbody2D rb;
    public TMP_Text overText;
    public TMP_Text lifeText;
    public TMP_Text hitText;
    public Color lowHp,mediumHp,highHp;
    
    [Header("KnockBack Setting")]
    public NetworkVariable<float> knockBackForce = new NetworkVariable<float>(10f);
    [field: SerializeField] public float maxKnockBackForce = 10f;
    public NetworkVariable<float> knockBackDuration = new NetworkVariable<float>(0.2f);
    [field: SerializeField] public float maxKnockBackDuration = 0.2f;
    public NetworkVariable<float> knockBackCooldown = new NetworkVariable<float>(0.5f);
    [field: SerializeField] public float maxKnockBackCooldown = 0.5f;
    
    private bool isKnockBack = false;
    private float knockBackTimer = 0f;
    private float knockBackCooldownTimer;
    
    [Header("Setting")]
    private float outAreaTimeCounter;
    private bool outOfAreaCheck;
    private bool firstInArea;
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }
        life.Value = maxLife;
        outAreaTime.Value = maxOutAreaTime;
        HandleHitPercentChanged(0,hitPercent.Value);
        HandleLifeChanged(0,life.Value);
    }
    
    public override void OnNetworkDespawn()
    {
        hitPercent.OnValueChanged -= HandleHitPercentChanged;
        life.OnValueChanged -= HandleLifeChanged;
        //Destroy(gameObject);
    }

    private void Update()
    {
        OutOfAreaHandle();
        hitPercent.OnValueChanged += HandleHitPercentChanged;
        life.OnValueChanged += HandleLifeChanged;
        if (hitPercent.Value > 80)
        {
            hitText.color = highHp;
        }
        else if (hitPercent.Value > 40)
        {
            hitText.color = mediumHp;
        }
        else
        {
            hitText.color = lowHp;
        }
        
        if (!isKnockBack) return;
        if (knockBackTimer <= 0)
        {
            isKnockBack = false;
            rb.velocity = Vector2.zero;
        }
        else
        {
            knockBackTimer -= Time.deltaTime;
        }
    }
    private void KnockBack(Vector2 direction)
    {
        if (isKnockBack) return;
        isKnockBack = true;
        knockBackTimer = knockBackDuration.Value; 
        knockBackCooldownTimer = knockBackCooldown.Value;
        rb.velocity = direction * knockBackForce.Value ;
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
        float calHitPercent = newPercent;
        calHitPercent = Mathf.Round(calHitPercent * 10f) * 0.1f;
        hitText.text = calHitPercent.ToString(CultureInfo.InvariantCulture);
    }
    
    private void HandleLifeChanged(int oldCount,int newCount)
    {
        lifeText.text = "LIFE: " + newCount;
    }

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

    public void ReceiveDamage(float percent,Vector2 direction)
    {
        ModifyHealth(percent);
        KnockBack(direction);
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
