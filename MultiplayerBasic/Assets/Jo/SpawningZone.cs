using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs; 
    [SerializeField] private Collider2D spawnArea;
    [SerializeField] private float spawnCooldown = 10f;

   

    public override void OnNetworkSpawn()
    {
        if(!IsServer) { return; }

        StartCoroutine(SpawnItemsSequentially());
    }
    private IEnumerator SpawnItemsSequentially()
    {
        while (true)
        {
            SpawnItem();
            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    

    private Vector2 GetRandomPointInCollider()
    {
        float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float randomY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        return new Vector2(randomX, randomY);
    }

   
   private void SpawnItem()
   {
       if (itemPrefabs.Length == 0)
       {
           Debug.LogError("No item prefabs assigned.");
           return;
       }

       int randomIndex = Random.Range(0, itemPrefabs.Length);
       Vector2 spawnPoint = GetRandomPointInCollider();
       GameObject itemInstance = Instantiate(itemPrefabs[randomIndex], spawnPoint, Quaternion.identity);

       itemInstance.GetComponent<NetworkObject>().Spawn();
   }
}