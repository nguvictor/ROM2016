using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public TriloController trelloObject;
    public Transform startPosition;
    public Transform endPosition;
    public Text levelTimerText, endText, trilosRemaining, trilosLost;
    public string nextLevelName;

    public int startingSpawnCount = 10; //How many to spawn at the start
    public int maxDeaths; //number of deaths for game over
    public int minSurvives; //number of trilo survivals for a win

    public float spawnRate=2.0f; //What rate to spawn in seconds
    public float levelTimer; //max amount of time to beat level

    private int spawnedCount=0; //How many we have currently spawned
    private int numTrilos;
    private int numTrilosAlive;

    private float timeAfter; //seconds after game ends before scene switches
    private float endTime; //time that scene switches
    private float spawnTimer; //current timer
    private float timeDecrement; //for the level timer

    private bool gameOver;

    private int deathCount; //Death Count of the trellos
    private int surviveCount; //Survive count of the trellos

    private List<TriloController> trellos;
    

    delegate void MultiDelegate(TriloController trello);
    MultiDelegate destroyCallback;//For when the trello is destroyed

    // Use this for initialization
    void Start ()
    {
        trellos = new List<TriloController>();

       
        timeDecrement = 1f;
        timeAfter = 5f;

        UpdateTimerText();
        UpdateEndText("");
        UpdateTriloStatText();

        gameOver = false;
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
                numTrilos++;
                numTrilosAlive++;
                UpdateTriloStatText();
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

        if (levelTimer <= 0f && !gameOver)
        {
            //game over
            GameOver("Restart?");
        }
        else
        {
            levelTimer -= Time.deltaTime;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (levelTimer < 0) levelTimer = 0;
        else levelTimerText.text = "Time: " + Mathf.Round(levelTimer);
    }

    void UpdateEndText(string result)
    {
        endText.text = result;
    }

    void UpdateTriloStatText()
    {
        trilosRemaining.text = "Remaining: " + (startingSpawnCount - spawnedCount);
        trilosLost.text = "Lost: " + deathCount;
    }

    void GameOver(string result)
    {
        UpdateEndText(result);
        gameOver = true;
    }

    void LoadTitleScreen()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    void trelloDestroyed(TriloController trilo, TriloController.states state)
    {

        //Death
        if(state == TriloController.states.DEATH && !gameOver)
        {
            //Add to death counter
            deathCount += 1;
            numTrilosAlive--;
            UpdateTriloStatText();
            if (deathCount >= maxDeaths)
            {
                //game over
                GameOver("Restart?");
            }
        }

        //Survive
        if (state == TriloController.states.SURVIVE && !gameOver)
        {
            //Add to survive counter
            surviveCount += 1;
            numTrilosAlive--;
            UpdateTriloStatText();
            if (surviveCount >= minSurvives)
            {
                //you're winner
                GameOver("Congratulations!");
                SceneManager.LoadScene(nextLevelName);
            }
        }

    }

}
