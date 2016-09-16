using UnityEngine;

public class TriloController : MonoBehaviour {

    private enum states { WALK, DIG_DOWN, DIG_SIDEWAYS, CLIMB_UP};

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
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

    public void Jump()
    {

    }

    protected void Die()
    {
        Destroy(this);
    }

}
