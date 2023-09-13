using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static List<Food> AllFood = new List<Food>();

    private float currentFood;

    public float CurrentFood
    {
        get => currentFood;
        private set
        {
            currentFood = Mathf.Clamp(value, 0, maxFood);
        }
    }

    [SerializeField] private float growRate = 1;
    [SerializeField] private float maxFood = 20;

    private void Awake()
    {
        AllFood.Add(this);
    }

    private void OnDestroy()
    {
        AllFood.Remove(this);
    }

    private void Update()
    {
        CurrentFood += growRate * Time.deltaTime;
        transform.localScale = Vector3.one * (CurrentFood / growRate);
    }

    public float RefillFood(float amounts)
    {
        float value = Mathf.Min(CurrentFood, amounts);
        CurrentFood -= value;
        return value;
    }

    private void OnValidate()
    {
        if (maxFood < 1)
        {
            maxFood = 1;
        }

        if (Mathf.Approximately(0, growRate) || growRate < 0)
        {
            growRate = 10 - 1f;
        }
    }
}
