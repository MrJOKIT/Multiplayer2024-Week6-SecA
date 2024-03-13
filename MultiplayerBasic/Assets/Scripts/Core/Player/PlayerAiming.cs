using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform headTransform;
    //[SerializeField] private Transform bodyTransform;

    private void LateUpdate()
    {
        if(!IsOwner) { return; }

        Vector2 aimScreenPosition = inputReader.AimPosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        headTransform.up = new Vector2(
            aimWorldPosition.x - headTransform.position.x,
            aimWorldPosition.y - headTransform.position.y);

        if (aimWorldPosition.x > headTransform.position.x)
        {
            headTransform.localScale = new Vector3(1,headTransform.localScale.y,headTransform.localScale.z);
            //headTransform.rotation = Quaternion.Euler(headTransform.rotation.x,0, headTransform.rotation.z);
        }
        else if (aimWorldPosition.x < headTransform.position.x)
        {
            headTransform.localScale = new Vector3(-1,headTransform.localScale.y,headTransform.localScale.z);
            //headTransform.rotation = Quaternion.Euler(headTransform.rotation.x, 180, headTransform.rotation.z);
        }
    }
}
