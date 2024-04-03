using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    [Header("References")] 
    [SerializeField] private Health health;
    [SerializeField] private BountryCoin coinPrefab;

    [Header("Settings")] 
    [SerializeField] private float coinSpread = 3f;
    [SerializeField] private float bountyPercentage = 50f;
    [SerializeField] private int bountyCoinCount = 10;
    [SerializeField] private int minBountyCoinValue = 5;
    [SerializeField] private LayerMask layerMask;

    private Collider2D[] coinBuffer = new Collider2D[1];

    private float coinRadius;
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        coinRadius = coinPrefab.GetComponent<CircleCollider2D>().radius;
        health.OnDie += HandleDie;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            return;
        }

        health.OnDie -= HandleDie;
    }

    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    public void SpendCoin(int costToFire)
    {
        TotalCoins.Value -= costToFire;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Coin>(out Coin coin)) {  return; }

        int coinValue = coin.Collect();

        if(!IsServer) { return; }

        TotalCoins.Value += coinValue;
    }

    private void HandleDie(Health health)
    {
        int bountyValue = (int) (TotalCoins.Value * (bountyPercentage / 100f));
        int bountyCoinValue = bountyValue / bountyCoinCount;
        if (bountyCoinValue < minBountyCoinValue)
        {
            return;
        }

        for (int i = 0; i < bountyCoinCount; i++)
        {
            BountryCoin coinInstance = Instantiate(coinPrefab, GetSpawnPoint(), Quaternion.identity);
            coinInstance.SetValue(bountyCoinValue);
            coinInstance.NetworkObject.Spawn();
        }
    }

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            Vector2 spawnPoint = (Vector2) transform.position + UnityEngine.Random.insideUnitCircle * coinSpread;
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.layerMask = layerMask;
            int numColliders = Physics2D.OverlapCircle(spawnPoint, coinRadius, contactFilter2D, coinBuffer);
            if (numColliders == 0)
            {
                return spawnPoint;
            }
        }
    }
}