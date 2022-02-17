using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float swerveSpeed;
    [SerializeField] private float maxSwerveAmount = 1f;

    [SerializeField] private float swerveAmount;

    [SerializeField] private int celebrationTime = 3;

    [SerializeField] private bool gameOver;
    [SerializeField] private bool shouldPlayerMove;

      [SerializeField] public GameObject FloatingText;
    
    private Vector3 initalPlayerPosition = new Vector3(0,0, -6.75f);

    private SwerveInputSystem swerveInputSystemScript;
    private GameManager gameManagerScript;
    private Animator playerAnimator;
    private Rigidbody playerRb;

    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private ParticleSystem fireworksParticle;
    [SerializeField] private ParticleSystem explosionParticleBlue;
    [SerializeField] private ParticleSystem explosionParticleYellow;

   void Awake() {
        swerveInputSystemScript = GetComponent<SwerveInputSystem>();
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        shouldPlayerMove = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //gameOver = false;
        //playerAnimator.SetBool("isRunning", true);
        //MoveForward();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.z == -6.75f) {
        shouldPlayerMove=true;

        } 
       // if (!gameOver) {
        if (gameManagerScript.isGameActive && shouldPlayerMove){
            // !!daha efficient bi yolu olabilir.
        playerAnimator.SetBool("isRunning", true);
        MoveForward();
        MoveLeftAndRight();
        } else {
            playerRb.velocity = Vector3.zero;
           playerAnimator.SetBool("isRunning", false);
        }
    }

    void MoveForward() {
    //transform.Translate(Vector3.forward * verticalSpeed * Time.deltaTime, Space.World);
    //swerveAmount = swerveSpeed * swerveInputSystemScript.moveFactorX;
    
    //swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
    //playerRb.MovePosition(transform.position +  Vector3.forward * verticalSpeed * Time.deltaTime + Vector3.right*swerveAmount);

    playerRb.velocity = Vector3.forward * verticalSpeed;
    //playerRb.AddForce(Vector3.forward * verticalSpeed  - playerRb.velocity, ForceMode.VelocityChange);
    }

    void MoveLeftAndRight() {
    //float swerveAmount = Time.deltaTime * swerveSpeed * swerveInputSystemScript.moveFactorX;
    

    //swerveAmount = Time.deltaTime * swerveSpeed * swerveInputSystemScript.moveFactorX;
   
   swerveAmount = swerveSpeed * swerveInputSystemScript.moveFactorX;
   swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
    
    //transform.Translate(Vector3.right*swerveAmount);
    //playerRb.MovePosition(transform.position +Vector3.right*swerveAmount);
    //playerRb.AddForce(Vector3.right*swerveAmount, ForceMode.VelocityChange);
    //Debug.Log(swerveInputSystemScript.moveFactorX);
    //Debug.Log(swerveAmount);
    
    playerRb.velocity = playerRb.velocity + Vector3.right * swerveAmount; 
    //playerRb.MovePosition(transform.position + Vector3.right*swerveAmount);
    
    //playerRb.AddForce(Vector3.right*swerveAmount, ForceMode.Impulse);

    }

    void OnCollisionEnter(Collision collision)
    {
     
    if (collision.gameObject.CompareTag("Obstacle")) {
        //gameOver = true;
        //playerAnimator.SetBool("isRunning", false);
        gameManagerScript.UpdateLives();
        collision.gameObject.SetActive(false);

        HitEffect();
        StartCoroutine(ShowFloatingText(-1, collision.gameObject.transform.position));
    }        

    else if (collision.gameObject.CompareTag("Collectable")) {
        ExplosionEffect(collision.gameObject);
        collision.gameObject.SetActive(false);
        gameManagerScript.UpdateCoins(collision.gameObject.GetComponent<Gem>().coinValue);
        StartCoroutine(ShowFloatingText(collision.gameObject.GetComponent<Gem>().coinValue, collision.gameObject.transform.position));

    }        
    
    }

    void OnTriggerEnter(Collider other)
    {
        
        shouldPlayerMove=false;
        playerAnimator.SetTrigger("victory");
        
        FireworkEffect();
        

        playerAnimator.SetBool("isRunning", false);

        transform.GetChild(3).gameObject.SetActive(true);
        StartCoroutine(WaitForAnimantion());
        
        
    }
    IEnumerator WaitForAnimantion()
    {
       
        yield return new WaitForSeconds(celebrationTime);
    

        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.GetComponent<CinematicScene>().ReturnToInitialPosition();  
        gameManagerScript.WinGame();
        transform.position = initalPlayerPosition;
        
    }

     void ExplosionEffect (GameObject gem)
    {
        Instantiate(gem.GetComponent<Gem>().explosionParticle, gem.transform.position, explosionParticleBlue.transform.rotation);
    }

     void HitEffect ()
     {
         Instantiate(hitParticle, transform.position, hitParticle.transform.rotation);
    }
     
     void FireworkEffect() 
     {
          Instantiate(fireworksParticle, transform.position + new Vector3 (0,5,0) , fireworksParticle.transform.rotation);
     }

      private IEnumerator ShowFloatingText(int value, Vector3 objectPosition)
    {
        Vector3 textOffsetForGems = new Vector3( 0,1,1);
        Vector3 textOffsetForObstacles = new Vector3(0,1,1);
        //FloatingText.transform.GetChild(0).GetComponent<TextMeshPro>().text= value.ToString();
        
        
        
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

        
        yield return new WaitForSeconds(0.4f);
        FloatingText.SetActive(false);
    }
}
