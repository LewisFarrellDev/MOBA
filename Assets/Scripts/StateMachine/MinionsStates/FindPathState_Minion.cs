using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPathState_Minion : BaseState
{
    private List<Transform> targetList = new List<Transform>();
    private Transform target;
    public float acceleration = 2;
    public float speed = 3;
    public float rotationSpeed = 3;
    public float minDistanceToTargetBeforeRotation = 3;
    float speedDistanceScale = 0;
    public bool useGizmos;

    Vector3[] path;
    int waypointIndex;

    bool isFollowingPath;

    public void FindPath()
    {
        if (path == null)
        {
            PathRequester.RequestPath(transform.position, target.position, OnPathFound);
        }
    }

    public void FindPath(Vector3 target)
    {
        if (path == null)
        {
            PathRequester.RequestPath(transform.position, target, OnPathFound);
        }
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (!pathSuccessful)
        {
            print("No path could be found");
            return;
        }

        path = newPath;
        if (gameObject != null)
            StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = Vector3.back;
        if (path.Length != 0)
            currentWaypoint = path[0];

        if (path.Length == 0)
        {
            FindPath();
            yield return null;
        }

        isFollowingPath = true;

        while (isFollowingPath)
        {
            if (gameObject == null)
                yield return null; 

            bool isFinalWaypoint = (waypointIndex == path.Length - 1 && targetList.Count == 1) ? true : false;
            if (Vector3.Distance(transform.position, currentWaypoint) < 1 || (Vector3.Distance(transform.position, currentWaypoint) < minDistanceToTargetBeforeRotation && !isFinalWaypoint))
            {
                waypointIndex++;
                if (waypointIndex >= path.Length)
                {
                    targetList.Remove(target);
                    if (targetList.Count == 0)
                    {
                        StopFollowPath();
                        yield break;
                    }
                    else
                    {
                        target = targetList[0];
                        RecalculatePath();
                        yield break;
                    }
                }

                currentWaypoint = path[waypointIndex];
                //RecalculatePath();
            }

            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            // Smooth move with de-acceleration 

            speedDistanceScale = Mathf.Lerp(speedDistanceScale, Mathf.Clamp01((Vector3.Distance(transform.position, currentWaypoint) / (minDistanceToTargetBeforeRotation * 2))), acceleration * Time.deltaTime);

            // Always move the agent forward
            transform.position += transform.forward * speed * speedDistanceScale * Time.deltaTime;

            // get the direction of where we need to go
            Vector3 direction = (currentWaypoint - transform.position).normalized;

            // Create the rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Rotate us
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);


            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (!useGizmos)
            return;

        if (path != null)
        {
            for (int i = waypointIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == waypointIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }

    public void StopFollowPath()
    {
        isFollowingPath = false;
        //transform.position = startPosition;
        //transform.rotation = startRotation;
        waypointIndex = 0;
        path = null;
    }

    public void RecalculatePath()
    {
        isFollowingPath = false;
        waypointIndex = 0;
        path = null;
        PathRequester.RequestPath(transform.position, target.position, OnPathFound);
    }

    public override void OnBeginState(StateManager stateManager)
    {
        FindPath();
        stateDescription = "Moving to Enemy Base";
    }

    public override void UpdateState()
    {
        //print("Following Path");
    }

    public override void OnEndState()
    {
        StopFollowPath();
    }

    public void SetTargets(List<Transform> targets)
    {
        targetList = new List<Transform>(targets);
        target = this.targetList[0];
    }
}
