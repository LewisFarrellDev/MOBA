using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState_Scuttle : BaseState
{
    GameObject fleeTarget;
    public float fleeSpeed = 5;

    public override void OnBeginState(StateManager stateManager)
    {
        stateDescription = "Fleeing...";
        this.stateManager = stateManager;
    }

    public override void OnEndState()
    {
        //throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        transform.position = Vector3.MoveTowards(transform.position, fleeTarget.transform.position, fleeSpeed * Time.deltaTime);
        transform.LookAt(fleeTarget.transform.position);

        if (Vector3.Distance(transform.position, fleeTarget.transform.position) <= 0.5f)
        {
            stateManager.ChangeState(stateManager.GetComponent<StateManager_Scuttle>().patrolState);
        }
    }

    public void SetFleeTarget(List<GameObject> potentialTargets, GameObject targetToFleeFrom)
    {
        fleeTarget = potentialTargets[0];
        foreach (GameObject target in potentialTargets)
        {
            if (Vector3.Distance(targetToFleeFrom.transform.position, target.transform.position) > Vector3.Distance(fleeTarget.transform.position, targetToFleeFrom.transform.position))
            {
                fleeTarget = target;
            }
        }
    }
}
