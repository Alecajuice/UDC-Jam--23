using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float timer;
    [SerializeField] private AudioSource timerSound;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioClip pressClip;
    [SerializeField] private AudioClip cancelClip;
    [SerializeField] private DoorController[] targets;

    private float deactivateTimer;
    private bool deactivate = false;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetBool("Pressed", true);
        foreach (DoorController target in targets) {
            target.Activate();
        }
        if (!deactivate) {
            timerSound.Play();
            buttonSound.clip = pressClip;
            buttonSound.Play();
        } else {
            deactivate = false;
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        deactivateTimer = 0f;
        deactivate = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (deactivate) {
            deactivateTimer += Time.deltaTime;
            if (deactivateTimer >= timer) {
                animator.SetBool("Pressed", false);
                foreach (DoorController target in targets) {
                    target.Deactivate();
                }
                timerSound.Stop();
                buttonSound.clip = cancelClip;
                buttonSound.Play();
                deactivate = false;
            }
        }
    }
}
