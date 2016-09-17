using UnityEngine;

public class TriloController : MonoBehaviour {

    public enum states { IDLE, WALK, DIG_DOWN, CLIMB_UP, FALL, BASH, DEATH, SURVIVE};

    public float moveFactor, maxVel, bashRate, digRate;

    private states currentState;

    private bool isClimber;
    private bool readyToBash;
    private bool isBashing;

    //Thresholds
    private float flipThreshold; //Threshold to cause a flip

    private int direction;

    private float nextBash, nextDig;

    private Transform tf;
    private Rigidbody2D rb;

    //Used to inform GameManager that trilo died/destroyed/survived
    public delegate void DestroyCallback(TriloController trilo, states state);
    public DestroyCallback destroyCallback;

    // Use this for initialization
    void Start ()
    {
        currentState = states.WALK;
        isClimber = false;
        readyToBash = false;
        isBashing = false;

        flipThreshold = 0.2f;

        direction = 1;

        nextBash = Time.time;
        nextDig = Time.time;

        tf = transform;
        rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update ()
    {
        if (currentState == states.BASH && Time.time > nextBash)
        {
            Bash();
            nextBash = Time.time + bashRate;
        }

        if (currentState == states.DIG_DOWN && Time.time > nextDig)
        {
            Dig();
            nextDig = Time.time + digRate;
        }



	}

    // For movement
    void FixedUpdate()
    {
        if (currentState == states.WALK)
            Walk();
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

        if(Mathf.Abs(rb.velocity.x) < flipThreshold)
        {
            FlipDirection();
            Debug.Log("Git");
        }

    }

    // detects collisionss
    void OnTriggerEnter2D(Collider2D coll)
    {
        //Hits the end
        if (coll.gameObject.tag == "End")
        {
            Survive();
        }
    }

    //IDLE, WALK, DIG_DOWN, CLIMB_UP, FALL, BASH
    public void PerformAbility(states newState)
    {
        switch(newState)
        {
            case states.IDLE: //idle
                return;
            case states.WALK: //walk
                currentState = newState;
                break;
            case states.DIG_DOWN:
                currentState = newState;
                break;
            case states.CLIMB_UP:

                break;
            case states.FALL:

                break;
            case states.BASH:
                ReadyBash();
                break;
        }
    }

    // moving left or right
    public void Walk()

    {
        //Debug.Log("vel is " + rb.velocity);
        if (direction > 0) direction = 1;
        else if (direction < 0) direction = -1;
        if (Mathf.Abs(rb.velocity.x) < maxVel)
            rb.AddForce(new Vector2(moveFactor * direction * Time.deltaTime, 0f));

        //Clamp the rotation so the trilo doesn't flip
        if(currentState != states.CLIMB_UP)
            rb.rotation = Mathf.Clamp(rb.rotation, -30.0f,30.0f); //Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * 1.0f);
    }

    // digging down
    public void Dig()
    {
        //victor's digging code
    }

    // climbing up "steps"
    public void Climb()
    {
        //climbing things
    }

    // falling; only moving vertically
    public void Fall()
    {

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
        tf.localScale = new Vector3(-tf.localScale.x, 1f, 1f);
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
