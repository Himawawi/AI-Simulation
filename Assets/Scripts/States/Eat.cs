using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Eat : State
{
    private Food target;
    private NavMeshAgent agent;
    private float minEatDistance;
    private float minFood;

    public Eat(CowFSM _cowFSM, float _minEatDistance, float _minFood) : base(_cowFSM)
    {
        agent = _cowFSM.GetComponent<NavMeshAgent>();
        minEatDistance = _minEatDistance;
        minFood = _minFood;
        cowFSM.CurrentCowState = CowFSM.CowState.HUNGRY;
        OnEnter();
    }

    // Everytime the cowFSM enters in this State, the target should update itself through the FindBestFood(); method.
    public override void OnEnter()
    {
        base.OnEnter();
        target = FindBestFood();
    }

    public override void OnUpdate()
    {
        /* The target is checked every frame. If there is no target, 
        the target should update itself through the FindBestFood(); method. */
        if (target == null)
        {
            target = FindBestFood();
            return;
        }

        if (Vector3.SqrMagnitude(target.transform.position - cowFSM.transform.position) < minEatDistance * minEatDistance)
        {
            cowFSM.EatFood(target);
            target = null;
            return;
        }

        // If the agent doesn' t have a path and his path isn' t pending.
        if (agent.isActiveAndEnabled && !agent.hasPath && !agent.pathPending)
        {
            // If the target is not in range of the NavMesh.
            if (agent.isActiveAndEnabled && !agent.SetDestination(target.transform.position))
            {

                if (NavMesh.SamplePosition(target.transform.position, out NavMeshHit hit, float.PositiveInfinity,
                     NavMesh.AllAreas))
                {
                    // _ = stands to not allocate any saves
                    _ = agent.SetDestination(hit.position);
                }
            }
        }
    }

    public override void CheckTransition()
    {
        if (cowFSM.Hunger < 5)
        {
            cowFSM.ChangeState(new MakeLove(cowFSM, cowFSM.mateDistance));
        }
    }

    private Food FindBestFood()
    {
        List<Food> allFood = Food.AllFood;
        float closestDistance = float.PositiveInfinity;
        Food closestFood = null;
        // Temporary Variable to subsitute Vector3.SqrMagnitude(food.transform.position - cowFSM.transform.position).
        float temp;

        foreach(var food in allFood)
        {
            // If the food.CurrentFood amount less than the cowFSM.Hunger is, continue.
            if (food.CurrentFood < minFood)
            {
                continue;
            }

            // temp now is equal to the distance between the cowFSM and the food. This saves the calculation.
            temp = Vector3.SqrMagnitude(food.transform.position - cowFSM.transform.position);
            // If the Distance between the position of cowFSM and food is less than closestDistance, ???
            if (temp < closestDistance)
            {
                closestDistance = temp;
                closestFood = food;
            }
        }

        return closestFood;
    }

   
}
