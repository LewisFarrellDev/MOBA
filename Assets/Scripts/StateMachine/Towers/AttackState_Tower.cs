using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Tower : BaseState
{
    public float damage;
    public float attackRate;
    public GameObject projectile;

    private List<GameObject> activeProjectiles = new List<GameObject>();

    private GameObject currentTarget;

    private float lastAttack;

    public override void OnBeginState(StateManager stateManager)
    {
        this.stateManager = stateManager;
        stateDescription = "Attacking...";
    }

    public override void OnEndState()
    {
        for (int i = activeProjectiles.Count -1; i >= 0; i--)
        {
            Destroy(activeProjectiles[i]);
            activeProjectiles.RemoveAt(i);
        }
    }

    public override void UpdateState()
    {
        currentTarget = stateManager.GetComponent<StateManager_Tower>().GetTarget();
        if (currentTarget == null)
        {
            // clean up any projectiles before switching states
            foreach (GameObject item in activeProjectiles)
            {
                Destroy(item);
            }
            activeProjectiles = new List<GameObject>();

            stateManager.ChangeState(stateManager.GetComponent<StateManager_Tower>().idleState);
            return;
        }

        // calculate top of tower
        Vector3 towerTop = transform.Find("Mesh").position;
        towerTop.y = towerTop.y * 2;
        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + towerTop.y, transform.position.z);

        // Draw line to current Target
        Debug.DrawLine(spawnLocation, currentTarget.transform.position, Color.red);

        // If we havnt fired yet within the set fireate
        if (Time.time > lastAttack + attackRate)
        {
            // Spawn the bullet
            lastAttack = Time.time;
            GameObject prefab = Instantiate(projectile, spawnLocation, transform.rotation);

            // Store a list of projectiles
            activeProjectiles.Add(prefab);

            // Play SFX
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Play();

            if (currentTarget.GetComponent<Entity>().TakeDamage(damage))
            {
                stateManager.GetComponent<StateManager_Tower>().RemoveTarget(currentTarget);
                GameObject.Destroy(currentTarget);
            }

        }


        foreach (GameObject item in activeProjectiles)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, currentTarget.transform.position, 50 * Time.deltaTime);

            if (Vector3.Distance(item.transform.position, currentTarget.transform.position) <= 0.5f)
            {
                activeProjectiles.Remove(item);
                Destroy(item);
                break;
            }
        }
    }
}
