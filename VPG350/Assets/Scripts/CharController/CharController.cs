using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class CharController : MonoBehaviour {

    /*
        States:
        
        Idle (No Sound)
        Crouch Sneak (Low Sound)
        Walk/Jog (Normal Sound)
        Sprint (High Sound)
        
        Controls for PC
        *Mouse Look
        *W,A,S,D Movement
        *C crouch
        *Hold control to Sneak Walk at SSpeed
        *Hold Shift to Sprint at FSpeed
        *Tab to open Map
        *Spacebar to Jump
        *E to interact
        *Left & Right mouse buttons will be QTE event buttons to break free

        Controls for Xbox Controller
        *Right Stick Look
        *Left Stick Movement
        *B is crouch
        *<50% stick movement to Sneak Walk at SSpeed
        *Press LS to Sprint at FSpeed
        *Back button to open Map
        *A to Jump
        *X to interact
        *Left & Right Triggers will be QTE event buttons to break free

        Controls for PS Controller
        *RS Look
        *LS Movement
        *O crouch
        *<50% stick mvement to Sneak Walk at SSpeed
        *L3 to Sprint at FSpeed
        *PS Pad to open Map
        *X to Jump
        *Square to interact
        *L2 & R2 buttons will be QTE event buttons to break free
    */

    // Char Variables

	// Anim State Variables
    public enum AnimState { idle, sneak, walk, run, struggle, struggleLose };
    // Crouch, Win, Lose, Interact, Jump
	public AnimState animState;
    public AnimState OldAnimState;

	// Movement Bools
    public bool CanMove;
    public bool IsRunning;
    public bool IsSneaking;
	public bool IsWalking;
	public bool WasRunning;
	public bool IsMoving;

    //Player Stamina Count
    public float calc_sta = 1.0f;

	// Player Speed Variables
    public float SneakSpeed = 5.0f;
    public float WalkSpeed = 10.0f;
	public float RunSpeed = 20.0f;
    //private float JumpSpeed = 8.0f;
    //private float Gravity = 20.0f;

	// Number of frames before being able to jump again; set to 0 to allow no wait time
    //public int JumpFactor = 1;

	// Movement Variables
    private Vector3 moveDirection = Vector3.zero;
    public CharacterController Controller;
    private Transform myTransform;
    //private int JumpTimer;
	Quaternion originalRotation;
	public bool canMove;

    //Mouse Look Variables
	public Camera cam;
	public bool useMouseLook;
	MouseLook mouseLook;

    
	// Char Parts Variables
	public Rigidbody rb;
	public Animator animator;

    // State Event
    public static event StateChanged Event_StateChanged;

    public delegate void StateChanged(AnimState NeoAnimState);


    // Use this for initialization
    void Start () {

        IsRunning = false;
        canMove = true;

        rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;

		if(cam != null)
		{
			if (useMouseLook)
			{
				mouseLook = new MouseLook();
				mouseLook.Init(transform, cam.transform);
			}
		}
		else
		{
			Debug.Log("Basic First Person Controller has no camera");
		}

        if(animator == null)
        {
            animator = this.GetComponent<Animator>();
        }

        Event_StateChanged += MyStateChanged;
    }

    void MyStateChanged(AnimState newState)
    {
        if (animator != null)
        {
            animator.SetInteger("AnimState", (int)newState);
        }
    }

    void Control() {

        Controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
			
		if (Input.GetKey (KeyCode.Space)) {
			//AnimRunTime = AnimState.jump;
		}

		if (Input.GetKey (KeyCode.Q)) {
			IsSneaking = true;
			IsRunning = false;
			IsWalking = false;
			animState = AnimState.sneak;
		} else if (Input.GetKey (KeyCode.LeftShift)) { 
			IsRunning = true;
			IsSneaking = false;
			IsWalking = false;
            animState = AnimState.run;
		} else if (IsMoving == true) {
			IsWalking = true;
			IsRunning = false;
			IsSneaking = false;
            animState = AnimState.walk;
		} else {
			IsMoving = false;
            animState = AnimState.idle;
		}

		if (!IsRunning && WasRunning) {
			rb.velocity = rb.velocity * .5f;
		}

		if (Input.GetKey (KeyCode.W)) {
			moveDirection += transform.forward;
			//Debug.Log("Walk Forward");
			IsMoving = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			moveDirection += transform.forward * -1;
			//Debug.Log("Walk Back");
			IsMoving = true;
		}

		if (Input.GetKey (KeyCode.D)) {
			moveDirection += transform.right;
			//Debug.Log("Walk Right");
			IsMoving = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			moveDirection += transform.right * -1;
			//Debug.Log("Walk Left");
			IsMoving = true;
		}

        //moveDirection = transform.TransformDirection(moveDirection);
		moveDirection.Normalize();

        if(IsRunning == true)
        {
            moveDirection *= RunSpeed;
		} else if (IsSneaking == true)
        {
            moveDirection *= SneakSpeed;
		} else if (IsWalking == true)
		{
			moveDirection *= WalkSpeed;
		}

		if (moveDirection != Vector3.zero) {
			rb.AddForce (moveDirection * .6f);
        } else {
			rb.velocity = Vector3.zero;
		}

        if (animator != null)
        {
            animator.SetFloat("moveV", moveDirection.magnitude / 10.0f);
        }

        WasRunning = IsRunning;
        //Controller.Move(moveDirection * Time.deltaTime);
		//Debug.Log ("IsWalking " + IsWalking + " IsRunning " + IsRunning + " IsSneaking " + IsSneaking);
    }

	private void RotateView()
	{
		//avoids the mouse looking if the game is effectively paused
		if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

		if (useMouseLook)
		{
			if(mouseLook == null)
			{
				mouseLook = new MouseLook();
				mouseLook.Init(transform, cam.transform);
			}
			mouseLook.LookRotation(transform, cam.transform);
		}
	}

	public void lockPlayerControls()
	{
		canMove = false;
		mouseLook.SetCursorLock(false);
	}

	public void unlockPlayerControls()
	{
		canMove = true;
		mouseLook.SetCursorLock(true);
	}

    // Update is called once per frame
    void Update() {

        if (OldAnimState != animState)
        {
            Event_StateChanged(animState);
        }

        if (canMove)
        {
            Control();
            RotateView();

            // Apply gravity
            //moveDirection.y -= Gravity * Time.deltaTime;
        }

        OldAnimState = animState;
    }

}
