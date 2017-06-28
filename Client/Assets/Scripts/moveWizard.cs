using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWizard : MonoBehaviour {
    public GameObject spell;
    public int moveSpeed;
	private Rigidbody2D rb2; 
	private Animator mAnimator;
	private bool facingRight;
	public bool jump;
	public bool grounded;
    public bool casting = false;
    bool firstCast = true;
    float castingFor = 0f;
	public float jumpForce;
    System.Random rnd;
    bool lookUp;

    void Awake () 
	{
		facingRight = true;
		grounded = false;
		jump = false;
		rb2 = GetComponent<Rigidbody2D> ();
		mAnimator = GetComponent<Animator>();
		moveSpeed = 10;
		jumpForce = 10.0f;
		showCoordinates ();
        rnd = new System.Random();
    }

	void OnCollisionEnter2D(Collision2D hit)
	{
		grounded = true;
	}


    void Update() {
        if (!casting) {
            float horizontal = Input.GetAxis("Horizontal");
            //		if (rb2.position.x < 104.60 && rb2.position.x > -11.82415) {
            rb2.velocity = new Vector2(moveSpeed * horizontal, rb2.velocity.y);
            //		}
            mAnimator.SetFloat("speed", Mathf.Abs(horizontal));
            //Debug.Log(mAnimator.GetFloat("speed"));
            lookUp = false;
            if (rnd.Next(0, 9) < 2)
                lookUp = true;
            mAnimator.SetBool("lookUp", lookUp);

            flip(horizontal);
            handleCasting();
            handleJump();
        }
        else {
            //castingFor = mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            //if (castingFor < 0.9) {
            //    if (castingFor > 0.4 && firstCast) {
            //        cast();
            //        firstCast = false;
            //    }
            //}
            //else {
            //    Debug.Log("Casting ended");
            //    mAnimator.SetBool("casting", false);
            //    casting = false;
            //    firstCast = true;
            //}
        }
	}

    private void cast() {
        //var lookPos = target.position - transform.position;
        //lookPos.y = 0;
        //var rotation = Quaternion.LookRotation(lookPos);
        //rotation *= Quaternion.Euler(0, 90, 0); // this add a 90 degrees Y rotation
        if (facingRight) {
            Instantiate(spell, this.gameObject.transform.position, Quaternion.Euler(0, 90, 90));
        }
        else {
            Instantiate(spell, this.gameObject.transform.position, Quaternion.Euler(0, -90, 90));
        }
    }

    void FixedUpdate()
	{
		if(jump == true)
		{
			rb2.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);
			jump = false;
		}
	}

	void handleJump()
	{
		if (Input.GetKeyDown(KeyCode.W) && grounded) {
			jump = true;
			grounded = false;
		}
	}

	void showCoordinates()
	{
		Debug.Log (rb2.position.x);
		Debug.Log (rb2.position.y);
	}

    void castingStop() {
        Debug.Log("Stop cast");
        casting = false;
        mAnimator.SetBool("casting", false);
    }

	void handleCasting()
	{
		if(Input.GetKeyDown(KeyCode.Space)){
            casting = true;
            mAnimator.SetBool("casting", true);
            castingFor = 0f;
            Debug.Log("Casting started");
            stopMovement();
        }
		//if (Input.GetKeyUp (KeyCode.Space)) {
		//	mAnimator.SetBool ("attack", false);
		//}
	}

	void flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}
    
    void stopMovement() {
        rb2.velocity = Vector2.zero;
        mAnimator.SetFloat("speed", 0);
    }
}