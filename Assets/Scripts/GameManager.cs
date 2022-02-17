using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int totalCoinsCollected;
    [SerializeField] public int totalCoin;
    [SerializeField] public int collectedCoin;   
    [SerializeField] private int lives;
    [SerializeField] private int stage;
    [SerializeField] private int level;
    [SerializeField] private int defaultLives;
    [SerializeField] private int livesPurchased;

    [SerializeField] private int priceOfAddingLives;
    [SerializeField] private int priceOfIncreasingGemEarning1;
    [SerializeField] private int priceOfIncreasingGemEarning2;

    [SerializeField] private int maxSafeSpawnAttempts;
    [SerializeField] public bool isGameActive;

    [SerializeField] private bool isClickAvailable;

    [SerializeField] public int stageNumber;

// BAK
    [SerializeField] private float maxZRange = 70;
    [SerializeField] private float minZRange = 0;
    [SerializeField] private float xRange = 1;
    [SerializeField] private float yPositionGem = 0.4f;
    [SerializeField] private float yPositionObstacle = 0;

    [SerializeField] private float objectCheckRadius =3;

    [SerializeField] public int celebrationTime = 3;

    [SerializeField] public GameObject EntryScreen;
    [SerializeField] public GameObject WinScreen;
    [SerializeField] public GameObject LoseScreen;
    [SerializeField] public GameObject GameScreen;
    [SerializeField] public GameObject UpdatesScreen;

  

    [SerializeField] private GameObject lastScreen;
    

    [SerializeField] public TextMeshProUGUI stageText;
    [SerializeField] public TextMeshProUGUI levelText;
    [SerializeField] public TextMeshProUGUI coinText;
    [SerializeField] public TextMeshProUGUI earnedCoinText;
    [SerializeField] public TextMeshProUGUI lostCoinText;
    [SerializeField] public TextMeshProUGUI livesText;
    [SerializeField] public TextMeshProUGUI livesTextAtUpdatesPanel;

    [SerializeField] public TextMeshProUGUI livesPriceText;
    [SerializeField] public TextMeshProUGUI gemEarning1Text;
    [SerializeField] public TextMeshProUGUI gemEarning2Text;

    // arraya çevirilebilir
    [SerializeField] public GameObject gemPrefab;
    [SerializeField] public GameObject obstaclePrefab;


    // sil
    private int safeCount = 0;

    private List<GameObject> gemList =  new List<GameObject>();
    private List<GameObject> obstacleList =  new List<GameObject>(); 


     void Awake() {
        isGameActive = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        level =1;
        totalCoin=90;
        totalCoinsCollected = totalCoin;
        stage=1;
        defaultLives = 3;

        maxSafeSpawnAttempts = 100;

        lastScreen = null;
        coinText.text = totalCoin.ToString();
        levelText.text = level.ToString();
        stageText.text = "Stage " + stage.ToString() ;

        isClickAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (isClickAvailable & Input.GetMouseButtonDown(0)) {
        //     StartGame();
        // }
    }

    public void UpdateCoins(int coinsToAdd) {

        
        collectedCoin += coinsToAdd;
        totalCoin += coinsToAdd;
        coinText.text = totalCoin.ToString();

        totalCoinsCollected = totalCoinsCollected+= coinsToAdd;
        LevelUp();
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

        CreateStage(stage);
        isGameActive=true;
        isClickAvailable=false;

        
        collectedCoin = 0;
        lives= defaultLives + livesPurchased;
        
        livesText.text = lives.ToString();
    }

    public void WinGame()
    {
        DestroyObjects(gemList);
        DestroyObjects(obstacleList);
        //totalCoin += collectedCoin;
        stage +=1;
        isGameActive = false;
        
        stageText.text = "Stage " + stage.ToString();
        coinText.text=totalCoin.ToString();
        earnedCoinText.text=collectedCoin.ToString() + " coins collected!";
        GameScreen.SetActive(false);
        WinScreen.SetActive(true);
       
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //StartGame();
    }

    public void CreateStage(int stageNumber) {
        int gemNumber ;
        int obstacleNumber; 
        if (stageNumber < 4) {
        gemNumber = 1 + stageNumber * 2;
        obstacleNumber = stageNumber * 2;
        } else if (stageNumber >= 4 && stageNumber < 7) {
        gemNumber = stageNumber + 4;
        obstacleNumber = stageNumber + 3;
        } else {
            gemNumber = 10;
            obstacleNumber = 10;
        }

        for (int i = 0; i <= gemNumber; i++) {

            //float zPosition = Random.Range(minZRange, maxZRange);
            //float xPosition = Random.Range(-xRange, xRange);

            float zPosition;
            float xPosition;
            Vector3 randomPos;


            bool isPositionValid;
            int spawnAttempts = 0;

            do {
            zPosition = Random.Range(minZRange, maxZRange);
            xPosition = Random.Range(-xRange, xRange);
            randomPos = new Vector3(xPosition, yPositionGem, zPosition);

            isPositionValid = true;

            isPositionValid = CheckOverlap(randomPos);
            spawnAttempts++;
            } while (!isPositionValid && spawnAttempts < maxSafeSpawnAttempts);

            // while(!isPositionValid && spawnAttempts < maxSpawnAttempts)
            // {
            // zPosition = Random.Range(minZRange, maxZRange);
            // xPosition = Random.Range(-xRange, xRange);
            // spawnAttempts++;
            
            // randomPos = new Vector3(xPosition, yPositionGem, zPosition);
            // isPositionValid = true;
            // Collider[] Colliders = Physics.OverlapSphere(randomPos, 3);
            // // //int numColliders = Physics.OverlapSphereNonAlloc(randomPos, 20, Collider[] Results);
            // foreach (Collider col in Colliders) 
            // {
            // if(col != null) {
            // isPositionValid = false;
            //     Debug.Log("sıkıntı");
            // }
                
            // }
            // }

            // if (Colliders.Length == 0 || Colliders == null) 
            // {
            //     isPositionValid =true;
            // }
            // safeCount++ ;
            // if(safeCount == 50) {
            //     Debug.Log(safeCount);
            //     break;
            // }
            // if(isPositionValid) {
            //     break;
            // }

            // } while (isPositionValid);
            //Vector3 randomPos = new Vector3(xPosition, yPositionGem, zPosition);

            //Collider[] Colliders = Physics.OverlapSphere(randomPos, 5);

            //var gem = Instantiate(gemPrefab, randomPos, gemPrefab.transform.rotation);
            if (isPositionValid) {
            var gem = ObjectPool.SharedInstance.GetPooledGem();
            if (gem != null) {
            
            gem.transform.position = randomPos;
            gem.SetActive(true);
            gemList.Add(gem);
            }
            }
        }


        for (int i = 0; i<= obstacleNumber; i++) {
            
            float zPosition;
            float xPosition;
            Vector3 randomPos;

            bool isPositionValid;
            int spawnAttempts = 0;

            do {
             zPosition = Random.Range(minZRange, maxZRange);
             xPosition = Random.Range(-xRange, xRange);
             randomPos = new Vector3(xPosition, yPositionObstacle, zPosition);
            isPositionValid = true;

            isPositionValid = CheckOverlap(randomPos);
            spawnAttempts++;
            } while (!isPositionValid && spawnAttempts < maxSafeSpawnAttempts);

            if(isPositionValid) {
            var obstacle = ObjectPool.SharedInstance.GetPooledObstacle();
            if (obstacle != null) {
            obstacle.transform.position = randomPos;
            obstacle.SetActive(true);
            obstacleList.Add(obstacle);
            }
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
        lostCoinText.text = totalCoin.ToString() + " coins lost! :( " ;

    }

    void LevelUp() {

        if (totalCoinsCollected >=  100) {
            if( totalCoinsCollected % 100 == 0) {
                level +=1;
                levelText.text = level.ToString();
                Debug.Log(level);
            }
        }
    }

    public void GoToStore () {
       
        
       
        if (EntryScreen.activeSelf) {
        EntryScreen.SetActive(false);
        lastScreen = EntryScreen;

        } else if (WinScreen.activeSelf) {

        WinScreen.SetActive(false);
        lastScreen = WinScreen;
        }
        
        UpdatesScreen.SetActive(true);
        CreateStore();

    }
    public void BackToMenu () {
        
        lastScreen.SetActive(true);
        UpdatesScreen.SetActive(false);
    }

    public void IncreaseGemEarnings () {

    }

    public void IncreaseLives () {
        if ( totalCoin >= priceOfAddingLives){
        totalCoin -= priceOfAddingLives;
        livesPurchased += 1 ;
        coinText.text = totalCoin.ToString();
        }
        else {
            // yeterli paranız yok.
        }
        livesTextAtUpdatesPanel.text = (defaultLives+livesPurchased).ToString(); 
    }
    public void CreateStore() {
        livesTextAtUpdatesPanel.text = (defaultLives+livesPurchased).ToString();
        livesPriceText.text = priceOfAddingLives.ToString();
        gemEarning1Text.text = priceOfIncreasingGemEarning1.ToString();
        gemEarning2Text.text = priceOfIncreasingGemEarning2.ToString();
    }

    public bool CheckOverlap(Vector3 randomPosition) {

            bool isPositionValid = true;
            Collider[] Colliders = Physics.OverlapSphere(randomPosition, objectCheckRadius);
            // //int numColliders = Physics.OverlapSphereNonAlloc(randomPos, 20, Collider[] Results);
            foreach (Collider col in Colliders) 
            {
            if(col != null) {
                if(col.gameObject.CompareTag("Collectable") || col.gameObject.CompareTag("Obstacle")) {
            isPositionValid = false;
                }
            }
            }
            return isPositionValid;
    }

    // private void OnMouseDown() {
    //     if(isClickAvailable) {
    //         StartGame();
    //         Debug.Log("clicked");
    //     }
    // }

  

}
