using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_Tower : StateManager
{
    // Other scripts
    private Entity entity;

    // states
    private AttackState_Tower attackState;
    public IdleState_Tower idleState;

    void Start()
    {
        entity = GetComponent<Entity>();
        idleState = GetComponent<IdleState_Tower>();
        attackState = GetComponent<AttackState_Tower>();

        ChangeState(idleState);

    }

    void FixedUpdate()
    {
        // Draw Attack radius
        List<Vector3> locations = new List<Vector3>();
        float radius = GetComponent<SphereCollider>().radius;

        for (int i = 0; i < 20; i++)
        {
            float angle = i * Mathf.PI * 2 / 20;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 position = transform.position + new Vector3(x, 0, z);
            locations.Add(position);
        }

        for (int i = 0; i < locations.Count; i++)
        {
            int index = (i == locations.Count - 1) ? 0 : i + 1;
            Debug.DrawLine(locations[i], locations[index], Color.white);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if we collided with a valid target
        Entity otherEntity = other.gameObject.GetComponent<Entity>();

        // If we did...
        if (otherEntity != null && otherEntity.team != entity.team)
        {
            targetList.Add(other.gameObject);

            ChangeState(attackState);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if we collided with a valid target
        Entity otherEntity = other.GetComponent<Entity>();

        // If we did...
        if (otherEntity != null && otherEntity.team != entity.team)
        {
            targetList.Remove(other.gameObject);

            if (targetList.Count <= 0)
                ChangeState(idleState);
        }
    }

    public void RemoveTarget(GameObject obj)
    {
        targetList.Remove(obj);

        if (targetList.Count == 0)
            ChangeState(idleState);
    }
}
