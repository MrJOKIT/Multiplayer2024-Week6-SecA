using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform weaponTransform;

    private void LateUpdate()
    {
        if(!IsOwner) { return; }

        Vector2 aimScreenPosition = inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        weaponTransform.up = new Vector2(
            aimWorldPosition.x - weaponTransform.position.x,
            aimWorldPosition.y - weaponTransform.position.y);
    }
}
