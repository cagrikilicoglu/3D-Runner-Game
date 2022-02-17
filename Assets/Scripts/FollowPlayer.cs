using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 3, -7);

    // private Vector3 desiredPosition;
    private float velocity;
    // [SerializeField] private float smoothTime;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 20;
        // smoothTime = 1f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Offset the camera behind the player by adding camera position at the beginning
        
        // Vector3 desiredPosition  =player.transform.position + offset;
        // transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.position = player.transform.position + offset;
        //transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, velocity* Time.deltaTime) ;   
        // transform.LookAt(player.transform);
      
    }
}
