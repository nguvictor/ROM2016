using UnityEngine;

public class TriloController : MonoBehaviour {

    

    public enum states { WALK, DIG_DOWN, DIG_SIDEWAYS, CLIMB_UP, FALL, BASH};
    states currentState;
    bool isClimber;
    bool isBashing;

    // Use this for initialization
    void Start ()
    {
        currentState = states.WALK;
        isClimber = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
        

	}

    // For movement
    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {

    }

    public void Climb()
    {

    }

    public void Dig()
    {

    }

    public void Bash()
    {

    }

    protected void Die()
    {
        Destroy(this);
    }

}
