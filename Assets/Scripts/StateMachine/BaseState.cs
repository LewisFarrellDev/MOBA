using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    [HideInInspector]
    public StateManager stateManager;
    [HideInInspector]
    public string stateDescription;

    public abstract void OnBeginState(StateManager stateManager);
    public abstract void UpdateState();
    public abstract void OnEndState();
    virtual public string GetStateDescription()
    {
        return stateDescription;
    }
}
