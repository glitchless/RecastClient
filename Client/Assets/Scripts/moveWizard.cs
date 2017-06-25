using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWizard : MonoBehaviour {

	public int moveSpeed;
	private Rigidbody2D rb2; 
	private Animator mAnimator;
	private bool facingRight;
	public bool jump;
	public bool grounded;
	public float jumpForce;

	void Awake () 
	{

		facingRight = true;
		grounded = false;
		jump = false;
		rb2 = GetComponent<Rigidbody2D> ();
		mAnimator = GetComponent<Animator> ();
		moveSpeed = 10;
		jumpForce = 10.0f;
		showCoordinates ();
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		grounded = true;
	}


	void Update () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
//		if (rb2.position.x < 104.60 && rb2.position.x > -11.82415) {
			rb2.velocity = new Vector2(moveSpeed * horizontal,rb2.velocity.y);
//		}
		mAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
		flip (horizontal);
		handleWeaponMode ();
		handleJump ();
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

	void handleWeaponMode()
	{
		if(Input.GetKeyDown(KeyCode.Space)){
			mAnimator.SetBool ("attack", true);
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			mAnimator.SetBool ("attack", false);
		}
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
}