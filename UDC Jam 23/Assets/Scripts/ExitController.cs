using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    private InGameUIController ui;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        ui = FindObjectOfType<InGameUIController>();
    }

    public void Exit() {
        ui.ShowLevelComplete();
    }
}
