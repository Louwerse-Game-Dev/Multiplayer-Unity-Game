using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Todo: this should probably be a Singleton
public class TitleScreenManager : MonoBehaviourSingleton<TitleScreenManager>
{
    [SerializeField]
    private GameManager gameManager;

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
        SceneManager.sceneLoaded += OnSinglePlayerLoaded;
    }

    [ContextMenu("Start Multiplayer")]
    public void startMultiplayer()
    {
        // Todo: Load a specific scene for local coop instead, where players can join and choose a character?
        // Todo: ^ if we do this we can run 'OnLocalCoopLoaded' code here instead of after sceneLoaded.
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnLocalCoopLoaded;
    }

    // Source: https://forum.unity.com/threads/call-a-function-after-the-scene-is-loaded.1207291/
    void OnSinglePlayerLoaded(Scene scene, LoadSceneMode mode)
    {
        Instantiate(gameManager.GetComponent<PlayerInputManager>().playerPrefab);
    }

    // Source: https://forum.unity.com/threads/call-a-function-after-the-scene-is-loaded.1207291/
    void OnLocalCoopLoaded(Scene scene, LoadSceneMode mode)
    {
        gameManager.togglePlayerInputManager();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
