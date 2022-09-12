using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class PlayerDeath : MonoBehaviour
{
    public PlayerSpawner spawner;
    [SerializeField] private float deathTime;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float fallAccel;
    [SerializeField] private Vector2 initialVel;
    [SerializeField] private AudioClip deathClip;

    private PlayerController playerController;
    private bool dying = false;
    private float time = 0f;
    private Vector3 velocity;

    void Awake() => playerController = GetComponent<PlayerController>();

    public void HandleDeath() {
        if (!dying) {
            playerController.Deactivate();
            GetComponent<PlayerSaveLoadController>().DisableSaveLoad();
            CapsuleCollider2D[] colliders = playerController.GetComponents<CapsuleCollider2D>();
            colliders[0].enabled = false; // disable standing collider
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = deathClip;
            audioSource.Play();
            dying = true;
            time = 0f;
            velocity = initialVel;
        }
    }

    void Update() {
        if (dying) {
            time += Time.deltaTime;
            if (time >= deathTime) {
                transform.rotation = Quaternion.identity;
                transform.position = spawner.transform.position;
                playerController.ApplyVelocity(new Vector2(0, 0), PlayerForce.Set);
                spawner.SpawnPlayer(this);
                CapsuleCollider2D[] colliders = playerController.GetComponents<CapsuleCollider2D>();
                colliders[0].enabled = true; // enable standing collider
                // playerController.Activate();
                dying = false;
            } else {
                // transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed);
                Vector3 rotatePoint = transform.Find("Midpoint").transform.position;
                transform.RotateAround(rotatePoint, Vector3.forward, rotateSpeed * Time.deltaTime);
                // Debug.Log(rotatePoint);
                transform.position += velocity * Time.deltaTime;

                velocity.y -= fallAccel * Time.deltaTime;
            }
        }
    }
}
