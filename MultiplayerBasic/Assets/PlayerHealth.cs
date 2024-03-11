using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float hitPercent;
    public float outAreaTime;
    private float outAreaTimeCounter;
    private bool outOfAreaCheck;

    private void Update()
    {
        if (outOfAreaCheck)
        {
            outAreaTimeCounter += Time.deltaTime;
            if (outAreaTimeCounter > outAreaTime)
            {
                PlayerDie();
                outAreaTimeCounter = 0f;
                outOfAreaCheck = false;
            }
        }
        
    }

    public void OutOfArea()
    {
        outOfAreaCheck = true;
    }

    public void InArea()
    {
        outOfAreaCheck = false;
        outAreaTimeCounter = 0f;
    }

    public void PlayerDie()
    {
        Destroy(gameObject);
    }
}
