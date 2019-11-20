using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Minion : BaseState
{
    public float damage = 10;
    public float attackRate = 0.5f;

    private float lastAttack = 0;

    private GameObject target;

    public override void OnBeginState(StateManager stateManager)
    {
        this.stateManager = stateManager;
        stateDescription = "Attacking...";
    }

    public override void OnEndState()
    {
        
    }

    public override void UpdateState()
    {
        target = stateManager.GetComponent<StateManager_Minion>().GetTarget();
        if (TargetNullCheck(target))
            return;

        if (Time.time > lastAttack + attackRate)
        {
            lastAttack = Time.time;
            Entity entity = target.GetComponent<Entity>();

            // Play SFX
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.Play();

            if (entity != null)
            {
                if (entity.TakeDamage(damage))
                {
                    stateManager.GetComponent<StateManager_Minion>().RemoveTarget(target);
                    GameObject.Destroy(target);
                }
            }
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
