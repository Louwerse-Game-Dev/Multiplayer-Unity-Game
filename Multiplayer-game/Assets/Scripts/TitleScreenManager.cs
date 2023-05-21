using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Todo: this should probably be a Singleton
public class TitleScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Todo: make these configurable in the Settings Menu later?
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
    }

    [ContextMenu("Start Single Player")]
    public void startSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    [ContextMenu("Start Multiplayer")]
    public void startMultiplayer()
    {
        SceneManager.LoadScene(1);
        // Todo: call GameManager -> toggleLocalCoop()
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
