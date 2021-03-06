﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour {
    public GameObject buttonGO;
    public Canvas canvas;
    public GameObject buttonPanel;
    GameObject HighlitedButton;
    Button button;
    bool assetSelected;
    public Texture2D texture;
    public CursorMode cursorMode = CursorMode.Auto;
    int x = 10; //number of possible 

    public int numOfBashers;
    public int numOfClimbers;
    public int numOfBlockers;
    public int numOfIdiots;

    public Sprite Sprite1;
    public Sprite Sprite2;
    public Sprite Sprite3;
    public Sprite Sprite4;
    public Sprite[] sprite;
    TriloController.states buttonSelected = TriloController.states.IDLE;// for buttons
    //public enum TrelloStatus;

    void Start() {
        
 sprite = new Sprite[] { Sprite1, Sprite2, Sprite3, Sprite4 };
        //create all the buttons
        // buttonGO.GetComponent<Button>();
        createMenu();
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (Input.GetMouseButtonDown(0))
        {

            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {

                if ((hit.collider != null))
                {
                   // print(hit.collider.gameObject.tag);
                    if (hit.collider.gameObject.tag.Equals("TRELLO"))
                    {
                        hit.collider.gameObject.GetComponent<TriloController>().PerformAbility(buttonSelected);
                    }

                }
                else
                {
                    // deselect();
                }
            }
            else {
                deselect();
            }


        }
    }

    public void createMenu() {
        GameObject newButton;
        for (int i = 0; i < System.Enum.GetValues((typeof(TriloController.states))).Length-5; i++) {
            TriloController.states state = (TriloController.states)(System.Enum.GetValues((typeof(TriloController.states))).GetValue(i));
            //string x = (System.Enum.GetValues((typeof(TriloController.states))).GetValue(i)).ToString();
            string x = "";
            newButton = Instantiate(buttonGO) as GameObject;

            button = newButton.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = x;
            //button.image.color = new Color(1, 1, 1, 0.5F);
            button.image.sprite =  sprite[i];
            button.GetComponentInChildren<Text>().color = new Color(0, 0, 0);
            newButton.transform.SetParent(buttonPanel.transform, false);
            //call change enum function;
            button.onClick.AddListener(() => { deselect(); assetSelected = true; buttonSelected = state; SetHighlightButton(newButton); fancyCursor(); });
        }
         newButton = Instantiate(buttonGO) as GameObject;
        button = newButton.GetComponent<Button>();
        newButton.GetComponentInChildren<Text>().text = "Restart";
        button.image.color = new Color(1, 1, 1, 0.5F);
        //button.image.sprite = sprite[i];
        button.GetComponentInChildren<Text>().color = new Color(0, 0, 0);
        newButton.transform.SetParent(buttonPanel.transform, false);
        //call change enum function;
        button.onClick.AddListener(() => { deselect(); assetSelected = false; Restart(); });


    }
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);



    }

    public void deselect() {
        changeCursorToDefault();
        assetSelected = false;
        if (HighlitedButton == null)
        {

        }
        else
        {

            Button ab = HighlitedButton.GetComponent<Button>();
            //MoveHere.assetSelected = false;
            ab.image.color = new Color(1, 1, 1, 0.5F);
            ab.GetComponentInChildren<Text>().color = new Color(0, 0, 0);
            HighlitedButton = null;
        }
        buttonSelected = TriloController.states.IDLE;
    }

    public void SetHighlightButton(GameObject button)
    {

        HighlitedButton = button;
        Button b = button.GetComponent<Button>();
        //b.image.color = new Color(0.1F, 0, 1, 0.5F);
        b.GetComponentInChildren<Text>().color = new Color(1, 1, 1);
        //HighlitedButton.assetSelected = true;

    }

    public void changeCursorToDefault()
    {
       // print("Cursor Change");
    Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

public void fancyCursor()
    {
        //print("default");
    Cursor.SetCursor(texture, Vector2.zero, cursorMode);
    }
}
