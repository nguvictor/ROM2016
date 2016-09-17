using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {
    public GameObject buttonGO;
    public Canvas canvas;
    public GameObject buttonPanel;
    GameObject HighlitedButton;
    Button button;
    bool assetSelected;
    int x = 10; //number of possible 
    TriloController.states buttonSelected = TriloController.states.IDLE;// for buttons
    //public enum TrelloStatus;

    void Start() {
        //create all the buttons
        // buttonGO.GetComponent<Button>();
        createMenu();
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void createMenu() {
        for (int i = 0; i < System.Enum.GetValues((typeof(TriloController.states))).Length; i++) {
            TriloController.states state = (TriloController.states)(System.Enum.GetValues((typeof(TriloController.states))).GetValue(i));
            string x = (System.Enum.GetValues((typeof(TriloController.states))).GetValue(i)).ToString();
            GameObject newButton = Instantiate(buttonGO) as GameObject;
            button = newButton.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = x;
            button.image.color = new Color(1, 1, 1, 0.5F);
            button.GetComponentInChildren<Text>().color = new Color(0, 0, 0);
            newButton.transform.SetParent(buttonPanel.transform, false);
            //call change enum function;
            button.onClick.AddListener(() => { deselect(); print(x); assetSelected = true; buttonSelected = state; SetHighlightButton(newButton); });
        }



    }


    public void deselect() {
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

    }

    public void SetHighlightButton(GameObject button)
    {

        HighlitedButton = button;
        Button b = button.GetComponent<Button>();
        b.image.color = new Color(0.1F, 0, 1, 0.5F);
        b.GetComponentInChildren<Text>().color = new Color(1, 1, 1);
        //HighlitedButton.assetSelected = true;

    }
}
