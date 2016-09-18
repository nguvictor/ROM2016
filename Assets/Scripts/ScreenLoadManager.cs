using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenLoadManager : MonoBehaviour {

    public void LoadGameScene()
    {
        SceneManager.LoadScene("main");
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
