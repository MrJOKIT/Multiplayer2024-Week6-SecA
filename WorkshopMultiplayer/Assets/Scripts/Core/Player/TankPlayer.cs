using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class TankPlayer : NetworkBehaviour
{
    [Header("References")] [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private Texture2D crossHair;
    [field:SerializeField] public Health Health { get; private set; }

    [Header("Settings")] [SerializeField] 
    private int ownerPriority = 15;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<int> PlayerColorIndex = new NetworkVariable<int>();

    public static event Action<TankPlayer> OnPlayerSpawned;
    public static event Action<TankPlayer> OnPlayerDespawned;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = 
                HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            PlayerName.Value = userData.userName;
            PlayerColorIndex.Value = userData.userColorIndex;
            
            OnPlayerSpawned?.Invoke(this);
        }
        
        if (IsOwner)
        {
            virtualCamera.Priority = ownerPriority;

            Cursor.SetCursor(crossHair, new Vector2(crossHair.width / 2, crossHair.height / 2), CursorMode.Auto);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }
}


