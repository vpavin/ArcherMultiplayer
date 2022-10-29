using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float playerSpeed = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    
    [Header("Flags")]
    [SerializeField] private bool grounded;

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private Transform followTransform;

    // Actions
    private InputAction _movementAction;
    private InputAction _cameraAction;
    private InputAction _aimAction;

    private Transform _cameraTransform;
    
    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _movementAction = playerInput.actions["Movement"];
        _cameraAction = playerInput.actions["Camera"];
        _aimAction = playerInput.actions["Aim"];
        
        _cameraTransform = Camera.main.transform;
        
        grounded = true;
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleAiming();
    }

    void HandleMovement()
    {
        Vector2 input = _movementAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * _cameraTransform.right.normalized + move.z * _cameraTransform.forward.normalized;
        move.y = 0f;
        
    }

    void HandleRotation()
    {
//        float targetAngle = _cameraTransform.eulerAngles.y;
//        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
//        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector2 input = _cameraAction.ReadValue<Vector2>();
        
        followTransform.transform.rotation *= Quaternion.AngleAxis(input.x * rotationSpeed, Vector3.up);
        followTransform.transform.rotation *= Quaternion.AngleAxis(input.y * rotationSpeed, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;
        
        if (angles.x is > 180 and < 340)
        {
            angles.x = 340;
        } else if (angles.x is < 180 and > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;

    }

    void HandleAiming()
    {
        aimVirtualCamera.Priority = _aimAction.inProgress ? 11 : 9;
    }
}
