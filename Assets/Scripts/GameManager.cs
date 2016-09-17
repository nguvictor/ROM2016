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

    private int deathCount; //Death Count of the trellos
    private int surviveCount; //Survive count of the trellos

    private List<TriloController> trellos;
    public TriloController trelloObject;

    delegate void MultiDelegate(TriloController trello);
    MultiDelegate destroyCallback;//For when the trello is destroyed

    // Use this for initialization
    void Start () {
        trellos = new List<TriloController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (spawnedCount < startingSpawnCount && startPosition)
        {
            if (timer <= 0.0f) {
                //Spawn trello
                TriloController trello = (TriloController)Instantiate(trelloObject, new Vector3(startPosition.position.x, startPosition.position.y, 0.0f), Quaternion.identity);
                trello.destroyCallback = trelloDestroyed;
                trellos.Add(trello);
                spawnedCount += 1;
                //Reset timer
                timer = spawnRate;
            }
            //Increment timer
            timer -= Time.deltaTime;
        }

        List<TriloController> toRemoves = new List<TriloController>();
        //Check end condition
        foreach(TriloController trello in trellos)
        {
            if(trello == null) //Probably been destroyed
            {
                toRemoves.Add(trello);
            }
        }

        foreach (TriloController toRemove in toRemoves)
        {
            trellos.Remove(toRemove);
        }


    }

    void trelloDestroyed(TriloController trilo, TriloController.states state){

        //Death
        if(state == TriloController.states.DEATH)
        {
            //Add to death counter
            deathCount += 1;
        }
        //Survive
        if (state == TriloController.states.SURVIVE)
        {
            //Add to survive counter
            surviveCount += 1;
        }

    }
}
