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
        if (playerInputManager == null) return;
        
        playerInputManager.enabled = playerInputManager.enabled == false;
    }

    [ContextMenu("Toggle Split Screen")]
    // Todo: make this configurable in the Settings Menu later?
    public void toggleSplitScreen()
    {
        if (playerInputManager == null) return;

        // Find all cameras with these tags:
        string[] tags = {"MainCamera", "PlayerCamera"};
        GameObject[] cameras = FindInActiveObjectsByTags(tags);

        // Todo: find out why we find 1 PlayerCamera to many? Left over from the SinglePlayer player object?
        // Debug.Log("Found " + cameras.Length + " Cameras");
        playerInputManager.splitScreen = playerInputManager.splitScreen == false;

        // Toggle which cameras are enabled.
        foreach (GameObject camera in cameras) {
            camera.SetActive(camera.activeSelf == false);
        }
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

    /*
    * The OnPlayerJoined event triggered by the playerInputManager.
    */
    void OnPlayerJoined(PlayerInput playerObject)
    {
        Transform playerCameraTransform = playerObject.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerJoined player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        // Todo: if player camera disabled... return?
        Camera playerCamera = playerCameraObject.GetComponent<Camera>();

        if (CamWithAudioListener(playerCameraObject)) {
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    /*
    * The onPlayerLeft event triggered by the playerInputManager.
    */
    void OnPlayerLeft(PlayerInput playerObject)
    {
        Transform playerCameraTransform = playerObject.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerLeft player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        // Todo: if player camera disabled... return?
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

    /*
    * Source: https://stackoverflow.com/questions/44456133/find-inactive-gameobject-by-name-tag-or-layer
    */
    GameObject[] FindInActiveObjectsByTags(string[] tags)
    {
        List<GameObject> validTransforms = new List<GameObject>();
        Transform[] objectTransforms = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];

        foreach (Transform objectTransform in objectTransforms)
        {
            if (objectTransform.hideFlags != HideFlags.None) continue;

            foreach (string tag in tags) {
                if (objectTransform.CompareTag(tag) == false) continue;

                validTransforms.Add(objectTransform.gameObject);
            }
        }
        return validTransforms.ToArray();
    }
}
