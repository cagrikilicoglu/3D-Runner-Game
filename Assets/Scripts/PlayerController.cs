using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float swerveSpeed;
    [SerializeField] private float maxSwerveAmount;
    [SerializeField] private float swerveAmount;

    [SerializeField] private int celebrationTime = 3;
    [SerializeField] private float floatingTextScreenTime = 0.4f;

    [SerializeField] private bool shouldPlayerMove;

    [SerializeField] public GameObject FloatingText;

    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private ParticleSystem fireworksParticle;
    [SerializeField] private ParticleSystem explosionParticleBlue;
    [SerializeField] private ParticleSystem explosionParticleYellow;

    private Vector3 initialPlayerPosition = new Vector3(0, 0, -6.75f);

    private SwerveInputSystem swerveInputSystemScript;
    private GameManager gameManagerScript;
    private Animator playerAnimator;
    private Rigidbody playerRb;

    
   void Awake() {
        swerveInputSystemScript = GetComponent<SwerveInputSystem>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        shouldPlayerMove = false;
    }

    void Start() {
        verticalSpeed = 6;
        swerveSpeed=3;
        maxSwerveAmount=300;
    }

    void FixedUpdate()
    {
        if(transform.position.z == initialPlayerPosition.z) {
            shouldPlayerMove=true;
        } 
      
    // if game is active and player should move, allow running and swerve mouse input
        if (gameManagerScript.isGameActive && shouldPlayerMove){
            playerAnimator.SetBool("isRunning", true);
            MoveForward();
            MoveLeftAndRight();
        } else {
            playerRb.velocity = Vector3.zero;
            playerAnimator.SetBool("isRunning", false);
        }
    }

    // Move player forward at a constant velocity
    void MoveForward() {

    playerRb.velocity = Vector3.forward * verticalSpeed;
    
    }

    // Move player left and right with mouse swerve input
    void MoveLeftAndRight() {
    
    swerveAmount = swerveSpeed * swerveInputSystemScript.moveFactorX;
    swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
    playerRb.velocity = playerRb.velocity + Vector3.right * swerveAmount; 
    
    }

    void OnCollisionEnter(Collision collision)
    {
     
    if (collision.gameObject.CompareTag("Obstacle")) {
        
        gameManagerScript.UpdateLives();
        collision.gameObject.SetActive(false);

        // When player collides with an obstacle show hit particles and the life the player loses
        HitEffect();
        StartCoroutine(ShowFloatingText(-1, collision.gameObject.transform.position));
    }        

    else if (collision.gameObject.CompareTag("Collectable")) {

        gameManagerScript.UpdateCoins(collision.gameObject.GetComponent<Gem>().coinValue);
        collision.gameObject.SetActive(false);

        // When player collides with a collectable show explosion particles and the coins the player win
        ExplosionEffect(collision.gameObject);
        StartCoroutine(ShowFloatingText(collision.gameObject.GetComponent<Gem>().coinValue, collision.gameObject.transform.position));

    }        
    }

    // When player hit the finish line, trigger animation 
    void OnTriggerEnter(Collider other)
    {
        shouldPlayerMove=false;

        // When the player wins the game, show victory animation and firework particles  
        playerAnimator.SetTrigger("victory");
        playerAnimator.SetBool("isRunning", false);
        FireworkEffect();
        
        // When the player wins the game, show the cinematic camera view 
        transform.GetChild(3).gameObject.SetActive(true);
        StartCoroutine(WaitForAnimantion());
    }

    // When the player hit the finish line, wait for certain celebration time to see victory animation with cinematic camera
    // After the animation, player wins the game and have an option to go to next stage
    IEnumerator WaitForAnimantion()
    {
        yield return new WaitForSeconds(celebrationTime);
    
        // Close cinematic camera and turn it back to initial position for next possible celebration
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.GetComponent<CinematicScene>().ReturnToInitialPosition(); 

        gameManagerScript.WinGame();
        transform.position = initialPlayerPosition;
        
    }

    void ExplosionEffect (GameObject gem)
    {
        ParticleSystem explosionParticleOfCurrentGem = gem.GetComponent<Gem>().explosionParticle;
        Instantiate(explosionParticleOfCurrentGem, gem.transform.position, explosionParticleOfCurrentGem.transform.rotation);
    }

    void HitEffect ()
    {
        Instantiate(hitParticle, transform.position, hitParticle.transform.rotation);
    }
     
    void FireworkEffect() 
    {
        Vector3 offset = new Vector3 (0,5,0);
        Instantiate(fireworksParticle, transform.position + offset, fireworksParticle.transform.rotation);
    }

    // When player collides with a collectable show its coin value in the scene for a certain time
    // When player collides with an obstacle show lost life in the scene for a certain time
    private IEnumerator ShowFloatingText(int value, Vector3 objectPosition)
    {
        Vector3 textOffsetForGems = new Vector3(0,1,1);
        Vector3 textOffsetForObstacles = new Vector3(0,2,2);
        FloatingText.SetActive(true);

        if(value == -1) {
            FloatingText.GetComponentInChildren<TextMeshPro>().text = value.ToString() + " :(";
            FloatingText.GetComponentInChildren<TextMeshPro>().color = Color.red;
            FloatingText.transform.position = objectPosition + textOffsetForObstacles;
        } else {
            FloatingText.GetComponentInChildren<TextMeshPro>().text = "+ " + value.ToString();
             FloatingText.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
            FloatingText.transform.position = objectPosition + textOffsetForGems;
        }

        yield return new WaitForSeconds(floatingTextScreenTime);
        FloatingText.SetActive(false);
    }
}
