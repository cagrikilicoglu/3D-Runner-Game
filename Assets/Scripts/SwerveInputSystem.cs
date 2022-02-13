using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveInputSystem : MonoBehaviour
{
   private float lastFrameMousePositionX;
   public float moveFactorX;

    void Update()
    {
    if( Input.GetMouseButtonDown(0)) {
        lastFrameMousePositionX = Input.mousePosition.x;
    }   
    else if (Input.GetMouseButton(0)) {
        moveFactorX = Input.mousePosition.x - lastFrameMousePositionX;
        lastFrameMousePositionX = Input.mousePosition.x;
    } 
    else if (Input.GetMouseButtonUp(0)) {

    
        moveFactorX = 0f;

    }
    }
}
