using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    [ContextMenu("Start Multiplayer")]
    public void startMultiplayer()
    {
        // Todo: Load a specific scene for local coop instead, where players can join and choose a character?
        // Todo: ^ if we do this we longer need do run 'OnLocalCoopLoaded' code after sceneLoaded.
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnLocalCoopLoaded;
    }

    // Source: https://forum.unity.com/threads/call-a-function-after-the-scene-is-loaded.1207291/
    void OnLocalCoopLoaded(Scene scene, LoadSceneMode mode)
    {
        gameManager.togglePlayerInputManager();

        // Remove the default (singlePlayer) player object.
        // Todo: just remove the player object from the game scene and initialize one on startSinglePlayer()?
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && players.Length == 1) {
            GameObject.Destroy(players[0]);
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
