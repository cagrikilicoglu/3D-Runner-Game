using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicScene : MonoBehaviour
{

    [SerializeField] private float verticalSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Quaternion initialRotation;
    // Start is called before the first frame update
    void Awake()
    {
    
    initialPosition = transform.position;
    initialRotation = transform.rotation;
    
    
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) {
        CinematicMovement();
        } 
    }

    void CinematicMovement()
    {
        transform.Translate(Vector3.right * -verticalSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.World);
    }

    public void ReturnToInitialPosition() {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
