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
    bool active; //check if trap can attack
    bool isTrapRunning;  // check if trap is doing activation cycle
    List<IDamageable> targets = new List<IDamageable>(); // List of damamged character
    float activationCooldown = 0f; // Timer before activation
    float attackCoolDown = 0f;  // Timmer before next attack
    Animator[] spikesAnomators; // All child spike animator

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isTrapRunning = false;
        spikesAnomators = GetComponentsInChildren<Animator>();
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


        //check of press action buton activate trap if not already in activation and colldown over
        if (basicActionInput.WasPressedThisFrame() && baseStats.timeDown <= activationCooldown && !isTrapRunning)
        {
            Debug.Log("Let's activate spike trap");
            StartCoroutine(TrapCycle());
            activationCooldown = 0;
        }

        // Check if spike is activated and attack if so
        if (active && baseStats.attackCooldown <= attackCoolDown)
        {
            //Debug.Log("Let's activate attack");
            Attack();
            attackCoolDown = 0;

        }
        else if (attackCoolDown < baseStats.attackCooldown)
        {
            attackCoolDown += Time.deltaTime;
        }

    }

    //Attack all character that is in list
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
        if (collision.TryGetComponent(out IDamageable character) && targets.Contains(character))
        {
            targets.Remove(character);
        }
    }


    IEnumerator TrapCycle()
    {
        Debug.Log("<color=cyan><b>[AI]</b> Spike cycle from control </color>");
        isTrapRunning = true;

        //make all spike go up
        foreach (Animator anim in spikesAnomators)
        {
            anim.Play("SpikeUp");

        }
        active = true;
        Debug.Log(active);
        yield return new WaitForSeconds(baseStats.timeUp);

        //make all spike go down
        foreach (Animator anim in spikesAnomators)
        {
            anim.Play("SpikeDown");

        }
        active = false;
        Debug.Log(active);
        yield return new WaitForSeconds(baseStats.timeDown);

        //waiting mode
        foreach (Animator anim in spikesAnomators)
        {
            anim.Play("Idle");

        }

        isTrapRunning = false;
    }
}

