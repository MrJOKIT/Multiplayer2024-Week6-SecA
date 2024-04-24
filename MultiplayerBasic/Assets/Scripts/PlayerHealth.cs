using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [Header("Over Time")]
    public NetworkVariable<float> outAreaTime = new NetworkVariable<float>(3.5f);
    [SerializeField] public float maxOutAreaTime = 3.5f;
    /*[Header("Life")]
    public NetworkVariable<int> life = new NetworkVariable<int>(2);
    [field: SerializeField] public int maxLife = 2;*/
    [Header("Hit Percent")]
    public NetworkVariable<float> hitPercent = new NetworkVariable<float>(0.1f);
    [SerializeField] public float maxHitPercent = 500;

    [Header("Ref")] 
    public KabigonPlayer kabigonPlayer;
    public TMP_Text overText;
    public Color lowHp,mediumHp,highHp;
    
    [Header("Setting")]
    public NetworkVariable<bool> outOfAreaCheck = new NetworkVariable<bool>(false);
    private bool firstInArea;
    private ulong clinetId;

    public Action<PlayerHealth> OnDie;

    private void Start()
    {
        clinetId = OwnerClientId;
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsClient)
        {
            return;
        }
        
        outAreaTime.OnValueChanged += HandleOutOfAreaChanged;
        outOfAreaCheck.OnValueChanged += OutOfAreaHandle;
        HandleOutOfAreaChanged(maxOutAreaTime,outAreaTime.Value);
        
    }

    public override void OnNetworkDespawn()
    {
        if (!IsClient)
        {
            return;
        }
        outAreaTime.OnValueChanged -= HandleOutOfAreaChanged;
        outOfAreaCheck.OnValueChanged -= OutOfAreaHandle;
    }

    private void LateUpdate()
    {
        if (!IsClient)
        {
            return;
        }
        OutOfAreaHandle(false,outOfAreaCheck.Value);
    }
    

    private void OutOfAreaHandle(bool oldCheck,bool newCheck)
    {
        OutOfAreaUpdateClientRpc();
    }
    private void HandleOutOfAreaChanged(float oldPercent,float newOutArea)
    {
        float overCount = newOutArea;
        if (outAreaTime.Value < 1)
        {
            overCount = Mathf.Round(overCount * 10f) * 0.1f;
            overText.text = overCount.ToString(CultureInfo.CurrentCulture);
        }
        else if (outAreaTime.Value < maxOutAreaTime)
        {
            overCount = (int)Math.Floor(overCount);
            overText.text = overCount.ToString(CultureInfo.CurrentCulture);
        }

    }
    public void OutOfArea()
    {
        outOfAreaCheck.Value = true;
        ScoreManager.instance.InitializeName(OwnerClientId);
    }

    public void InArea()
    {
        outOfAreaCheck.Value = false;
        
    }

    public void ReceiveDamage(float percent)
    {
        ModifyHealth(percent);
        
    }

    private void ModifyHealth(float value)
    {
        float newHitPoint = hitPercent.Value + value;
        if (newHitPoint >= maxHitPercent)
        {
            newHitPoint = 0;
        }
        hitPercent.Value = Mathf.Clamp(newHitPoint, 0, maxHitPercent);
    }

    public void PlayerDie()
    {
        ScoreManager.instance.Initialized(clinetId);
        ProCamera2D.Instance.RemoveCameraTarget(transform);
        RespawnHandler.instance.HandlePlayerDespawned(kabigonPlayer);
    }
    
    [ClientRpc]
    private void OutOfAreaUpdateClientRpc()
    {
        if (outOfAreaCheck.Value)
        {
            outAreaTime.Value -= Time.deltaTime;
            if (outAreaTime.Value <= 0)
            {
                OnDie?.Invoke(this);
                PlayerDie();
            }
        }
        else
        {
            outAreaTime.Value = Mathf.Clamp(maxOutAreaTime, 0, maxOutAreaTime);
            overText.text = string.Empty;
        }
    }

    [ClientRpc]
    public void ApplyKnockBackClientRpc(float damage,Vector2 knockBackDirection)
    {
       
        float knockBackMagnitude = hitPercent.Value + damage * 0.4f; 

        Rigidbody2D playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            playerRigidbody.AddForce(knockBackDirection.normalized * knockBackMagnitude, ForceMode2D.Impulse); 
            StartCoroutine(StopKnockBack(playerRigidbody));
        }
    }

    private IEnumerator StopKnockBack(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.1f); // ระยะเวลา
        rb.velocity = Vector2.zero;
    }

}
