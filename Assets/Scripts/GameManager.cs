using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int totalGem;
    [SerializeField] public int collectedGem;   
    [SerializeField] private int lives;
    [SerializeField] private int level;
    [SerializeField] public bool isGameActive;

    [SerializeField] public int levelNumber;

// BAK
    [SerializeField] private float maxZRange = 45;
    [SerializeField] private float minZRange = 0;
    [SerializeField] private float xRange = 1;
    [SerializeField] private float yPositionGem = 0.4f;
    [SerializeField] private float yPositionObstacle = 0;

    [SerializeField] public int celebrationTime = 3;

    [SerializeField] public GameObject EntryScreen;
    [SerializeField] public GameObject WinScreen;
    [SerializeField] public GameObject LoseScreen;
    [SerializeField] public GameObject GameScreen;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI gemText;
    [SerializeField] public TextMeshProUGUI earnedGemText;
    [SerializeField] public TextMeshProUGUI lostGemText;
    [SerializeField] public TextMeshProUGUI livesText;

    // arraya çevirilebilir
    [SerializeField] public GameObject gemPrefab;
    [SerializeField] public GameObject obstaclePrefab;

    private List<GameObject> gemList =  new List<GameObject>();
    private List<GameObject> obstacleList =  new List<GameObject>(); 


     void Awake() {
        isGameActive = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        totalGem=0;
        
        level=1;

        gemText.text = totalGem.ToString();
        levelText.text = level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGem(int gemToAdd) {

        collectedGem += gemToAdd;
        totalGem += gemToAdd;
        gemText.text = totalGem.ToString();
        //Debug.Log(totalGoldCollected);

    }

    public void UpdateLives() {
       
        if (lives > 1)
        {
            lives -= 1;
            //livesText.text = "Lives: " + lives;
         

        }
        else
        {
            lives = 0;
            //livesText.text = "Lives: " + lives;

            //gameOverText.gameObject.SetActive(true);
            //restartButton.gameObject.SetActive(true);
            LoseGame();
            isGameActive = false;
        }
           livesText.text = lives.ToString();
        //Debug.Log(lives);
        //Debug.Log(isGameActive);

    }
    /// level seçeneği eklenebilir.
    public void StartGame() {

        EntryScreen.SetActive(false);
        WinScreen.SetActive(false);
        GameScreen.SetActive(true);

        CreateLevel(level);
        isGameActive=true;

        
        collectedGem = 0;
        lives=3;
        
        livesText.text = lives.ToString();
    }

    public void WinGame()
    {
        DestroyObjects(gemList);
        DestroyObjects(obstacleList);
        //totalGem += collectedGem;
        level +=1;
        isGameActive = false;
        
        levelText.text = level.ToString();
        gemText.text=totalGem.ToString();
        earnedGemText.text=collectedGem.ToString() + "gems collected!";
        GameScreen.SetActive(false);
        WinScreen.SetActive(true);
       
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //StartGame();
    }

    public void CreateLevel(int levelNumber) {
        int gemNumber ;
        int obstacleNumber; 
        if (levelNumber < 5) {
        gemNumber = 4 + levelNumber * 2;
        obstacleNumber = levelNumber * 3;
        } else {
        gemNumber = levelNumber * 3;
        obstacleNumber = levelNumber * 2;
        }

        for (int i = 0; i<= gemNumber; i++) {

            float zPosition = Random.Range(minZRange, maxZRange);
            float xPosition = Random.Range(-xRange, xRange);
            Vector3 randomPos = new Vector3(xPosition, yPositionGem, zPosition);

            //var gem = Instantiate(gemPrefab, randomPos, gemPrefab.transform.rotation);
            var gem = ObjectPool.SharedInstance.GetPooledGem();
            if (gem != null) {
            
            gem.transform.position = randomPos;
            gem.SetActive(true);
            gemList.Add(gem);
            }
        }


        for (int i = 0; i<= obstacleNumber; i++) {
            
            float zPosition = Random.Range(minZRange, maxZRange);
            float xPosition = Random.Range(-xRange, xRange);
            Vector3 randomPos = new Vector3(xPosition, yPositionObstacle, zPosition);

            var obstacle = ObjectPool.SharedInstance.GetPooledObstacle();
            if (obstacle != null) {
            obstacle.transform.position = randomPos;
            obstacle.SetActive(true);
            obstacleList.Add(obstacle);
            }
            
        }

    }

    void DestroyObjects(List<GameObject> objects) {
        foreach (GameObject item in objects)
        {
            item.SetActive(false);
        }
    }

    void LoseGame() {

        DestroyObjects(gemList);
        DestroyObjects(obstacleList);
        LoseScreen.SetActive(true);
        GameScreen.SetActive(false);
        lostGemText.text = totalGem.ToString() + " gems lost! :( " ;

    }

}
