using UnityEngine;
using Destructible2D;

public class TriloController : MonoBehaviour {

    //Idle and following are "unclickable"
    public enum states {  DIG, CLIMB, BASH, BLOCK, IDLE, WALK, FALL,  DEATH, SURVIVE};

    public float moveFactor, climbFactor, maxVel, bashRate, digRate;

    public states currentState;

    private bool isClimber;
    private bool isClimbing;
    private bool readyToBash;
    private bool isBashing;

    //Thresholds
    private float flipThreshold; //Threshold to cause a flip

    //Digging
    public Texture2D digTexture; 

    public int direction = 1;

    private float nextBash, nextDig;

    private Transform tf;
    private Rigidbody2D rb;
    private Animator animator;

    //Used to inform GameManager that trilo died/destroyed/survived
    public delegate void DestroyCallback(TriloController trilo, states state);
    public DestroyCallback destroyCallback;

    // Use this for initialization
    void Start ()
    {
        currentState = states.WALK;
        isClimber = false;
        isClimbing = false;
        readyToBash = false;
        isBashing = false;

        digRate = 1.0f;

        flipThreshold = 0.8f;

        climbFactor = 15.0f;

        //direction = 1;

        nextBash = Time.time;
        nextDig = Time.time;

        tf = transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

	// Update is called once per frame
	void Update ()
    {

        if (currentState == states.BASH && Time.time > nextBash)
        {
            Bash();
            nextBash = Time.time + bashRate;
        }

        if (currentState == states.DIG && Time.time > nextDig)
        {
            Dig();
            nextDig = Time.time + digRate;
        }

        if(currentState == states.CLIMB)
        {
            rb.gravityScale = 0.0f;
        }else
        {
            rb.gravityScale = 1.0f;
        }

	}

    // For movement
    void FixedUpdate()
    {
        if (currentState == states.WALK)
            Walk();
        if (currentState == states.CLIMB)
            Climb();
        if (currentState == states.FALL)
            Fall();
    }


    // detects collisionss
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Wall")
        {
            if (!readyToBash)
                FlipDirection();
            else
                currentState = states.BASH;
        }

        Vector2 contactNormal = coll.contacts[0].normal;

        //Debug.Log(Vector2.Angle(transform.right, contactNormal));
        Debug.DrawLine(transform.right, transform.right * 3.0f,Color.red);
        //Debug.DrawLine(contactNormal, contactNormal*3.0f, Color.blue);
        
        if (currentState == states.WALK && Mathf.Abs(rb.velocity.x) < flipThreshold) //&& CheckOnWall())
        {
            FlipDirection();
        }/*
        else if (currentState == states.WALK && Mathf.Abs(rb.velocity.x) < flipThreshold)//They might be stuck help them out
        {
            Debug.Log("Assistance" + Mathf.Abs(rb.velocity.x));
            rb.AddForce(transform.right * direction * Time.deltaTime * moveFactor*1.5f);
        }*/
        /*
        if(isClimber && Mathf.Abs(rb.velocity.x) < flipThreshold)
        {
            //Calculate Potential Climb

            //else
            FlipDirection();
        }
        */

    }

    // detects collisionss
    void OnTriggerEnter2D(Collider2D coll)
    {
        //Hits the end
        if (coll.gameObject.tag == "End")
        {
            Survive();
        }

        if (isClimber && currentState== states.WALK || currentState == states.CLIMB)
        {

            if (CheckOnSurface())
            {
                isClimbing = true;
                currentState = states.CLIMB;
            }

            
        }
    }

    //DIG, CLIMB, BASH, BLOCK, IDLE, WALK, FALL,  DEATH, SURVIVE
    public void PerformAbility(states newState)
    {
        switch(newState)
        {
            case states.IDLE: //idle
                return;
            case states.WALK: //walk
                currentState = newState;
                break;
            case states.DIG:
                currentState = newState;
                break;
            case states.CLIMB:

                break;
            case states.FALL:

                break;
            case states.BASH:
                ReadyBash();
                break;
            case states.BLOCK:
                currentState = newState;
                gameObject.layer = 9;
                this.rb.constraints = RigidbodyConstraints2D.FreezeAll;
                break;
        }
    }

    // moving left or right
    public void Walk()

    {
        /*
        if (currentState == states.WALK && Mathf.Abs(rb.velocity.x) < flipThreshold && CheckOnWall())
        {
            FlipDirection();
        }/*
        else if (currentState == states.WALK && Mathf.Abs(rb.velocity.x) < flipThreshold)//They might be stuck help them out
        {
            Debug.Log("Assistance" + Mathf.Abs(rb.velocity.x));
            rb.AddForce(transform.right * direction * Time.deltaTime * moveFactor * 2.5f);
        }*/
        //Debug.Log("vel is " + rb.velocity);
        if (direction > 0) direction = 1;
        else if (direction < 0) direction = -1;
        if (Mathf.Abs(rb.velocity.x) < maxVel)
            rb.AddForce(transform.right * direction * Time.deltaTime * moveFactor);
        Debug.DrawLine(transform.position, transform.right * direction * moveFactor * 2.0f);
        //rb.AddForce(new Vector2(moveFactor * direction * Time.deltaTime, 0f));

        //Clamp the rotation so the trilo doesn't flip
        if (currentState != states.CLIMB)
        {
            if(direction > 0)
                rb.rotation = Mathf.Clamp(rb.rotation, -20.0f, 30.0f); //Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * 1.0f);
            else
                rb.rotation = Mathf.Clamp(rb.rotation, -25.0f, 20.0f); //Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * 1.0f);
        }
            
    }

    // digging down
    public void Dig()
    {
        //victor's digging code
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1f);
        Debug.DrawLine(transform.position - transform.up * 0.2f, transform.position - transform.up * 10f);
        
        if (hit.collider != null && hit.collider.gameObject.tag == "Ground" )
        {
            animator.SetBool("digging", true);
            D2dDestructible.StampAll(transform.position, Vector2.one * 1.2f, 0.0f, digTexture, 1, -1);
        }
        else {
            this.PerformAbility(states.WALK);
            animator.SetBool("digging", false);
        }
        }

    // climbing up "steps"
    public void Climb()
    {
        if (CheckOnSurface())//if the trello is in the sky
        {
            //Stick on wall
            if (Mathf.Abs(rb.velocity.x) < maxVel)
                rb.AddForce(new Vector2(moveFactor * direction * Time.deltaTime * 2.0f, climbFactor));
        }
        else
        {
            //Force to ground
            currentState = states.FALL;
            isClimbing = false;

        }
        
        if(direction == 1)
            rb.rotation = Mathf.Clamp(rb.rotation, 30.0f, 180.0f); //Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * 1.0f);
        else
            rb.rotation = Mathf.Clamp(rb.rotation, -30.0f, -180.0f); //Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * 1.0f);
    }

    public bool CheckOnSurface()
    {
        
        bool result = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position - transform.up * 0.1f , transform.position - transform.up*0.8f);
        Debug.DrawLine(transform.position - transform.up * 0.1f, transform.position - transform.up*0.8f );
        //Debug.Log(hit.collider.tag);
        if (hit.collider != null && hit.collider.tag != "TRELLO")
        {
            Debug.Log(hit.collider.tag);
            result = true;
        }
        return result;
    }

    public bool CheckOnWall()
    {

        bool result = false;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + transform.right * 0.8f);
        Debug.DrawLine(transform.position, transform.position + transform.right * 0.8f);
        //Debug.Log(hit.collider.tag);
        if (hit.collider != null && hit.collider.tag != "TRELLO")
        {
            Debug.Log(hit.collider.tag);
            result = true;
        }
        Debug.Log(result);
        return result;
    }

    // falling; only moving vertically
    public void Fall()
    {
        rb.angularVelocity = 0.0f;
        rb.velocity = Vector2.zero;
        currentState = states.WALK;
    }

    // bashing walls
    public void Bash()
    {
        //victor's sideways bashing code
        
    }

    // To be hooked up to the "bash" button
    public void ReadyBash()
    {
        readyToBash = true;
    }

    // flips direction value and sprite
    protected void FlipDirection()
    {
        direction = -direction;
        tf.localScale = new Vector3(-tf.localScale.x, tf.localScale.y, 1f);
    }

    // kills trilo
    protected void Die()
    {
        destroyCallback(this,states.DEATH);
        Destroy(this.gameObject);
    }

    // If the trilo makes it to the end
    protected void Survive()
    {
        destroyCallback(this, states.SURVIVE);
        Destroy(this.gameObject);
    }

}
