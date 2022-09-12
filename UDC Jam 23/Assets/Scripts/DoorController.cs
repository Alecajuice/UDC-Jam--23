using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool inverted;

    private AudioSource audioSource;
    private bool open;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Deactivate();
    }

    public void Activate() {
        animator.SetBool("Open", !inverted);
        if (!open) audioSource.Play();
        open = !inverted;
    }
    
    public void Deactivate() {
        animator.SetBool("Open", inverted);
        if (open) audioSource.Play();
        open = inverted;
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            foreach (ContactPoint2D contact in other.contacts) {
                if (!open) { // Door is closing
                    if (Vector2.Angle(contact.normal, Vector2.up) < 90) { // Coliding with underside of door
                        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                        if (playerController.Grounded) { // Colliding with ground
                            PlayerDeath playerDeath = other.gameObject.GetComponent<PlayerDeath>();
                            playerDeath.HandleDeath();
                        }
                    }
                }
            }
        }
    }
}
