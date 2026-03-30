using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

public class SpikeAI : TrapAI
{
    [SerializeField]
    SpikeStats_SO baseStats; // Base stats of spike
    bool active;
    bool isPossesed;
    List<IDamageable> targets = new List<IDamageable>();
    float timeCooldown; //Time that passes before next attack
    //Animator[] spikesAnomators;
    Animator anim;

    void Start()
    {
        isPossesed = false;
        //spikesAnomators = GetComponentsInChildren<Animator>();
        StartCoroutine(TrapCycle());
        anim = GetComponent<Animator>();
    }

	private void Update()
    {
        if (active && targets.Count > 0 && timeCooldown <= 0)
        {
            foreach (IDamageable character in targets)
            {
                character.takeDamage(baseStats.power, transform.position, baseStats.kockbackForce);
            }

            timeCooldown = baseStats.attackCooldown;
        }

        //attack cooldown
        if (timeCooldown > 0)
        {
            timeCooldown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable character))
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

    public void SetisPossesed(bool state)
    { 
        isPossesed = state;
        if (!isPossesed) //To restart TrapCycle when depossessing
            StartCoroutine(TrapCycle()); //This causes the coroutine to be called 999+ times even at the start and even skip the timers in the coroutine...?
    }

    IEnumerator TrapCycle()
    {
        while (!isPossesed)
        {
			yield return new WaitForSeconds(baseStats.timeUp);
			anim.Play("SpikeBundle_GoingUp");
            active = true;
			yield return new WaitForSeconds(baseStats.timeDown);
            anim.Play("SpikeBundle_GoingDown");
            active = false;
			yield return new WaitForSeconds(baseStats.attackCooldown);
            anim.Play("SpikeBundle_Idle");

            yield return null;
			Debug.Log("<color=red><b>[AI]</b> Spike cycle from AI </color>");

            ////make all spike go up
            //foreach (Animator anim in spikesAnomators)
            //{
            //    anim.Play("SpikeUp");

            //}
            //active = true;
            //yield return new WaitForSeconds(baseStats.timeUp);

            ////make all spike go down
            //foreach (Animator anim in spikesAnomators)
            //{
            //    anim.Play("SpikeDown");

            //}
            //active = false;
            //yield return new WaitForSeconds(baseStats.timeDown);


            ////waiting mode
            //foreach (Animator anim in spikesAnomators)
            //{
            //    anim.Play("Idle");

            //}
        }
    }
}
