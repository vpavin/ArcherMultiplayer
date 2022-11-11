using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Pavos.Archer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour
{

    [Header("Body")]
    [SerializeField] private Transform followTransform;
    
    [Header("Stats")]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationPower = 2f;
    [SerializeField] private float rotationLerp = .5f;
    
    [Header("Flags")]
    [SerializeField] private bool grounded;

    private Animator _animator;
    
    // Controllers
    private CameraController _cameraController;
    
    // Actions
    private InputAction _movementAction;
    private InputAction _cameraAction;
    private InputAction _aimAction;
    
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    
    private static readonly int IsWalking = Animator.StringToHash("Walking");
    private static readonly int IsAiming = Animator.StringToHash("Aiming");
    private static readonly int MovementX = Animator.StringToHash("MovementX");
    private static readonly int MovementY = Animator.StringToHash("MovementY");

    void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _animator = GetComponentInChildren<Animator>();

        _cameraController = GetComponent<CameraController>();

        _movementAction = playerInput.actions["Movement"];
        _cameraAction = playerInput.actions["Camera"];
        _aimAction = playerInput.actions["Aim"];
        
        grounded = true;
        _targetPosition = transform.position;
        _targetRotation = followTransform.rotation;
    }
    
    void Update()
    {
        Vector2 movementInput = _movementAction.ReadValue<Vector2>();
        Vector2 rotationInput = _cameraAction.ReadValue<Vector2>();
        bool isAiming = _aimAction.inProgress;
        
        HandleMovement(movementInput, isAiming);
        
        _cameraController.HandleRotation(rotationInput, rotationPower); 
        _cameraController.HandleAiming(isAiming);

        _animator.SetFloat(MovementX, movementInput.x);
        _animator.SetFloat(MovementY, movementInput.y);
        _animator.SetBool(IsWalking, movementInput.magnitude > 0);
        _animator.SetBool(IsAiming, isAiming);
        transform.position = _targetPosition;
    }
    
    void HandleMovement(Vector2 movementInput, bool isAiming)
    {
        _targetRotation = Quaternion.Lerp(followTransform.transform.rotation, _targetRotation, Time.deltaTime * rotationLerp);
        var angles = followTransform.transform.localEulerAngles;
        
        if (movementInput.x == 0 && movementInput.y == 0) 
        {   
            _targetPosition = transform.position;

            if (isAiming)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return; 
        }
        
        float moveSpeed = movementSpeed / 100f;
        Vector3 position = (transform.forward * movementInput.y * moveSpeed) + (transform.right * movementInput.x * moveSpeed);
        _targetPosition = transform.position + position;
        
        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
        
    }


}
