using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class DuckSpawner : MonoBehaviour
{
    [SerializeField] private GameObject duckPrefab;
    [SerializeField] private float spawnArea;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDuckRandomly();
        }
    }

    void SpawnDuckRandomly()
    {
        Vector3 randomPos = Random.insideUnitSphere * spawnArea;

        
        Instantiate(duckPrefab, this.transform.position + randomPos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(this.transform.position, spawnArea);
    }
}
