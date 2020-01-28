﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("General settings")]
    public bool isPlayerControlled = true;

    [Header("Character movement")]
    public float movementSpeed = 5f;
    public float jumpStrength = 5f;
    float distToGround = 0;

    private Rigidbody objectRb;

    [Header("Character camera")]
    public bool usesCamera = true;
    public Transform characterCamera;
    public float cameraSensitivity = 5f;
    public float cameraDistance = 5f;
    public Vector2 pitchLimit = new Vector2(-45f, 90f);

    private Vector2 CameraRotation = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        if(characterCamera.GetComponent<Camera>() == null)
        {
            Debug.LogError(name + " ThirdpersonController: Current character Camera is not a camera object!");
        }
        distToGround = GetComponent<Collider>().bounds.extents.y;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Movement(Vector2 newDir)
    {
        Vector3 movementDirection = Quaternion.Euler(0, CameraRotation.x, 0) * new Vector3(newDir.x, 0, newDir.y);
        Vector3 newVel = movementDirection * movementSpeed;
        //Setting the Y axis to the current object velocity so that falling down can still work normaly
        newVel.y = objectRb.velocity.y;
        objectRb.velocity = newVel;

        //Make sure the player faces the way he is moving
        transform.rotation = Quaternion.Euler(0, CameraRotation.x, 0);
    }
    
    public void Jump()
    {
        if (!CheckGrounded())
        {
            return;
        }
        Vector3 newVel = objectRb.velocity;
        newVel.y = jumpStrength;
        objectRb.velocity = newVel;
    }

    bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public void RotateCameraView(Vector2 newCamMovement)
    {
        Debug.Log(newCamMovement);
        //Get mouse movement for this frame
        CameraRotation.x += newCamMovement.x * cameraSensitivity;
        CameraRotation.y -= newCamMovement.y * cameraSensitivity;

        //Limit the angle of the Y angle of the camera
        CameraRotation.y = Mathf.Clamp(CameraRotation.y, pitchLimit.x, pitchLimit.y);

        Vector3 cameraDirection = Vector3.forward * -1f;
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
        //Setting the rotation of the camera so that it is still looking at the player
        characterCamera.rotation = Quaternion.Euler(CameraRotation.y, CameraRotation.x, 0);
    }
}
