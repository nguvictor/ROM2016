using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public TriloController trelloObject;
    public Transform startPosition;
    public Transform endPosition;
    public Text levelTimerText, endText;

    public int startingSpawnCount = 10; //How many to spawn at the start

    public float spawnRate=2.0f; //What rate to spawn in seconds

    private float timeDecrement; //for the level timer
    private int spawnedCount=0; //How many we have currently spawned

    private float spawnTimer; //current timer
    private float levelTimer; //max amount of time to beat level

    private int deathCount; //Death Count of the trellos
    private int surviveCount; //Survive count of the trellos
    private int maxDeaths; //number of deaths for game over
    private int minSurvives; //number of trilo survivals for a win

    private List<TriloController> trellos;
    

    delegate void MultiDelegate(TriloController trello);
    MultiDelegate destroyCallback;//For when the trello is destroyed

    // Use this for initialization
    void Start ()
    {
        trellos = new List<TriloController>();

        maxDeaths = 5;
        minSurvives = 5;
        levelTimer = 10;
        timeDecrement = 1f;

        UpdateTimerText();
        UpdateEndText("");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (spawnedCount < startingSpawnCount && startPosition)
        {
            if (spawnTimer <= 0.0f)
            {
                //Spawn trello
                TriloController trello = (TriloController)Instantiate(trelloObject, new Vector3(startPosition.position.x, startPosition.position.y, 0.0f), Quaternion.identity);
                trello.destroyCallback = trelloDestroyed;
                trellos.Add(trello);
                spawnedCount += 1;
                //Reset timer
                spawnTimer = spawnRate;
            }
            //Increment timer
            spawnTimer -= Time.deltaTime;
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

        if (levelTimer <= 0f)
        {
            //game over
            UpdateEndText("LOSE");
        }
        else
        {
            levelTimer -= Time.deltaTime;
            UpdateTimerText();
        }

    }

    void UpdateTimerText()
    {
        levelTimerText.text = "" + Mathf.Round(levelTimer);
    }

    void UpdateEndText(string result)
    {
        endText.text = result;
    }

    void trelloDestroyed(TriloController trilo, TriloController.states state)
    {

        //Death
        if(state == TriloController.states.DEATH)
        {
            //Add to death counter
            deathCount += 1;
            if (deathCount >= maxDeaths)
            {
                //game over
                UpdateEndText("LOSE");
            }
        }

        //Survive
        if (state == TriloController.states.SURVIVE)
        {
            //Add to survive counter
            surviveCount += 1;
            if (surviveCount >= minSurvives)
            {
                //you're winner
                UpdateEndText("WIN");
            }
        }

    }

}
