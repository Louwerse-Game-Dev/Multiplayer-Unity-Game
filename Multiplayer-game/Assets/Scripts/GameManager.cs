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
        if (playerInputManager == null) return;

        playerInputManager.splitScreen = playerInputManager.splitScreen == false;
    }

    void OnEnable()
    {
        if (playerInputManager != null && playerInputManager.enabled == false) {
            togglePlayerInputManager();
        }
    }

    void OnDisable()
    {
        if (playerInputManager != null && playerInputManager.enabled == true) {
            togglePlayerInputManager();
        }
    }

    void OnPlayerJoined(PlayerInput playerObject)
    {
        Transform playerCameraTransform = playerObject.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerJoined player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        Camera playerCamera = playerCameraObject.GetComponent<Camera>();

        if (CamWithAudioListener(playerCameraObject)) {
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    void OnPlayerLeft(PlayerInput playerObject)
    {
        Transform playerCameraTransform = playerObject.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerLeft player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        Camera playerCamera = playerCameraObject.GetComponent<Camera>();

        if (playerCamera.GetComponent<AudioListener>().enabled) {
            playerCamera.GetComponent<AudioListener>().enabled = false;

            EnableCamWithAudioListener();
        }
    }

    /*
    * Looks for all GameObjects with the tag PlayerCamera except the currentPlayerCamera
    * and returns true if any of them have an audioListener enabled
    */
    private bool CamWithAudioListener(GameObject currentPlayerCamera)
    {
        GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("PlayerCamera");

        if (playerCameras == null || playerCameras.Length == 0) {
            return false;
        }

        foreach (GameObject playerCamera in playerCameras) {
            if (playerCamera == currentPlayerCamera) continue;

            if (playerCamera.GetComponent<AudioListener>().enabled == true) {
                return true;
            }
        }

        return false;
    }

    /*
    * Looks for all GameObjects with the tag PlayerCamera 
    * and enables the audioListener of the first camera found with audioListener disabled.
    */
    private void EnableCamWithAudioListener()
    {
        GameObject[] playerCameras = GameObject.FindGameObjectsWithTag("PlayerCamera");

        if (playerCameras == null || playerCameras.Length == 0) {
            return;
        }

        foreach (GameObject playerCamera in playerCameras) {
            if (playerCamera.GetComponent<AudioListener>().enabled == false) {
                playerCamera.GetComponent<AudioListener>().enabled = true;
            }
        }
    }
}
