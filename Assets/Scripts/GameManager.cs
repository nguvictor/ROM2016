using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;


    public int startingSpawnCount = 10; //How many to spawn at the start

    public float spawnRate=2.0f; //What rate to spawn in seconds

    private int spawnedCount=0; //How many we have currently spawned

    private float timer; //current timer

    private List<GameObject> trellos;
    public GameObject trelloObject;

	// Use this for initialization
	void Start () {
        trellos = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if (spawnedCount < startingSpawnCount && startPosition)
        {
            if (timer <= 0.0f) {
                //Spawn trello
                GameObject trello = (GameObject)Instantiate(trelloObject, new Vector3(startPosition.position.x, startPosition.position.y, 0.0f), Quaternion.identity);
                trellos.Add(trello);
                spawnedCount += 1;
                //Reset timer
                timer = spawnRate;
            }
            //Increment timer
            timer -= Time.deltaTime;
        }

        List<GameObject> toRemoves = new List<GameObject>();
        //Check end condition
        foreach(GameObject trello in trellos)
        {
            if(trello == null) //Probably been destroyed
            {
                toRemoves.Add(trello);
            }
        }

        foreach (GameObject toRemove in toRemoves)
        {
            trellos.Remove(toRemove);
        }


    }
}
