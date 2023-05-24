using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Todo: this should probably be a Singleton see: https://gist.github.com/rickyah/271e3aa31ff8079365bc ?
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    [ContextMenu("Toggle PlayerInputManager (Local Co-Op)")]
    public void togglePlayerInputManager()
    {
        if (playerInputManager == null) {
            return;
        }
        
        playerInputManager.enabled = playerInputManager.enabled == false;
    }

    [ContextMenu("Toggle Split Screen")]
    public void toggleSplitScreen()
    {
        if (playerInputManager == null) {
            return;
        }
        playerInputManager.splitScreen = playerInputManager.splitScreen == false;
    }

    void OnEnable()
    {
        togglePlayerInputManager();
    }

    void OnDisable()
    {
        togglePlayerInputManager();
    }
}
