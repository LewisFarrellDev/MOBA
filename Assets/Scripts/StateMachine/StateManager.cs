using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    // Activate State
    [HideInInspector]
    public BaseState currentState;

    // label to display state
    public Text stateLabel;

    // List of targets
    public List<GameObject> targetList = new List<GameObject>();

    public GameObject GetTarget()
    {
        List<GameObject> newTargetList = new List<GameObject>();

        foreach (GameObject target in targetList)
        {
            if (target != null)
                newTargetList.Add(target);
        }

        targetList = newTargetList;

        if (targetList.Count == 0)
            return null;

        return targetList[0];
    }

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
            currentState.OnEndState();

        currentState = newState;
        currentState.OnBeginState(this);
        if (stateLabel != null)
            stateLabel.text = currentState.GetStateDescription();
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }
}
