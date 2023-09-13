using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowFSM : MonoBehaviour
{
    // Für Debug.
    public CowState CurrentCowState;
    public enum CowState
    {
        IDLE, HUNGRY, MAKELOVE
    }

    public static List<CowFSM> AllCows = new List<CowFSM>();

    [Header("General")]
    [SerializeField] private float maxHunger;
    [SerializeField] private float hungerRate;
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private float hunger;
    // Hier ändert sich die aktive State dauerhaft in der Update.
    private State currentState;

    [Header("Idle")]
    [SerializeField] private float rotateSpeed;

    [Header("Eat")]
    public float feedDistance;

    [Header("MakeLove")]
    public float mateDistance;

    public float Hunger
    {
        get => hunger;
        set
        {
            hunger = value;

            if (hunger >= maxHunger)
            {
                Debug.Log(cowPrefab.gameObject.name + " is dead.");
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    public State CurrentState => currentState;


    private void Awake()
    {
        AllCows.Add(this);
        currentState = new Idle(this, rotateSpeed);
    }

    private void OnDestroy()
    {
        AllCows.Remove(this);
    }

    private void Update()
    {
        // Every second cowFSM gets 2 points of hunger.
        Hunger += hungerRate * Time.deltaTime;

        currentState.CheckTransition();
        currentState.OnUpdate();
    }

    public void ChangeState(State _state)
    {
        currentState.OnExit();
        currentState = _state;
        currentState.OnEnter();
    }

    public void EatFood(Food _food)
    {
        Debug.Log(cowPrefab.gameObject.name + " Has eaten the food");
        // hunger -= _food.RefillFood(Hunger);
        Hunger -= _food.RefillFood(Hunger);
    }

    public void MakeLove(CowFSM _cow)
    {
        Hunger += 5;
        _cow.Hunger += 5;
        // To solve a problem where the children would spawn frozen till death.
        CowFSM child = Instantiate(cowPrefab, transform.position, transform.rotation).GetComponent<CowFSM>();
        if (!AllCows.Contains(child))
        {
            AllCows.Add(child);
        }
        child.currentState = new Idle(child, child.rotateSpeed);
        // This fixes a problem where the children would spawn with Hunger at 10 and stay frozen till death.
        child.Hunger = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.transform.position, mateDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.transform.position, feedDistance);
    }
}
