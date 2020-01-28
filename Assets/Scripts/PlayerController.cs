﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public class Vector2Event : UnityEvent<Vector2>
{}

public class PlayerController : MonoBehaviour
{
    InputManager controls;

    public Vector2Event movementEvent;
    public Vector2Event mouseMovementEvent;
    public UnityEvent interactEvent;

    private void Awake()
    {
        controls = new InputManager();
        controls.Enable();

        controls.Player.Interact.performed += ctx => interactEvent.Invoke();

        controls.Player.Movement.performed += ctx => movementEvent.Invoke(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => movementEvent.Invoke(new Vector2(0,0));

        //controls.Player.MouseMovement.performed += ctx => mouseMovementEvent.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
