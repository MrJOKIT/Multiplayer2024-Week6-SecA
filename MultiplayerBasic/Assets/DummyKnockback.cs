using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackCooldown = 0.5f;

    private Rigidbody2D rb;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private float knockbackCooldownTimer = 0f;

    private DummyHealth dummyHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dummyHealth = GetComponent<DummyHealth>();
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
            rb.velocity = direction * knockbackForce * dummyHealth.hitPercent.Value / 75;
        }
    }
}
