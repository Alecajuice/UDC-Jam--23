using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private static GameObject music;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (music == null) {
            music = gameObject;
        } else {
            Object.Destroy(gameObject);
        }
    }
}
