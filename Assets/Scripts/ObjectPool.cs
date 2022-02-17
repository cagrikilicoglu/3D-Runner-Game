using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    //public List<GameObject> pooledObjects;
    public List<GameObject> pooledObstacles;
    public List<GameObject> pooledGems;
   // public GameObject objectToPool;
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
        // set 20 obstacles and gems at the start of the game and pool them
        pooledObstacles = new List<GameObject>();
        pooledGems = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
           
            GameObject obstacle = (GameObject)Instantiate(obstacleToPool);

            int randomGemIndex = Random.Range(0,2);
            GameObject gem = (GameObject)Instantiate(gemsToPool[randomGemIndex]);
            
            obstacle.SetActive(false);
            gem.SetActive(false);
            pooledObstacles.Add(obstacle);
            pooledGems.Add(gem);
        }
    }

  
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
