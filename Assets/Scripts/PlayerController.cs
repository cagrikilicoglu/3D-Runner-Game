using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float swerveSpeed;
    [SerializeField] private float maxSwerveAmount = 1f;


    [SerializeField] private int celebrationTime = 3;

    [SerializeField] private bool gameOver;
    [SerializeField] private bool shouldPlayerMove;
    
    private Vector3 initalPlayerPosition = new Vector3(0,0, -6.75f);

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
    // Start is called before the first frame update
    void Start()
    {
        
        //gameOver = false;
        //playerAnimator.SetBool("isRunning", true);
        //MoveForward();
        
    }

    // Update is called once per frame
    void Update()
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
           //playerAnimator.SetBool("isRunning", false);
        }
    }

    void MoveForward() {
    transform.Translate(Vector3.forward * verticalSpeed * Time.deltaTime, Space.World);
    //playerRb.velocity = Vector3.forward * verticalSpeed;
    }

    void MoveLeftAndRight() {
    float swerveAmount = Time.deltaTime * swerveSpeed * swerveInputSystemScript.moveFactorX;
    swerveAmount = Mathf.Clamp(swerveAmount, -maxSwerveAmount, maxSwerveAmount);
    //Debug.Log(swerveInputSystemScript.moveFactorX);
    //Debug.Log(swerveAmount);
    
    //playerRb.velocity
    transform.Translate(Vector3.right*swerveAmount);

    }

    void OnCollisionEnter(Collision collision)
    {
     
    if (collision.gameObject.CompareTag("Obstacle")) {
        //gameOver = true;
        playerAnimator.SetBool("isRunning", false);
        gameManagerScript.UpdateLives();
    }        

    else if (collision.gameObject.CompareTag("Collectable")) {
        

        collision.gameObject.SetActive(false);
        gameManagerScript.UpdateGem(1);

    }        
    
    }

    void OnTriggerEnter(Collider other)
    {
        
        shouldPlayerMove=false;
        playerAnimator.SetTrigger("victory");
        playerAnimator.SetBool("isRunning", false);
        StartCoroutine(WaitForAnimantion());
        
        
    }
    IEnumerator WaitForAnimantion()
    {
        Debug.Log("okkkkk");

        yield return new WaitForSeconds(celebrationTime);
        gameManagerScript.WinGame();
        transform.position = initalPlayerPosition;
        
    }

}
