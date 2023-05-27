using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

// Todo: this should probably be a Singleton see: https://gist.github.com/rickyah/271e3aa31ff8079365bc ?
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField]
    private List<GameObject> players;
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
        GameObject mainCamera = FindInActiveObjectByTag("MainCamera");
        GameObject[] playerCameras = FindInActiveObjectsByTag("PlayerCamera");

        if (mainCamera == null) return;

        playerInputManager.splitScreen = playerInputManager.splitScreen == false;

        // Toggle which cameras are enabled.
        mainCamera.SetActive(mainCamera.activeSelf == false);
        foreach (GameObject playerCamera in playerCameras) {
            playerCamera.SetActive(playerCamera.activeSelf == false);
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

    private void updateMainCameraTargets()
    {
        GameObject mainCamera = FindInActiveObjectByTag("MainCamera");

        if (mainCamera == null) return;

        mainCamera.GetComponent<MultipleTargetCamera>().targets = players;
    }

    /*
    * The OnPlayerJoined event triggered by the playerInputManager.
    */
    void OnPlayerJoined(PlayerInput playerInput)
    {
        if (players.Contains(playerInput.gameObject) == false) {
            players.Add(playerInput.gameObject);
        }
        updateMainCameraTargets();

        Transform playerCameraTransform = playerInput.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerJoined player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        Camera playerCamera = playerCameraObject.GetComponent<Camera>();

        // Check if we have a playerCamera with an AudioListener enabled, if so disable the AudioListener of this new playerCamera.
        if (CamWithAudioListener(playerCameraObject)) {
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }

        // If splitScreen is disabled we should disable the playerCamera.
        if (playerInputManager.splitScreen == false && playerCameraObject.activeSelf == true) {
            playerCameraObject.SetActive(playerCameraObject.activeSelf == false);
        }
    }

    /*
    * The onPlayerLeft event triggered by the playerInputManager.
    */
    void OnPlayerLeft(PlayerInput playerInput)
    {
        if (players.Contains(playerInput.gameObject) == true) {
            players.Remove(playerInput.gameObject);
        }
        updateMainCameraTargets();

        Transform playerCameraTransform = playerInput.transform.Find("Player Camera");
        if (playerCameraTransform == null) {
            Debug.LogError("OnPlayerLeft player has no Player Camera transform");
            return;
        }

        GameObject playerCameraObject = playerCameraTransform.gameObject;
        Camera playerCamera = playerCameraObject.GetComponent<Camera>();

        // Check if we still have a playerCamera with an AudioListener enabled, if not enable the AudioListener of an other playerCamera.
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
            if (players.Contains(playerCamera.transform.parent.gameObject) == false 
                || playerCamera == currentPlayerCamera) 
            {
                continue;
            }

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
            if (players.Contains(playerCamera.transform.parent.gameObject) == true 
                && playerCamera.GetComponent<AudioListener>().enabled == false) 
            {
                playerCamera.GetComponent<AudioListener>().enabled = true;

                return;
            }
        }
    }

    /*
    * Source: https://stackoverflow.com/questions/44456133/find-inactive-gameobject-by-name-tag-or-layer
    */
    GameObject FindInActiveObjectByTag(string tag)
    {
        List<GameObject> validObjectsInScene = new List<GameObject>();

        // WARNING: Read this -> https://docs.unity3d.com/ScriptReference/Resources.FindObjectsOfTypeAll.html
        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (EditorUtility.IsPersistent(gameObject.transform.root.gameObject) 
                || gameObject.hideFlags == HideFlags.NotEditable 
                || gameObject.hideFlags == HideFlags.HideAndDontSave)
            {
                continue;
            }

            if (gameObject.transform.CompareTag(tag) == false) continue;

            validObjectsInScene.Add(gameObject);
        }

        if (validObjectsInScene.Count == 1) {
            return validObjectsInScene[0];
        }

        if (validObjectsInScene.Count > 1) {
            Debug.LogError("FindInActiveObjectByTag() => Found more than 1 object with tag " + tag);
        }

        return null;
    }

    /*
    * Source: https://stackoverflow.com/questions/44456133/find-inactive-gameobject-by-name-tag-or-layer
    */
    GameObject[] FindInActiveObjectsByTag(string tag)
    {
        List<GameObject> validObjectsInScene = new List<GameObject>();

        // WARNING: Read this -> https://docs.unity3d.com/ScriptReference/Resources.FindObjectsOfTypeAll.html
        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (EditorUtility.IsPersistent(gameObject.transform.root.gameObject) 
                || gameObject.hideFlags == HideFlags.NotEditable 
                || gameObject.hideFlags == HideFlags.HideAndDontSave)
            {
                continue;
            }

            if (gameObject.transform.CompareTag(tag) == false) continue;

            validObjectsInScene.Add(gameObject);
        }

        return validObjectsInScene.ToArray();
    }
}
