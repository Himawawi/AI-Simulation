using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MakeLove : State
{
    // Reference type leitet von eine Klasse ab (Unity)
    // Object kann man refernzieren (grün angezeigt), value type (blau angezeigt) nicht.
    private CowFSM target = null;
    private NavMeshAgent agent = null;
    float minMakeLoveDistance;

    public MakeLove(CowFSM _cowFSM, float _minMakeLoveDistance) : base(_cowFSM)
    {
        if (agent == null)
        {
            // out for value type, ref for reference type
            if (cowFSM.TryGetComponent(out NavMeshAgent _agent))
            {
                agent = _agent;
            }
{
}
        }
        minMakeLoveDistance = _minMakeLoveDistance;
        cowFSM.CurrentCowState = CowFSM.CowState.MAKELOVE;
        OnEnter();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        target = FindBestCow();
    }

    public override void OnUpdate()
    {
        if (target == null)
        {
            target = FindBestCow();
            return;
        }

        if(Vector3.Distance(target.transform.position, cowFSM.transform.position) <= minMakeLoveDistance)
        {
            cowFSM.MakeLove(target);
            target = null;
            return;
        }

        if (agent.isActiveAndEnabled && !agent.hasPath && !agent.pathPending)
        {
            if (agent.isActiveAndEnabled && !agent.SetDestination(target.transform.position))
            {
                if (NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, float.PositiveInfinity,
                     NavMesh.AllAreas))
                {
                    _ = agent.SetDestination(hit.position);
                }
            }
        }
    }

    public override void CheckTransition()
    {
        if (cowFSM.Hunger > 5)
        {
            cowFSM.ChangeState(new Eat(cowFSM, cowFSM.feedDistance, 10));
        }
    }

    private CowFSM FindBestCow()
    {
        List<CowFSM> allCows = CowFSM.AllCows;
        float closestDistance = float.PositiveInfinity;
        CowFSM closestCow = null;
        float temp;

        foreach (var cow in allCows)
        {
            if (cow == cowFSM || !(cow.CurrentState is MakeLove))
            {
                continue;
            }

            temp = Vector3.SqrMagnitude(cow.transform.position - cowFSM.transform.position);
            if (temp < closestDistance)
            {
                closestDistance = temp;
                closestCow = cow;
            }
        }

        return closestCow;
    }

}
