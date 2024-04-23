using System;
using System.Collections;
using System.Globalization;
using Com.LuisPedroFonseca.ProCamera2D;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [Header("Over Time")]
    public NetworkVariable<float> outAreaTime = new NetworkVariable<float>(3.5f);
    [SerializeField] public float maxOutAreaTime = 3.5f;
    
    [Header("Hit Percent")]
    public NetworkVariable<float> hitPercent = new NetworkVariable<float>(0);
    [SerializeField] public float maxHitPercent = 500;

    [Header("Ref")] 
    public KabigonPlayer kabigonPlayer;
    public TMP_Text overText;
    public Color lowHp, mediumHp, highHp;
    
    [Header("Setting")]
    public NetworkVariable<bool> outOfAreaCheck = new NetworkVariable<bool>(false);

    private ulong clientId;
    
    public Action<PlayerHealth> OnDie;

    private void Start()
    {
        clientId = OwnerClientId;
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
            overCount = (int)Mathf.Floor(overCount);
            overText.text = overCount.ToString(CultureInfo.CurrentCulture);
        }
    }

    public void OutOfArea()
    {
        outOfAreaCheck.Value = true;
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
        ScoreManager.instance.Initialized(clientId);
        ProCamera2D.Instance.RemoveCameraTarget(transform);
        RespawnHandler.instance.HandlePlayerDespawned(kabigonPlayer);
    }

   
    [ClientRpc]
    public void ApplyKnockBackClientRpc(Vector2 knockBackDirection)
    {
       
        float knockBackMagnitude = hitPercent.Value * 0.7f; 

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
