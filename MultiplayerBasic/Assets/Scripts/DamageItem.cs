using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DamageItem : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    private void Update()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
