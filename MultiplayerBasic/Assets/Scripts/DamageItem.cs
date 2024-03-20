using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    [SerializeField] private Collider2D hitBox;
    
    public void ActiveDamage()
    {
        hitBox.enabled = true;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
