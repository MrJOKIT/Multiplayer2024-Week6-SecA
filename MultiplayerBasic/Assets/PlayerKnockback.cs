using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerKnockback : NetworkBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackCooldown = 0.5f;

    private Rigidbody2D rb;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackCooldownTimer = 0f;

    private PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
    }

    private void Update()
    {
        if (isKnockback)
        {
            if (knockbackTimer <= 0)
            {
                isKnockback = false;
                rb.velocity = Vector2.zero;
            }
            else
            {
                knockbackTimer -= Time.deltaTime;
            }
        }
        else
        {
            if (knockbackCooldownTimer <= 0)
            {
                // Handle player movement input here
            }
            else
            {
                knockbackCooldownTimer -= Time.deltaTime;
            }
        }
    }

    public void Knockback(Vector2 direction)
    {
        if (!isKnockback)
        {
            isKnockback = true;
            knockbackTimer = knockbackDuration;
            knockbackCooldownTimer = knockbackCooldown;
            rb.velocity = direction * knockbackForce * playerHealth.hitPercent.Value / 75;
        }
    }
}
