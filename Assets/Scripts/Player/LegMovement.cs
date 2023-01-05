using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement:MonoBehaviour {

    [SerializeField] LegMovement otherFoot;
    [SerializeField] float footDistance;
    public bool isMoving;

    GameObject player;
    Rigidbody2D rgbd2D;
    Vector3 lastPosition, footCenter;

    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        rgbd2D = player.GetComponent<Rigidbody2D>();
    }

    void Start() {
        lastPosition = transform.position;
        footCenter = transform.localPosition;
    }

    void Update() {
        if(otherFoot.isMoving)
            return;

        if(Vector2.Distance(player.transform.position + footCenter, lastPosition) > 1f && !isMoving)
            isMoving = true;

        if(isMoving)
            MoveLeg();
    }

    void MoveLeg() {
        lastPosition = player.transform.position + footCenter + (Vector3)rgbd2D.velocity.normalized * 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, lastPosition, 0.25f);
    }
}
