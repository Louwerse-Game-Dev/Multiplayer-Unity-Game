using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Todo: make these configurable in the Settings Menu later?
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
    }

    [ContextMenu("Start Game")]
    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
