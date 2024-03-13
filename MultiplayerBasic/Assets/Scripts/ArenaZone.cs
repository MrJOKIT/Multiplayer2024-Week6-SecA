using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<KabigonPlayer>().KabigonAdd();
            col.GetComponent<PlayerHealth>().InArea();
        }
        else if (col.CompareTag("Dummy"))
        {
            col.GetComponent<DummyHealth>().InArea();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().OutOfArea();
        }
        else if (other.CompareTag("Dummy"))
        {
            other.GetComponent<DummyHealth>().OutOfArea();
        }
    }
}
