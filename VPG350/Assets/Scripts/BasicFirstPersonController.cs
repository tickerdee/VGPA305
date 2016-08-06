using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class BasicFirstPersonController : MonoBehaviour {

    public bool useMouseLook = true;
    MouseLook mouseLook;
    public Camera cam;

    Rigidbody rg;

    public float speed, walkSpeed, runspeed, standingTurnSpeed;

    public float walkToRunRate = .02f, shiftRunRateModifer = .05f, shiftRunSpeedModifer = 20;

    float shiftModifier;

    bool canMove, interacting;//Not implemented

    float timeMoving;

    // Use this for initialization
    void Start () {

        rg = GetComponent<Rigidbody>();

        //walkSpeed = 100;
        //runspeed = 400;
        //standingTurnSpeed = 3;

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
        

        canMove = true;
        interacting = false;
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
	void Update () {

        if (canMove)
        {
            RotateView();
            rg.angularVelocity = Vector3.zero;
            float rotation = 0;
            Vector3 movementDirection = Vector3.zero;

            rg.velocity = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                timeMoving += shiftRunRateModifer;
                shiftModifier = shiftRunSpeedModifer;
            }
            else
            {
                //timeMoving = 0;
                shiftModifier = 0;
            }

            bool hasMovementInput = false;
            movementDirection = Vector3.zero;

            if (Input.GetKey("w"))
            {
                hasMovementInput = true;
                movementDirection += transform.forward;
            }
            if (Input.GetKey("s"))
            {
                hasMovementInput = true;
                movementDirection += transform.forward * -1;
            }

            if (Input.GetKey("d"))
            {
                hasMovementInput = true;
                movementDirection += transform.right;
            }
            if (Input.GetKey("a"))
            {
                hasMovementInput = true;
                movementDirection += transform.right * -1;
            }

            movementDirection.Normalize();

            if (hasMovementInput)
            {
                timeMoving += walkToRunRate;
            }
            else{
                movementDirection = Vector3.zero;
                timeMoving = 0;
            }

            if (Input.GetKey("q"))
            {
                //rotation = -standingTurnSpeed;
            }
            else if (Input.GetKey("e"))
            {
                //rotation = standingTurnSpeed;
            }

            transform.Rotate(0, rotation, 0);

            timeMoving = Mathf.Clamp(timeMoving, 0, 1);
            Vector3 movement = Mathf.Lerp(walkSpeed, runspeed + shiftModifier, timeMoving) * movementDirection;

            rg.AddForce(movement);
            speed = movement.magnitude;

            interacting = false;
        }
        else
        {

            interacting = true;
            rg.velocity = Vector3.zero;

        }

    }//End Update

    private void RotateView()
    {
        //avoids the mouse looking if the game is effectively paused
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        // get the rotation before it's changed
        float oldYRotation = transform.eulerAngles.y;

        if (useMouseLook)
        {
            if(mouseLook == null)
            {
                mouseLook = new MouseLook();
                mouseLook.Init(transform, cam.transform);
            }
            mouseLook.LookRotation(transform, cam.transform);
        }

        /*
        if (m_IsGrounded || advancedSettings.airControl)
        {
            // Rotate the rigidbody velocity to match the new direction that the character is looking
            Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
            m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
        }
        */
    }
}
