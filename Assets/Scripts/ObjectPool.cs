using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    private List<GameObject> pooledObstacles;
    private List<GameObject> pooledGems;
    public GameObject obstacleToPool;
    public List<GameObject> gemsToPool;

    [SerializeField] private int amountToPool;

    void Awake()
    {
        SharedInstance = this;
        amountToPool = 20;
    }

    void Start()
    {
        // set 20 obstacles and gems at the start of the game and pool them to spawn in the game
        pooledObstacles = new List<GameObject>();
        pooledGems = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {  
            GameObject obstacle = (GameObject)Instantiate(obstacleToPool);

            // Determine which type of collectable is pooled randomly
            int randomGemIndex = Random.Range(0,2);
            GameObject gem = (GameObject)Instantiate(gemsToPool[randomGemIndex]);

            obstacle.SetActive(false);
            gem.SetActive(false);
            pooledObstacles.Add(obstacle);
            pooledGems.Add(gem);
        }
    }

    // Get an available one of the pooled obstacles when needed
    public GameObject GetPooledObstacle()
    {
        for (int i = 0; i < pooledObstacles.Count; i++)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }

        }
        return null;
    }

    //Get an available one of the pooled collectables when needed
    public GameObject GetPooledGem()
    {
        for (int i = 0; i < pooledGems.Count; i++)
        {
            if (!pooledGems[i].activeInHierarchy)
            {
                return pooledGems[i];
            }

        }
        return null;
    }
}
