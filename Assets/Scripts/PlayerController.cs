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
    [SerializeField] private float rotationSpeed = 5f;
    
    [Header("Flags")]
    [SerializeField] private bool grounded;

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    
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
        float targetAngle = _cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleAiming()
    {
        aimVirtualCamera.Priority = _aimAction.inProgress ? 11 : 9;
    }
}
