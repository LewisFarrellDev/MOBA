using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState_Scuttle : BaseState
{
    public List<GameObject> patrolPoints = new List<GameObject>();
    public float patrolSpeed = 2;
    public float minDistanceToTargetBeforeRotation = 5;
    public float acceleration = 2;
    public float rotationSpeed = 2;
    public bool useGizmos;

    int currentPatrolPointIndex = 0;
    float speedDistanceScale = 0;
    bool isReverse = false;


    public override void OnBeginState(StateManager stateManager)
    {
        stateDescription = "Patroling...";
        this.stateManager = stateManager;
        currentPatrolPointIndex = 0;

        if (patrolPoints.Count != 0)
        {
            foreach (GameObject patrolPoint in patrolPoints)
            {
                if (Vector3.Distance(transform.position, patrolPoint.transform.position) < Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].transform.position))
                {
                    currentPatrolPointIndex = patrolPoints.IndexOf(patrolPoint);
                }
            }
        }
    }

    public override void OnEndState()
    {

    }

    public override void UpdateState()
    {
        GameObject target = patrolPoints[currentPatrolPointIndex];

        if (Vector3.Distance(transform.position, target.transform.position) <= minDistanceToTargetBeforeRotation)
        {

            if (currentPatrolPointIndex >= patrolPoints.Count - 1)
                isReverse = true;

            if (isReverse)
                currentPatrolPointIndex--;
            else
                currentPatrolPointIndex++;

            currentPatrolPointIndex = Mathf.Clamp(currentPatrolPointIndex, 0, patrolPoints.Count - 1);

            if (currentPatrolPointIndex <= 0)
                isReverse = false;
        }

        // Smooth move with de-acceleration 
        speedDistanceScale = Mathf.Lerp(speedDistanceScale, Mathf.Clamp01((Vector3.Distance(transform.position, target.transform.position) / (minDistanceToTargetBeforeRotation * 2))), acceleration * Time.deltaTime);

        // Always move the agent forward
        transform.position += transform.forward * patrolSpeed * speedDistanceScale * Time.deltaTime;

        // get the direction of where we need to go
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // Create the rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Rotate us
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    public List<GameObject> GetPatrolPoints()
    {
        return patrolPoints;
    }

    void OnDrawGizmos()
    {
        if (!useGizmos)
            return;

        // Draw the patrol path
        for (int i = 0; i < patrolPoints.Count - 1; i++)
        {
            int index = (i == patrolPoints.Count - 1) ? 0 : i + 1;
            Debug.DrawLine(patrolPoints[i].transform.position, patrolPoints[index].transform.position, Color.green);
        }
    }
}
