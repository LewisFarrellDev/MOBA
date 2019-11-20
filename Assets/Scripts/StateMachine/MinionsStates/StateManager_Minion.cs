using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager_Minion : StateManager
{
    // States
    [HideInInspector]
    public FindPathState_Minion findPathState;
    [HideInInspector]
    public AttackState_Minion attackState;
    [HideInInspector]
    public ChaseState_Minion chaseState;

    // Other scripts
    private Entity entity;

    // Use this for initialization
    void Start()
    {
        entity = GetComponent<Entity>();

        findPathState = GetComponent<FindPathState_Minion>();
        chaseState = GetComponent<ChaseState_Minion>();
        attackState = GetComponent<AttackState_Minion>();

        ChangeState(findPathState);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if we collided with a valid target
        Entity otherEntity = other.gameObject.GetComponent<Entity>();

        // If we did...
        if (otherEntity != null && otherEntity.team != entity.team)
        {
            targetList.Add(other.gameObject);

            if (currentState != chaseState)
                ChangeState(chaseState);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if we collided with a valid target
        Entity otherEntity = other.GetComponent<Entity>();

        // If we did...
        if (otherEntity != null && otherEntity.team != entity.team)
        {
            RemoveTarget(other.gameObject);
        }
    }

    public void RemoveTarget(GameObject obj)
    {
        targetList.Remove(obj);

        if (targetList.Count == 0)
            ChangeState(findPathState);
        else
            ChangeState(chaseState);
    }
}
