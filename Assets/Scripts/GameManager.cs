using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int totalCoinsCollected;
    [SerializeField] public int totalCoin;
    [SerializeField] public int collectedCoin;  

    [SerializeField] private int defaultLives; 
    [SerializeField] private int livesPurchased;
    [SerializeField] private int lives;
    [SerializeField] private int priceOfAddingLives;

    [SerializeField] private int stage;
    [SerializeField] private int level;
    
    [SerializeField] public bool isGameActive;
    
    // spawn variables for collectables and obstacles
    [SerializeField] private float maxZRange = 70;
    [SerializeField] private float minZRange = 0;
    [SerializeField] private float xRange = 1;
    [SerializeField] private float yPositionGem = 0.4f;
    [SerializeField] private float yPositionObstacle = 0;
    [SerializeField] private float objectCheckRadius = 3;
    [SerializeField] private int maxSafeSpawnAttempts = 1000;

    private List<GameObject> gemList =  new List<GameObject>();
    private List<GameObject> obstacleList =  new List<GameObject>(); 

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
    [SerializeField] public TextMeshProUGUI notEnoughCoinsText;
    [SerializeField] public TextMeshProUGUI livesPriceText;

    void Awake() {
        isGameActive = false;
    }
    
    
    void Start()
    {
        // Set default game variables at start
        level = 1;
        totalCoin = 50;
        totalCoinsCollected = totalCoin;
        stage = 1;
        defaultLives = 3;
        lastScreen = null;

        // Set UI elements at start
        coinText.text = totalCoin.ToString();
        levelText.text = level.ToString();
        stageText.text = "Stage " + stage.ToString();
        
    }

    public void UpdateCoins(int coinsToAdd) {

        // Calculate the coins collected in current stage
        collectedCoin += coinsToAdd;

        // Calculate the coins the player has in total
        totalCoin += coinsToAdd;
        coinText.text = totalCoin.ToString();

        // Calculate the total collected coins (ignoring the coins spend on store) to determine player's level
        totalCoinsCollected += coinsToAdd;
        LevelUp();

    }

    void LevelUp() {

        // For each 100 coins collected from the beginning of the session by player, level up the player
        if (totalCoinsCollected >=  100) {
            if( totalCoinsCollected >= level * 100) {
                level +=1;
                levelText.text = level.ToString();
            }
        }
    }

    public void UpdateLives() {
       
        if (lives > 1)
        {
            // when player has lives more than 1, decrease it by 1
            lives -= 1;
        }
        else
        {
            // when lives is zero, lose game
            lives = 0;
            LoseGame();
        }
           livesText.text = lives.ToString();
    }

    public void StartGame() {

        // UI manipulations
        EntryScreen.SetActive(false);
        WinScreen.SetActive(false);
        GameScreen.SetActive(true);

        isGameActive = true;
        CreateStage(stage);

        // When starting each stage, set coins collected in this stage to zero
        collectedCoin = 0;

        // Add lives when starting a stage, if purchased in the store
        lives = defaultLives + livesPurchased;        
        livesText.text = lives.ToString();
    }

    public void WinGame()
    {
        isGameActive = false;
        stage +=1;
        
        // Destroy all the objects in the scene before creating new stage
        DestroyObjects(gemList);
        DestroyObjects(obstacleList);
       
        // UI manipulations
        stageText.text = "Stage " + stage.ToString();
        coinText.text = totalCoin.ToString();
        earnedCoinText.text = collectedCoin.ToString() + " coins collected!";
        GameScreen.SetActive(false);
        WinScreen.SetActive(true);
       
    }

    void LoseGame() {
        isGameActive = false;

        // Destroy all the objects in the scene before creating new stage
        DestroyObjects(gemList);
        DestroyObjects(obstacleList);

        // UI manipulations
        LoseScreen.SetActive(true);
        GameScreen.SetActive(false);
        lostCoinText.text = totalCoin.ToString() + " coins lost! :( " ;
    
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CreateStage(int stageNumber) {

        int gemNumber ;
        int obstacleNumber; 

        // Create certain number of gems and obstacle for each stage, depending on which stage the player is about to start
        if (stageNumber < 4) {
            gemNumber = 1 + stageNumber * 2;
            obstacleNumber = stageNumber * 2;
        } else if (stageNumber >= 4 && stageNumber < 7) {
            gemNumber = stageNumber + 4;
            obstacleNumber = stageNumber + 3;
        } else {
            // After stage 7, no gems and obstacles added to scene because of space constraint
            gemNumber = 10;
            obstacleNumber = 10;
        }

        // When required number of gems is determined, spawn them in random positions
        for (int i = 0; i <= gemNumber; i++) {

            float zPosition;
            float xPosition;
            Vector3 randomPos;
            bool isPositionValid;
            int spawnAttempts = 0;

            // To prevent overlapping of spawned elements, check if the spawn position is available
            // If it's not available, create a new random position until finding an available place 
            // To prevent an infinite loop, search for a random position with a maximum of certain spawn attempts (currently set to 1000)
            do {
            zPosition = Random.Range(minZRange, maxZRange);
            xPosition = Random.Range(-xRange, xRange);
            randomPos = new Vector3(xPosition, yPositionGem, zPosition);

            isPositionValid = true;
            isPositionValid = CheckOverlap(randomPos);

            spawnAttempts++;
            } while (!isPositionValid && spawnAttempts < maxSafeSpawnAttempts);

            // When an available random position is found, spawn object from pooled objects
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

            // To prevent overlapping of spawned elements, check if the spawn position is available
            // If it's not available, create a new random position until finding an available place 
            // To prevent an infinite loop, search for a random position with a maximum of certain spawn attempts (currently set to 1000)
            do {
            zPosition = Random.Range(minZRange, maxZRange);
            xPosition = Random.Range(-xRange, xRange);
            randomPos = new Vector3(xPosition, yPositionObstacle, zPosition);

            isPositionValid = true;
            isPositionValid = CheckOverlap(randomPos);

            spawnAttempts++;
            } while (!isPositionValid && spawnAttempts < maxSafeSpawnAttempts);

             // When an available random position is found, spawn object from pooled objects
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

    // When searching for random positions for collectables and obstacles, check if it is overlapping any of already spawned object
    public bool CheckOverlap(Vector3 randomPosition) {

            bool isPositionValid = true;
            Collider[] Colliders = Physics.OverlapSphere(randomPosition, objectCheckRadius);
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

    void DestroyObjects(List<GameObject> objects) {
        // Destroy objects currently placed on list
        foreach (GameObject item in objects)
        {
            item.SetActive(false);
        }
    }

    
    public void GoToStore () {
       
        // Since the player can both go to the store from entry screen and win screen, hold last scene the player is at
        // Make UI manipulation depending on the screen from which player is going to the store
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

    // Create store for player to purchase lives 
    public void CreateStore() {
        livesTextAtUpdatesPanel.text = (defaultLives+livesPurchased).ToString();
        livesPriceText.text = priceOfAddingLives.ToString();
    }

    // When player is going back from the store, show which screen player is coming from (last screen variable)
    public void BackToMenu () {
        lastScreen.SetActive(true);
        UpdatesScreen.SetActive(false);
    }

    // Player can increase the live count, he/she has when starting a new stage by purchasing live from the store
    public void IncreaseLives () {
        if (totalCoin >= priceOfAddingLives){
        totalCoin -= priceOfAddingLives;
        livesPurchased += 1 ;
        coinText.text = totalCoin.ToString();
        }
        else {
        // If the player doesn't have enough coins, warn the player
        StartCoroutine(ShowNotEnoughCoinsText());
        }
        livesTextAtUpdatesPanel.text = (defaultLives+livesPurchased).ToString(); 
    }


    private IEnumerator ShowNotEnoughCoinsText()
    {
        notEnoughCoinsText.text = "You don't have enough coins. :(";
        yield return new WaitForSeconds(1.0f);
        notEnoughCoinsText.text = "";
    } 
}
