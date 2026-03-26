using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpikeControl : TrapAction
{
    [SerializeField]
    SpikeStats_SO baseStats;
    PlayerInput playerInput;
    InputAction basicActionInput;
    bool active;
    List<IDamageable> targets = new List<IDamageable>();
    float activationCooldown = 0f;
    float attackCoolDown = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        active = false;
        playerInput = GetComponent<PlayerInput>();
        basicActionInput = playerInput.actions["BasicAction"];
    }

    // Update is called once per frame
    void Update()
    {
        //Cooldown timer
        if (activationCooldown < baseStats.timeDown) //Maybe individual cooldowns for each attack?
            activationCooldown += Time.deltaTime;

        if (basicActionInput.WasPressedThisFrame() && baseStats.timeDown <= activationCooldown)
        {
            Debug.Log("Let's activate spike trap");
            StartCoroutine(TrapCycle());
            activationCooldown = 0;
        }

        if (active && baseStats.attackCooldown <= attackCoolDown)
        {
            Debug.Log("Let's activate attack");
            Attack();
            attackCoolDown = 0;

        }
        else if (attackCoolDown < baseStats.attackCooldown)
        {
            attackCoolDown += Time.deltaTime;
        }

    }


    void Attack()
    {
        foreach (IDamageable character in targets)
        {
            Debug.Log("spike attack ");
            character.takeDamage(baseStats.power, transform.position, baseStats.kockbackForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable character) && !targets.Contains(character))
        {
            targets.Add(character);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable character))
        {
            targets.Remove(character);
        }
    }


    IEnumerator TrapCycle()
    {
        foreach (Transform anim in this.transform)
        {
            //do animation up

        }
        active = true;
        Debug.Log(active);
        yield return new WaitForSeconds(baseStats.timeUp);

        foreach (Transform anim in this.transform)
        {
            //do animation down

        }
        active = false;
        Debug.Log(active);
        yield return new WaitForSeconds(baseStats.timeDown);
    }
}

