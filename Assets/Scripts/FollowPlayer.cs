using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 3, -7);

    void LateUpdate()
    {
        // Offset the camera behind the player by adding camera position at the beginning
        transform.position = player.transform.position + offset;
        
      
    }
}
