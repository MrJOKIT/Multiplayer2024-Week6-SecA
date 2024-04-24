using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    //[SerializeField] private float turningRate = 30f;

    private Vector2 previousMovementInput;
    private bool onDash;

    [SerializeField] private float delayDash;
    private float timer;
    
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    //private float dashTimer = 0f;
    private bool isDashing;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        inputReader.MoveEvent += HandleMove;
        inputReader.PrimaryDashEvent += HandleDash;
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner) { return; }
        inputReader.MoveEvent -= HandleMove;
        inputReader.PrimaryDashEvent -= HandleDash;
    }
    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) { return ; }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer > 0)
        {
            return;
        }

        if (isDashing)
        {
            return;
        }
        if (!onDash)
        {
            return;
        }
        PrimaryDash();

        timer = delayDash;
        
    }

    private void FixedUpdate()
    {
        if (!IsOwner) { return ; }

        Vector3 movement = new Vector3(previousMovementInput.x, previousMovementInput.y);

        transform.position += movement * movementSpeed * Time.deltaTime;
    }
    
    private void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    private void HandleDash(bool dashInput)
    {
        onDash = dashInput;
    }

    private void PrimaryDash()
    {
        StartCoroutine(Dash());
        Debug.Log("Dash");
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        float dashTimer = dashDuration;
        Vector2 dashDirection = previousMovementInput.normalized;
        rb.velocity = dashDirection * dashDistance / dashDuration;

        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }
}
