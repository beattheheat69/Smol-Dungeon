using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SpikeAI : TrapAI
{
    [SerializeField]
    SpikeStats_SO baseStats; // Base stats of spike
    bool active;
    List<IDamageable> targets = new List<IDamageable>();
    float timeCooldown; //Time that passes before next attack

    void Start()
    {
        StartCoroutine(TrapCycle());
    }

    private void Update()
    {
        if (active && targets.Count > 0 && timeCooldown <= 0)
        {
            foreach (IDamageable character in targets)
            {
                Debug.Log("spike attack ");
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

    IEnumerator TrapCycle()
    {
        while (true)
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
}
