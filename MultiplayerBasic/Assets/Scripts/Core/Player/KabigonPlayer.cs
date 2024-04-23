using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Com.LuisPedroFonseca.ProCamera2D;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class KabigonPlayer : NetworkBehaviour
{
    [Header("References")]
    //[SerializeField] private CinemachineVirtualCamera virtualCamera;
    //[field:SerializeField] public Health Health { get; private set; }

    [Header("Settings")]
    [SerializeField]
    private Texture2D crossHair;
    //private int ownerPriority = 15;

    public bool haveCam;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<int> PlayerColorIndex = new NetworkVariable<int>();

    public static event Action<KabigonPlayer> OnPlayerSpawned;
    public static event Action<KabigonPlayer> OnPlayerDespawned;

    

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
            Cursor.SetCursor(crossHair,new Vector2(crossHair.width/2,crossHair.height/2),CursorMode.Auto);
        }
        
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }

    public void KabigonAdd()
    {
        if (!haveCam)
        {
            ProCamera2D.Instance.AddCameraTarget(transform);
            haveCam = true;
        }
    }
    
}


