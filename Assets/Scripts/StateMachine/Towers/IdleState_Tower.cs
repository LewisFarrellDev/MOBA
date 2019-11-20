using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Tower : BaseState
{
    public override void OnBeginState(StateManager stateManager)
    {
        this.stateManager = stateManager;
        stateDescription = "Idle...";
    }

    public override void OnEndState()
    {
        //
    }

    public override void UpdateState()
    {

    }
}
