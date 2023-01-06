using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement:MonoBehaviour {
    
    [SerializeField] float movementSpeed, runningSpeed;

    [HideInInspector] public bool cantMove;

    float currentMovementSpeed;
    Rigidbody2D rgbd2D;
    Vector2 position;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if(cantMove) {
            rgbd2D.velocity = Vector3.zero;
            return;
        }

        GetInput();
        UpdatePosition();
    }

    void GetInput() {
        position.x = Input.GetAxisRaw("Horizontal");
        position.y = Input.GetAxisRaw("Vertical");
        currentMovementSpeed = (Input.GetKey(KeyCode.LeftShift) ? runningSpeed : movementSpeed);
    }

    void UpdatePosition() {
        rgbd2D.velocity = position.normalized * currentMovementSpeed;
    }
}
