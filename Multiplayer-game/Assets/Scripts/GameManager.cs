using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Todo: make these configurable in the Settings Menu later?
        Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow, 60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
