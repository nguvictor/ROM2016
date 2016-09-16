using UnityEngine;
using System.Collections;

public class GenericTrelloByteController : MonoBehaviour {

    enum trelloState {walking, falling, digging, climber, bashing };
    bool isClimber;

    trelloState currentState = trelloState.walking;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
