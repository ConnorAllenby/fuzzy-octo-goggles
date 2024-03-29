﻿using UnityEngine;
using System.Collections;
using DebugSystemCollections;
[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	// Floats
	[Range(0,20)]
	public float movementSpeed;
	[Range(0,10)]
	public float mouseSensitivity;
    [Range(0, 10)]
    public float ADSSensitivity;
    public float currentSensitivity;
	[Range(0,30f)]
	public float jumpSpeed;

    [Range(0, 20)]
    public float walkSpeed;
    [Range(0,20)]
	public float sprintSpeed;

    
	
	// DO NOT TOUCH
	public float sideSpeed;
	public float forwardSpeed; 
	public float verticalRotation = 0;
	public float upDownRange = 60.0f;
	
	public float verticalVelocity = 0;


    //Weapons

    public GameObject weapon1;
    public GameObject weapon2;
    [SerializeField]
    GameObject lastWeapon;
	// Character Controller
	public CharacterController characterController;
	
	// Transforms.

	// Bools
	public  bool playerIsSprinting = false;

	
	// Vector3's
	public Vector3 speed;

    //FSM

    public PlayerBaseState currentState;
    public readonly Player_IdleState playerIdleState = new Player_IdleState();
    public readonly Player_RunningState playerRunningState = new Player_RunningState();
    public readonly Player_JumpingState playerJumpingState = new Player_JumpingState();
    public readonly Player_SprintState playerSprintState = new Player_SprintState();

    void Awake()
    {
        movementSpeed = walkSpeed;
        TransitionToState(playerIdleState);
        currentSensitivity = mouseSensitivity;
    }

	// Use this for initialization
	void Start () {
		
		characterController = GetComponent<CharacterController>();


		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () 
	{
        PlayerInputs();
        currentState.UpdateState(this);
    }


	private void FixedUpdate() 
	{
		Rotation();

	}

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;

        currentState.EnterState(this);
    }


	public void Rotation()
	{
		// Rotation
		
		float rotLeftRight = Input.GetAxis("Mouse X") * currentSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		
		verticalRotation -= Input.GetAxis("Mouse Y") * currentSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}

	public void Movement()
	{
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        speed = transform.rotation * speed;
        characterController.Move(speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = sprintSpeed;
        }
        else
        {
            movementSpeed = walkSpeed;
        }
    }

	public void PlayerInputs()
	{
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon2.SetActive(false);
            weapon1.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon1.SetActive(false);
            weapon2.SetActive(true);
        }

	}
	
}
