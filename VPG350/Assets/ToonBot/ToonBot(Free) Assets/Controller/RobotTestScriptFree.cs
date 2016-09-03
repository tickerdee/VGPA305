using UnityEngine;
using System.Collections;

public class RobotTestScriptFree : MonoBehaviour {

	private Animator anim;
    private Rigidbody rbody;
    //private float jumpTimer = 0;
    private float moveH;
    private float moveV;

    public float moveX;
    public float moveZ;


    
	void Start ()
    {
	    anim = GetComponent<Animator> ();
        rbody = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
    {
	
		//Controls the Input for running animations
		//WASD: Movement

			
        if(Input.GetKey(KeyCode.W))
        {
            anim.Play("sprint_Anim", 0,0f);// -1 is the layer of animation, Base Layer is usually -1, 0f is the start of the animation in a range of 0 to 1.
        }
        else if(Input.GetKey(KeyCode.A))
        {
            anim.Play("sprint_Anim", 0, 0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.Play("sprint_Anim", 0, 0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.Play("sprint_Anim", 0, 0f);
        }

        moveH = Input.GetAxis("Horizontal");
        moveV = Input.GetAxis("Vertical");

        anim.SetFloat("moveH", moveH);
        anim.SetFloat("moveV", moveV);

        if(moveZ<= 0f)
        {
            moveX = 0f;
        }

        moveX = moveH * 320f * Time.deltaTime;
        moveZ = moveV * 320f * Time.deltaTime;

        //rbody.velocity = new Vector3(moveX, 0f, moveZ);
        rbody.AddForce(moveX, 0f, moveZ, ForceMode.VelocityChange);

    }
}
