using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow:MonoBehaviour {

    GameObject player;
    Rigidbody2D rgbd2D;
    Vector3 playerCenter, playerVelocity, computedOffset;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rgbd2D = player.GetComponent<Rigidbody2D>();
    }

    void Start() {

    }

    void Update() {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        return;
        if(rgbd2D.velocity.magnitude < 0.3f)
            return;

        playerCenter = player.transform.position;
        playerVelocity = rgbd2D.velocity;
        computedOffset = new Vector3(playerCenter.x + playerVelocity.x * 0.5f, playerCenter.y + playerVelocity.y * 0.5f, -10);

        transform.position = Vector3.SmoothDamp(transform.position, computedOffset, ref playerVelocity, 8);
    }
}
