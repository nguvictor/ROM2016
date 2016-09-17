using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour {
    public GameObject buttonGO;
    public Canvas canvas; 
    Button button;
    bool assetSelected;
    int x = 10; //number of possible 
    TriloController.states buttonSelected = TriloController.states.WALK;// for buttons
    delegate void Back();
    Back back;
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

            string x = (System.Enum.GetValues((typeof(TriloController.states))).GetValue(i)).ToString();
            GameObject newButton = Instantiate(buttonGO) as GameObject;
            button = newButton.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = x;
            newButton.transform.SetParent(canvas.transform, false);
            button.onClick.AddListener(() => { print(x); });
        }

    }





}
