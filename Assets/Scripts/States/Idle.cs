using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private float rotationSpeed;

    public Idle(CowFSM _cowFSM, float _rotationSpeed) : base(_cowFSM)
    {
        rotationSpeed = _rotationSpeed;
        cowFSM.CurrentCowState = CowFSM.CowState.IDLE;
        OnEnter();
    }

    public override void OnUpdate()
    {
        cowFSM.transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.up);
    }

    public override void CheckTransition()
    {
        if (cowFSM.Hunger < 5)
        {
            cowFSM.ChangeState(new MakeLove(cowFSM, cowFSM.mateDistance));
        }

        else if (cowFSM.Hunger > 10)
        {
            cowFSM.ChangeState(new Eat(cowFSM, 10, cowFSM.feedDistance));
        }
    }

    
}
