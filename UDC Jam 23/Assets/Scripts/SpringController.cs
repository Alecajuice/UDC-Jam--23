using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class SpringController : MonoBehaviour
{
    [SerializeField] private Animator springHead;
    [SerializeField] private Animator springBody;
    [SerializeField] private AudioClip[] springClips;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        playerController.ApplyVelocity(transform.rotation * Vector3.up * playerController.Speed.magnitude, PlayerForce.Set);
        springHead.SetTrigger("Bounce");
        springBody.SetTrigger("Bounce");
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = springClips[Random.Range(0, springClips.Length)];
        audioSource.Play();
    }
}
