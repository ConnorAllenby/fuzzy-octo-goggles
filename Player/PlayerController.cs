using UnityEngine;
using System.Collections;
using DebugSystemCollections;
[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	// Floats
	[Range(0,20)]
	public float movementSpeed;
	[Range(0,10)]
	public float mouseSensitivity;
	[Range(0,30f)]
	public float jumpSpeed;

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

    private PlayerBaseState currentState;
    public readonly Player_IdleState playerIdleState = new Player_IdleState();
    public readonly Player_RunningState playerRunningState = new Player_RunningState();
    public readonly Player_JumpingState playerJumpingState = new Player_JumpingState();


    void Awake()
    {
        TransitionToState(playerIdleState);
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
		Movement();

	}

    public void TransitionToState(PlayerBaseState state)
    {
        currentState = state;

        currentState.EnterState(this);
    }


	public void Rotation()
	{
		// Rotation
		
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
	}

	public void Movement()
	{
		// Movement
		if(playerIsSprinting)
		{
			//forwardSpeed = Input.GetAxis("Vertical") * sprintSpeed;
		}
		else
		{
			
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
