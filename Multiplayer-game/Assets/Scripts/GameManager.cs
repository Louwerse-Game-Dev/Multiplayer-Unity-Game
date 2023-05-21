using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Todo: this should probably be a Singleton
public class GameManager : MonoBehaviour
{
    PlayerInputManager playerInputManager;

    // Start is called before the first frame update
    void Start()
    {
        // Todo: make these configurable in the Settings Menu later?
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);

        playerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Todo: We should make this a Singleton so we can call this function from the Main Menu.
    [ContextMenu("Toggle Local Co-Op")]
    public void toggleLocalCoop()
    {
        playerInputManager.enabled = playerInputManager.enabled == false;
    }

    // Todo: We should make this a Singleton so we can call this function from Menu scenes.
    [ContextMenu("Toggle Split Screen")]
    public void toggleSplitScreen()
    {
        playerInputManager.splitScreen = playerInputManager.splitScreen == false;
    }
}
