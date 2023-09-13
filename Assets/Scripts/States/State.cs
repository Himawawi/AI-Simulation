using UnityEngine;
public abstract class State
{
    // Protected in order to get access to it from other classes as well
    protected CowFSM cowFSM;

    public State(CowFSM _cowFSM)
    {
        cowFSM = _cowFSM;
    }

    public virtual void OnEnter()
    {
        Debug.Log("Current State: " + this.ToString());
    }

    public abstract void OnUpdate();

    public virtual void OnExit()
    {

    }

    public abstract void CheckTransition();
}
