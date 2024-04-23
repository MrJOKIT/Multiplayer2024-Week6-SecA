using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private float damage = 20;

    private ulong ownerClientId;
    public void SetOwner(ulong ownerClientId)
    {
        this.ownerClientId = ownerClientId;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.attachedRigidbody == null) {  return; }

        if(col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if(ownerClientId == netObj.OwnerClientId)
            {
                return;
            }
        }

        if(col.attachedRigidbody.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            Vector2 knockBackDirection = (col.transform.position - transform.position).normalized;
            playerHealth.ReceiveDamage(damage);
        }
    }
}
