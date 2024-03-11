using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    
    [SerializeField] private NetworkObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        KabigonPlayer[] players = FindObjectsByType<KabigonPlayer>(FindObjectsSortMode.None);
        foreach (KabigonPlayer player in players)
        {
            HandlePlayerSpawned(player);
            //ProCamera2D.Instance.AddCameraTarget(player.transform);
        }

        KabigonPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        KabigonPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }

        KabigonPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        KabigonPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(KabigonPlayer player)
    {
        //player.Health.OnDie += (health) => HandlePlayerDie(player);
        //ProCamera2D.Instance.AddCameraTarget(player.transform);
    } 

    private void HandlePlayerDespawned(KabigonPlayer player)
    {
        //player.Health.OnDie -= (health) => HandlePlayerDie(player);
        ProCamera2D.Instance.RemoveCameraTarget(player.transform);
    }

    private void HandlePlayerDie(KabigonPlayer player)
    {
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;

        NetworkObject playerInstance = Instantiate(
            playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        
        playerInstance.SpawnAsPlayerObject(ownerClientId);
        //ProCamera2D.Instance.AddCameraTarget(playerInstance.transform);
        
    }
}
