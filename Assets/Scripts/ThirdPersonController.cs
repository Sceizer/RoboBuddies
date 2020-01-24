using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Character movement")]
    public float movementSpeed = 5f;
    private Rigidbody objectRb;

    [Header("Character camera")]
    public bool usesCamera = true;
    public Transform characterCamera;
    public float cameraSensitivity = 5f;
    public float cameraDistance = 5f;

    private Vector2 CameraRotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        if(characterCamera.GetComponent<Camera>() == null)
        {
            Debug.LogError(name + " ThirdpersonController: Current character Camera is not a camera object!");
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        CameraRotation.x -= Input.GetAxis("Mouse X") * cameraSensitivity;
        CameraRotation.y -= Input.GetAxis("Mouse Y") * cameraSensitivity;

        ApplyMovement(hori, vert);
        CalculateNewCameraPosition();
    }

    void ApplyMovement(float horizontalMovement, float verticalMovement)
    {
        Vector3 newVel = new Vector3(horizontalMovement, 0, verticalMovement) * movementSpeed;
        //Setting the Y axis to the current object velocity so that falling down can still work normaly
        newVel.y = objectRb.velocity.y;
        objectRb.velocity = newVel;
    }

    void CalculateNewCameraPosition()
    {
        Vector3 cameraDirection = transform.forward * -1f;
        //Rotate the direction so that the camera can look at different angles
        cameraDirection = Quaternion.Euler(CameraRotation.y, CameraRotation.x,0) * cameraDirection;

        //Calculate distance between the character and the camera
        Ray collisionTestRay = new Ray(transform.position, cameraDirection);
        RaycastHit collisionInformation;
        float distanceTheCameraCanGoBack = cameraDistance;

        //See how far the camera can go before hitting a object
        if (Physics.Raycast(collisionTestRay, out collisionInformation))
        {
            distanceTheCameraCanGoBack = collisionInformation.distance;

            if(distanceTheCameraCanGoBack > cameraDistance)
            {
                distanceTheCameraCanGoBack = cameraDistance;
            }
        }

        characterCamera.position = transform.position + cameraDirection * distanceTheCameraCanGoBack;
        characterCamera.LookAt(transform.position);
    }
}
