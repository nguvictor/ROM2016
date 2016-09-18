using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoadManager : MonoBehaviour {

    public void LoadGameScene()
    {
        SceneManager.LoadScene("tutorial_Scence00");
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
