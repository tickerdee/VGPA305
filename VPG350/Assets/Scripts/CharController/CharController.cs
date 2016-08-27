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

    public enum AnimState { idle, sneak, walk, run, grapple, struggle, crouch, jump, interact, defeat, victory };
	public AnimState AnimRunTime;

	public float timeMoving;

    public bool CanMove;
    public bool IsRunning;
    public bool IsSneaking;
	public bool IsWalking;

    public bool IsMoving;

    //Player Stamina Count
    public float calc_sta = 1.0f;

    public float SneakSpeed = 5.0f;
    public float WalkSpeed = 10.0f;
	public float RunSpeed = 20.0f;
    private float JumpSpeed = 8.0f;
    private float Gravity = 20.0f;

    // If set true, diagonal speed can't exceed normal move speed
    public bool limitDiagonalSpeed = true;

    // If set true, the run key toggles between running and walking
    public bool toggleRun = false;

    // If set true, player can change direction while in the air
    public bool airControl = false;

    // Number of frames before being able to jump again; set to 0 to allow no wait time
    public int JumpFactor = 1;

    private Vector3 moveDirection = Vector3.zero;
    public CharacterController Controller;
    private Transform myTransform;
    private bool PlayerControlled = false;
    private int JumpTimer;

	public bool IsControlled; // sets if character is being player controlled

    //Mouse Look Variables
	public Camera cam;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 0.01F;
    public float sensitivityY = 0.01F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;
    private object controller;
    private int jumpTimer;
    private float jumpSpeed;
    private bool useMouseLook;
	MouseLook mouseLook;
	public bool canMove;


    // Use this for initialization
    void Start () {

        IsRunning = false;
        canMove = true;
		useMouseLook = true;

        myTransform = transform;
        jumpTimer = JumpFactor;

        Rigidbody rb = GetComponent<Rigidbody>();
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

    }

    void Control() {

        Controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

		//if (canMove == true) {
			
			if (Input.GetKey (KeyCode.Space)) {
				AnimRunTime = AnimState.jump;
			}

			if (Input.GetKey (KeyCode.Q)) {
				IsSneaking = true;
				IsRunning = false;
				IsWalking = false;
				AnimRunTime = AnimState.sneak;
			} else if (Input.GetKey (KeyCode.LeftShift)) { 
				IsRunning = true;
				IsSneaking = false;
				IsWalking = false;
				AnimRunTime = AnimState.run;
			} else if (IsMoving == true) {
				IsWalking = true;
				IsRunning = false;
				IsSneaking = false;
				AnimRunTime = AnimState.walk;
			} else {
				IsMoving = false;
				AnimRunTime = AnimState.idle;
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
			// Jump! But only if the jump button has been released and player has been grounded for a given number of frames
			if (Input.GetKey (KeyCode.Space)) {
				jumpTimer++;
			} else if (jumpTimer >= JumpFactor) {
				moveDirection.y = jumpSpeed;
				jumpTimer = 0;
			}
		//}



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


        Controller.Move(moveDirection * Time.deltaTime);
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


        if (canMove)
        {
            Control();
            RotateView();

            // Apply gravity
            moveDirection.y -= Gravity * Time.deltaTime;
        }
    }

}
