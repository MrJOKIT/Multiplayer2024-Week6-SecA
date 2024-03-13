using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Skill
{
    public string skillName;
    public Transform vfxAttack;
}
public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private Skill[] playerSkill;
    [SerializeField] private float delayAttack;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform sfxSpawn;
    [SerializeField] private float attackRadius;
    [SerializeField] private Collider2D playerCollider;
    private float timer;
    private bool onAttack;
    private bool inDelayAttack;

    private float delayAttackTimeCounter;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent += Attack;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.PrimaryFireEvent -= Attack;
    }

    private void Attack(bool onAttack)
    {
        this.onAttack = onAttack;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        if (timer>0)
        {
            timer -= Time.deltaTime;
        }

        if (!onAttack)
        {
            return;
        }
        if (!IsOwner)
        {
            return;
        }
        if(timer > 0){ return; }
        Debug.Log("Attack");
        ClientAttack();
        
        timer = delayAttack;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
    
    private void ClientAttack()
    {
        Transform damageItem = Instantiate(playerSkill[0].vfxAttack, sfxSpawn.position, sfxSpawn.rotation, sfxSpawn.parent);
        Physics2D.IgnoreCollision(playerCollider,damageItem.GetComponent<Collider2D>());
    }
}
