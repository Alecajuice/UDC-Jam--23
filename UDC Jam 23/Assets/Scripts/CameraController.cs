using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class CameraController : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;

    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;

    //Follow player
    [SerializeField] private float aheadDistance;
    [SerializeField] private Vector2 cameraSpeed;
    [SerializeField] private float yOffset;
    
    public Transform player;
    private bool hasTarget = false;
    private Vector3 targetPos;
    float facing;

    private void Update()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        //Room camera
        // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow player
        if (playerController.Input.x != 0) facing = playerController.Input.x > 0 ? 1 : -1;
        if (!hasTarget)
            targetPos = new Vector3(player.position.x + aheadDistance * facing, player.position.y + yOffset, transform.position.z);
        // Vector2 speed = Vector2.Scale(targetPos - transform.position, cameraSpeed);
        // transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed.magnitude);
        transform.position = new Vector3(Mathf.SmoothDamp(transform.position.x, targetPos.x, ref velocity.x, 1/cameraSpeed.x),
                                         Mathf.SmoothDamp(transform.position.y, targetPos.y, ref velocity.y, 1/cameraSpeed.y),
                                        transform.position.z);
    }

    public void SetTarget(Vector3 target) {
        hasTarget = true;
        targetPos = target;
    }

    public void SetTargetWithLookahead(Vector3 target) {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController.Input.x != 0) facing = playerController.Input.x > 0 ? 1 : -1;
        hasTarget = true;
        targetPos = new Vector3(target.x + aheadDistance * facing, target.y, target.z);
    }

    public void RemoveTarget() {
        hasTarget = false;
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}
