using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CameraController mainCamera;

    private bool spawning = false;
    private PlayerDeath player;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
        PlayerDeath deathHandler = player.GetComponent<PlayerDeath>();
        deathHandler.spawner = this;
        mainCamera.player = player.transform;
    }

    public void SpawnPlayer(PlayerDeath player) {
        // mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
        spawning = true;
        this.player = player;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (spawning) {
            Camera camera = mainCamera.GetComponent<Camera>();
            float height = camera.orthographicSize * 2;
            float width = height * camera.aspect;
            float x = camera.transform.position.x;
            float y = camera.transform.position.y;
            Rect cameraBounds = new Rect(x - width / 2, y - height / 2, width, height);
            if (cameraBounds.Contains(player.transform.position)) {
                player.GetComponent<PlayerController>().Activate();
                player.GetComponent<PlayerSaveLoadController>().EnableSaveLoad();
                // player.GetComponent<PlayerController>().enabled = true;
                spawning = false;
            }
        }
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        SpriteRenderer sprite = playerPrefab.GetComponentInChildren<SpriteRenderer>();
        Rect screenRect = new Rect(transform.position.x, transform.position.y, sprite.bounds.size.x, -sprite.bounds.size.y);
        Gizmos.DrawGUITexture(screenRect, sprite.sprite.texture);
    }
}
