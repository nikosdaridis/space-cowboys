using System.Collections.Generic;
using UnityEngine;

public class PoolHandler : MonoBehaviour
{
    // Private
    [SerializeField] private List<GameObject> gameObjects;
    [SerializeField] private int initialPoolSize;
    [SerializeField] private int increaseByAmount;
    [SerializeField] private GameObject prefab;

    void Awake()
    {
        // Increase the Pool with an initial Size
        IncreasePool(initialPoolSize);
    }

    public GameObject RequestGameObject()
    {
        // Check if there is an Available gameObject
        for (int i = 0; i <= gameObjects.Count - 1; i++)
        {
            if (gameObjects[i].activeInHierarchy == false)
            {
                return gameObjects[i]; // Return an Inactive gameObject for Use
            }
        }

        // Inrease Pool Size if Full
        IncreasePool(increaseByAmount);

        // Return the Newest Instantiated gameObject
        return gameObjects[gameObjects.Count - 1];
    }

    private void IncreasePool(int increaseSize)
    {
        for (int i = 1; i <= increaseSize; i++)
        {
            // Instantiate and Add it to the List
            gameObjects.Add(Instantiate(prefab, Vector3.zero, Quaternion.identity, transform) as GameObject);

            // Disable it
            gameObjects[gameObjects.Count - 1].SetActive(false);
        }
    }
}