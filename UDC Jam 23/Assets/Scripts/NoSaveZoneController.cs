using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSaveZoneController : MonoBehaviour
{
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSaveLoadController saveLoadController = other.GetComponentInChildren<PlayerSaveLoadController>();
        if (saveLoadController != null) {
            saveLoadController.DisableSave();
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerSaveLoadController saveLoadController = other.GetComponentInChildren<PlayerSaveLoadController>();
        if (saveLoadController != null) {
            saveLoadController.EnableSave();
        }
    }
}
