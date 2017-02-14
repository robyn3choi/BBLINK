using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRecording : MonoBehaviour
{

    public PlayerController player;
    public Queue<float> xList = new Queue<float>();
    public Queue<float> yList = new Queue<float>();
    public Vector2 currentVelocity;
    Vector2 previousPosition;

    bool grounded;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    Animator myAnim;
    bool facingRight = true;
    public bool isJumping = false;
    int shadowDelay = 80;
    float nextTimeToSearch = 0;

    //public static ShadowRecording instance = null;

//    void Awake() {
//        if (instance == null) {
//            instance = this;
//        }    
//        else if (instance != this) {
//            Destroy(gameObject);
//        }
//    }

    void Start()
    {
        previousPosition = transform.position;
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {

    }


    void FixedUpdate()
    {
        if (player == null) {
            FindPlayer();
            return;
        }
        if (player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            myAnim.speed = 1;
            xList.Enqueue(player.transform.position.x);
            yList.Enqueue(player.transform.position.y);

            // shadow is 120 frames behind player
            if (xList.Count > shadowDelay)
            {
                transform.position = new Vector2(xList.Dequeue(), yList.Dequeue());
            }

            // calculate velocity because there's no rigidbody on the shadow
            currentVelocity = ((Vector2) transform.position - previousPosition) / Time.deltaTime;
            previousPosition = transform.position;

            if (currentVelocity.y > 0) {
                isJumping = true;
            }
            else {
                isJumping = false;
            }
        }
        else {
            myAnim.speed = 0;
        }

        // same animation logic as player - refactor later?
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        myAnim.SetBool("isGrounded", grounded);
        myAnim.SetFloat("verticalSpeed", currentVelocity.y);
        myAnim.SetFloat("speed", Mathf.Abs(currentVelocity.x));

        if (currentVelocity.x > 0 && !facingRight) {
            Flip();
        }
        else if (currentVelocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()  {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
  
    void FindPlayer() {
        if (nextTimeToSearch <= Time.time) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null) {
                player = searchResult.GetComponent<PlayerController>();
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }

    public void RestartQueues() {
        xList = new Queue<float>();
        yList = new Queue<float>();
    }
}
