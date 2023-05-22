using UnityEngine;

[CreateAssetMenu(menuName = "LouwerseGames/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private GameManager gameManager;
    public static GameManager GameManager { get { return Instance.gameManager; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstInitialize()
    {
        // TODO: somehow fix this garbage: https://youtu.be/cH-QQoNNpaI?t=222
        // Instance.gameManager = Instantiate(Instance.gameManager) as GameManager;

        Debug.Log("This message will output before Awake.");
    }
}
