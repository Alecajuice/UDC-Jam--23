using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TarodevController;

public class PlayerSaveLoadController : MonoBehaviour
{
    [SerializeField] private GameObject savePointPrefab;
    [SerializeField] private Animator loadMask;
    [SerializeField] private ParticleSystem loadParticles;
    [SerializeField] private float loadTime;
    [SerializeField] private AudioClip saveClip;
    [SerializeField] private AudioClip loadClip;
    [SerializeField] private AudioClip errorClip;

    GameObject savePoint = null;
    bool hasSave = false;

    bool canSave = true;
    bool canLoad = true;

    PlayerController playerController;
    CameraController cameraController;
    AudioSource audioSource;
    bool isLoading = false;
    float timer = 0f;
    Vector2 loadVelocity;

    // Save point indicator UI
    InGameUIController ui;

    void Awake() {
        playerController = GetComponent<PlayerController>();
        cameraController = FindObjectOfType<CameraController>();
        audioSource = GetComponent<AudioSource>();
        ui = FindObjectOfType<InGameUIController>();
    }

    void Update() {
        if (isLoading) {
            timer += Time.deltaTime;
            if (timer >= loadTime) {
                playerController.ReturnControl();
                transform.position = savePoint.transform.position;
                isLoading = false;
                playerController.ApplyVelocity(loadVelocity, PlayerForce.Set);
                canLoad = true;
                canSave = true;
                loadMask.SetBool("Loading", false);
                playerController.Gravity = true; // turn on player gravity
                CapsuleCollider2D[] colliders = playerController.GetComponents<CapsuleCollider2D>();
                colliders[0].enabled = true; // enable standing collider
                cameraController.RemoveTarget();
                loadParticles.Stop();
            }
        }
    }

    public void Save() {
        if (canSave) {
            if (savePoint != null) Destroy(savePoint);
            Vector3 position = transform.position;
            savePoint = Instantiate(savePointPrefab, position, Quaternion.identity);
            audioSource.clip = saveClip;
            audioSource.Play();
            ui.savePoint = savePoint.transform.position;
            ui.savePoint.y += 0.75f;
            ui.hasSave = true;
            hasSave = true;
        } else {
            if (!audioSource.isPlaying) {
                audioSource.clip = errorClip;
                audioSource.Play();
            }
        }
    }

    public void Load() {
        if (canLoad && hasSave) {
            isLoading = true;
            timer = 0f;
            loadVelocity = playerController.Speed;
            canLoad = false;
            canSave = false;
            loadMask.SetBool("Loading", true);
            playerController.Gravity = false; // turn off player gravity
            CapsuleCollider2D[] colliders = playerController.GetComponents<CapsuleCollider2D>();
            colliders[0].enabled = false; // disable standing collider
            playerController.TakeAwayControl();
            cameraController.SetTargetWithLookahead(savePoint.transform.position);
            loadParticles.Play();
            audioSource.clip = loadClip;
            audioSource.Play();
        } else {
            if (!audioSource.isPlaying) {
                audioSource.clip = errorClip;
                audioSource.Play();
            }
        }
    }

    public void DisableSave() {
        canSave = false;
    }

    public void EnableSave() {
        canSave = true;
    }

    public void DisableLoad() {
        canLoad = false;
    }

    public void EnableLoad() {
        canLoad = true;
    }

    public void DisableSaveLoad() {
        canSave = false;
        canLoad = false;
    }

    public void EnableSaveLoad() {
        canSave = true;
        canLoad = true;
    }
}
