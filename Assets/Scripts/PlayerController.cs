using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    bool grounded;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;
    AudioSource audio;

    Rigidbody2D myRb;
    Animator myAnim;
    bool facingRight = true;
    int blinkCount = 0;
    int blinkLimit = 2;
    int frameCount = 0;
    int shadowDelay = 120;

    public ShadowRecording shadow;

    public float shadowJumpForce;

    // public static PlayerController instance = null;


    //    void Awake() {
    //        if (instance == null) {
    //            instance = this;
    //        }    
    //        else if (instance != this) {
    //            Destroy(gameObject);
    //        }
    //    }
    // Use this for initialization
    void Start () {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        //shadow = ShadowRecording.instance;
    }

    void Update () {
        if (shadow == null) {
            return;
        }
        if (grounded && Input.GetButtonDown("Jump")) {
            grounded = false;
            myAnim.SetBool("isGrounded", grounded);
            myRb.AddForce(new Vector2(0, jumpHeight));
        }

        if (Input.GetButtonDown("Blink") && blinkCount < blinkLimit) {  
            audio.Play();
            blinkCount++;
            transform.position = shadow.transform.position;

            // add artificial jump force if shadow is jumping
            if (shadow.isJumping) {
                myRb.AddForce(new Vector2(shadow.currentVelocity.x, shadowJumpForce));
            }
            myRb.AddForce(shadow.currentVelocity);
        }

        if (transform.position.y < -25f) {
            GameManager.KillPlayer(this, shadow);
        }
//        print("update");
    }

    void FixedUpdate () {

        // every time shadow blinks, reset blinkCount
        if (frameCount > shadowDelay) {
            blinkCount = 0;
            frameCount = 0;
        }
        frameCount++;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("isGrounded", grounded);
        myAnim.SetFloat("verticalSpeed", myRb.velocity.y);

        float move = Input.GetAxis("Horizontal");

        myRb.velocity = new Vector2(move * speed, myRb.velocity.y);
        //print(myRb.velocity.x);
        myAnim.SetFloat("speed", Mathf.Abs(move));

        if (move > 0 && !facingRight) {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        //print("fixedupdate");
    }

    void Flip()  {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter(Collider collider) {
        print("playertrigger");
    }
}
