using System;
using System.Collections;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine;

public class CheckerMan : MonoBehaviour
{
    public string tagName;


    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(tagName))
        {
            Debug.Log("CheckMan Entry");
            //ใช้เพื่อให้รู้ว่ารอดเข้ามาในสังเวียน
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(tagName))
        {
            Debug.Log("CheckMan Stay");
            //ใช้เพื่อให้รู้ว่ายังอยู่ในสังเวียน
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagName))
        {
            Debug.Log("CheckMan Exit");
            //เริ่มนับถอยหลังการออกจากสังเวียน
        }
    }
}
