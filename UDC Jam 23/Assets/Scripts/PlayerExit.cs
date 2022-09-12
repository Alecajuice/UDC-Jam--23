using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using UnityEngine.SceneManagement;

public class PlayerExit : MonoBehaviour
{
    public float exitSpeed;
    public float rotateSpeed;
    public AudioClip exitClip;

    private bool exiting = false;
    private Transform exit;
    private ExitController exitController;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit") && !exiting) {
            PlayerController playerController = GetComponent<PlayerController>();
            playerController.Deactivate();
            GetComponent<PlayerSaveLoadController>().DisableSaveLoad();
            exiting = true;
            exit = other.transform;
            exitController = other.GetComponent<ExitController>();
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = exitClip;
            audioSource.Play();
        }
    }

    void Update() {
        if (exiting) {
            if (Vector3.Distance(transform.position, exit.position) < 0.1) {
                AudioSource source = GetComponentInChildren<AudioSource>();
                source.Stop();
                exitController.Exit();
                exiting = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, exit.position, exitSpeed * Time.deltaTime);
            transform.rotation *= Quaternion.Euler(0, 0, rotateSpeed * Time.deltaTime);
            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 0, 1), exitSpeed * Time.deltaTime);
        }
    }
}
