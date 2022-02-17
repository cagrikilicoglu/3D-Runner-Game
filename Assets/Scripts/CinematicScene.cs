using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicScene : MonoBehaviour
{

    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Quaternion initialRotation;
    
    void Awake()
    {
    // Hold the initial position and rotation of the cinematic camera
    initialPosition = transform.position;
    initialRotation = transform.rotation;
    }

    void Update()
    {
        // when the cinematic camera is activated make cinematic movement
        if (gameObject.activeSelf) {
        CinematicMovement();
        } 
    }

    void CinematicMovement()
    {
        transform.Translate(Vector3.right * -verticalSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.World);
    }

    // After the cinematic camera is off return it to initial position for the next time
    public void ReturnToInitialPosition() {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
