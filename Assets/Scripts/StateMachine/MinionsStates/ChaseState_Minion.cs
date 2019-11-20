using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_Minion : BaseState
{
    public float chaseSpeed = 2;
    public float minDistanceToTargetBeforeRotation = 5;
    public float acceleration = 2;
    public float rotationSpeed = 2;

    public float distanceToAttack = 2;

    private GameObject target;

    public override void OnBeginState(StateManager stateManager)
    {
        this.stateManager = stateManager;
        stateDescription = "Chasing...";
    }

    public override void OnEndState()
    {
        
    }

    public override void UpdateState()
    {
        target = stateManager.GetComponent<StateManager_Minion>().GetTarget();
        if (TargetNullCheck(target))
            return;

        if (Vector3.Distance(transform.position, target.transform.position) < distanceToAttack)
        {
            stateManager.GetComponent<StateManager_Minion>().ChangeState(stateManager.GetComponent<StateManager_Minion>().attackState);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, chaseSpeed * Time.deltaTime);

            /*

            // Smooth move with de-acceleration 
            speedDistanceScale = Mathf.Lerp(speedDistanceScale, Mathf.Clamp01((Vector3.Distance(transform.position, target.transform.position) / (minDistanceToTargetBeforeRotation * 2))), acceleration * Time.deltaTime);

            // Always move the agent forward
            transform.position += transform.forward * chaseSpeed * speedDistanceScale * Time.deltaTime;

            // get the direction of where we need to go
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Create the rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Rotate us
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

             */
        }
    }

    bool TargetNullCheck(GameObject target)
    {
        if (target == null)
        {
            stateManager.GetComponent<StateManager_Minion>().ChangeState(stateManager.GetComponent<StateManager_Minion>().findPathState);
            return true;
        }

        return false;
    }

}
