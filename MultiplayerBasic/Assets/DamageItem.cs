using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageItem : NetworkBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] Collider2D hitbox;
    
    private ulong ownerClientId;
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Dummy"))
        {
            Vector2 knockbackDirection = (col.transform.position - transform.position).normalized;
            col.GetComponent<DummyHealth>().ReceiveDamage(damage,knockbackDirection);
        }

        if(col.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (col.transform.position - transform.position).normalized;
            col.GetComponent<PlayerHealth>().ReceiveDamage(damage,knockbackDirection);
        }
    }
    

    public void ActiveDamage()
    {
        hitbox.enabled = true;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
