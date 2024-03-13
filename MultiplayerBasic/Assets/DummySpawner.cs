using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    public GameObject dummyPrefab;
    public GameObject dummyInGame;
    public Transform spawnPoint;

    private void FixedUpdate()
    {
        if (dummyInGame == null)
        {
            dummyInGame = Instantiate(dummyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
