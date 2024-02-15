using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;
    private HostGameManager gameManager;

    public static HostSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                return instance;
            }

            instance = FindFirstObjectByType<HostSingleton>();

            if (instance == null)
            {
                Debug.LogError("No HostSingleton in the scene!");
                return null;
            }

            return instance;
        }
    }

    private void Start()
    {
        
    }

    public async Task CreateClient()
    {
        gameManager = new HostGameManager();
        await gameManager.InitAsync();
    }
    
}


