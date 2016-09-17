using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoadManager : MonoBehaviour {

    public void LoadGameScene()
    {
        //load whichever scene is used for the game
        //SceneManager.LoadScene("");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("tyler-credits");
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("tyler-title");
    }

}
