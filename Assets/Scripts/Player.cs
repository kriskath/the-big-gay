﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour 
{
    private int health = 10; //Do we need this?
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float vertical;
    private float horizontal;
    public bool canMove = true;
    private Animator animator;

	// Use this for initialization
	void Start () 
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        HandleAnimation();

        UpdateMovement();

        CheckInteract();
    }

    private void HandleAnimation()
    {
        if (canMove) {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
            switch ((int) vertical) {
                case (-1):
                    animator.SetBool("WalkDown", true);
                    animator.SetBool("WalkUp", false);
                    break;
                case (1):
                    animator.SetBool("WalkUp", true);
                    animator.SetBool("WalkDown", false);
                    break;
                default:
                    animator.SetBool("WalkDown", false);
                    animator.SetBool("WalkUp", false);
                    break;
            }

            switch ((int)horizontal) {
                case (-1):
                    animator.SetBool("WalkLeft", true);
                    animator.SetBool("WalkRight", false);
                    break;
                case (1):
                    animator.SetBool("WalkRight", true);
                    animator.SetBool("WalkLeft", false);
                    break;
                default:
                    animator.SetBool("WalkLeft", false);
                    animator.SetBool("WalkRight", false);
                    break;
            }
        }
    }

    //Updates player movement and animations
    private void UpdateMovement()
    {
        //clamp movement
        Vector2 movement = new Vector2(horizontal, vertical);
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        rb.velocity = movement;
    }

    public void StopMoving()
    {
        horizontal = 0;
        vertical = 0;
        canMove = false;
    }

    private void CheckInteract()
    {
        if (Input.GetKeyDown("e")) 
        {
            Interact();
        }
    }

    void Interact()
    {

        //TODO: Check for interactable object and call .Interact()
    }
}