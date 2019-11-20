using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_Scuttle : StateManager
{
    [HideInInspector]
    public PatrolState_Scuttle patrolState;
    private FleeState_Scuttle fleeState;

    private GameObject enemy;
    public float damage;

    void Start()
    {
        patrolState = GetComponent<PatrolState_Scuttle>();
        fleeState = GetComponent<FleeState_Scuttle>();
        ChangeState(patrolState);
    }

    void OnTriggerEnter(Collider other)
    {
        Entity entityOther = other.GetComponent<Entity>();
        if (entityOther != null)
        {
            enemy = other.gameObject;
            // 40% chance to run away on contact with enemy
            if (Random.Range(0.0f, 1.0f) <= 0.4f)
            {
                fleeState.SetFleeTarget(patrolState.patrolPoints, other.gameObject);
                ChangeState(fleeState);
            }
        }
    }

    public void FuzzyLogic()
    {
        float random = Random.Range(0.0f, 1.0f);

        // 10% chance to flee away
        if (random > 0 && random < 0.1f)
        {
            fleeState.SetFleeTarget(patrolState.patrolPoints, enemy);
            ChangeState(fleeState);
        }
        // 20% chance to do damage back to the attacker
        else if (random > 0.1f && random < 0.3f)
        {
            Entity entityOther = enemy.GetComponent<Entity>();
            if (entityOther != null)
                entityOther.TakeDamage(damage);
        }
        // 70% chance to carry on patrolling
    }
}

