using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    private Rigidbody rigidbody;
    private Animator animator;
    
    [Header("Colliders")]
    [SerializeField] private Collider rootCollider;
    [SerializeField] private List<Collider> ragdollParts = new();
    
    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody>();
        this.animator = GetComponentInChildren<Animator>();

        this.rootCollider = GetComponent<Collider>();
        SetRagdollParts();
        
    }

    public void TurnRagdollOn()
    {
        animator.enabled = false;
        rootCollider.enabled = false;
        rigidbody.useGravity = false;
        
        foreach (Collider collider in ragdollParts)
        {
            collider.isTrigger = false;
            collider.attachedRigidbody.useGravity = true;
            collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
    
    private void SetRagdollParts() {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.attachedRigidbody.useGravity = false;
                collider.isTrigger = true;
                ragdollParts.Add(collider);
            }
        }
    }
}
