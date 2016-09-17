using UnityEngine;

public class TriloController : MonoBehaviour {

    public enum states { IDLE, WALK, DIG_DOWN, DIG_SIDEWAYS, CLIMB_UP, FALL, BASH};

    public float moveFactor;
    public float maxVel;

    private states currentState;

    private bool isClimber;
    private bool isBashing;

    private int direction;

    private Transform tf;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start ()
    {
        currentState = states.WALK;
        isClimber = false;
        isBashing = false;

        direction = 1;

        tf = transform;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    // For movement
    void FixedUpdate()
    {
        if (currentState == states.WALK)
            Walk();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("colldided");
        direction = -direction;
        Walk();
    }

    // moving left or right
    public void Walk()
    {
        Debug.Log("vel is " + rb.velocity);
        if (direction > 0) direction = 1;
        else if (direction < 0) direction = -1;
        if (Mathf.Abs(rb.velocity.x) < maxVel)
            rb.AddForce(new Vector2(moveFactor * direction * Time.deltaTime, 0f));
    }

    // climbing up "steps"
    public void Climb()
    {

    }

    // digging down
    public void Dig()
    {

    }

    // bashing walls
    public void Bash()
    {

    }

    // kills trilo
    protected void Die()
    {
        Destroy(this);
    }

}
