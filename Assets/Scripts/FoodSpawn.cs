using UnityEngine;
using UnityEngine.InputSystem;

public class FoodSpawn : MonoBehaviour
{
    [SerializeField] GameObject foodPrefab;
    private Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnFood();
        
    }

    private void SpawnFood()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(foodPrefab, new Vector3(hit.point.x, hit.point.y + 
                    foodPrefab.transform.position.y, hit.point.z), Quaternion.identity);
            }
        }
    }
}
