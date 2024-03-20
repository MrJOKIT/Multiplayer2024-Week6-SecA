using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKnockBack : NetworkBehaviour
{
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

    public Rigidbody2D rb;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner) { return; }
    }
    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
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

    public void ActiveKnockBack(Vector2 direction)
    {
        KnockBack(direction);
    }
    private void KnockBack(Vector2 direction)
    {
        if (isKnockBack) return;
        isKnockBack = true;
        knockBackTimer = knockBackDuration.Value; 
        knockBackCooldownTimer = knockBackCooldown.Value;
        rb.velocity = direction * knockBackForce.Value ;
    }
}
