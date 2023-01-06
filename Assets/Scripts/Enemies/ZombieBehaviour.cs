using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ZombieBehaviour:MonoBehaviour {

    [HideInInspector]
    public enum ZombieState {
        Idle, Attacking, Die
    }

    public ZombieState currentState;

    [SerializeField] float movementSpeed, idelSpeed;

    [Header("FOV")]
    [SerializeField] float fieldOfView;
    [SerializeField] float rayCastStep, fieldOfViewRange;

    Rigidbody2D rgbd2D;
    GameObject player;
    float rotation;
    bool pushedBack, idleCoroutineRunning;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentState = ZombieState.Idle;
    }

    void Update() {
        if(pushedBack)
            return;

        switch(currentState) {
            case ZombieState.Idle:
                if(!idleCoroutineRunning)
                    StartCoroutine(RandomMovement());

                IdleCheckForPlayer();
                break;
            case ZombieState.Attacking:
                MoveTowardsPlayer();
                break;
            case ZombieState.Die:

                break;
        }
    }

    //When idle, check if player is in view
    void IdleCheckForPlayer() {
        if(Vector3.Distance(player.transform.position, transform.position) > 17)
            return;

        for(float rayOffset = -fieldOfView; rayOffset <= fieldOfView; rayOffset += rayCastStep) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.33f, Quaternion.Euler(0, 0, rayOffset) * transform.up, fieldOfViewRange);
            //Debug.DrawRay(transform.position + transform.up * 0.33f, Quaternion.Euler(0, 0, rayOffset) * (transform.up * fieldOfViewRange));
            if(!hit)
                continue;

            if(!hit.collider.CompareTag("Player"))
                continue;

            currentState = ZombieState.Attacking;
        }
    }

    //Random idle movement
    IEnumerator RandomMovement() {
        idleCoroutineRunning = true;
        yield return new WaitForSeconds(Random.Range(2.5f, 10f));
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        rgbd2D.velocity = transform.up * idelSpeed;
        yield return new WaitForSeconds(Random.Range(1, 3));
        rgbd2D.velocity = Vector3.zero;
        idleCoroutineRunning = false;
        StopCoroutine(RandomMovement());
    }

    //MoveTowards Player
    void MoveTowardsPlayer() {
        //Move towards player
        Vector3 forcedDirection = player.transform.position - transform.position;
        rgbd2D.velocity = forcedDirection.normalized * movementSpeed;

        //Rotate towards player
        rotation = Mathf.Atan2(forcedDirection.y, forcedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation - 90);
    }


    //If player shoot, check if zombie can see player
    public void NoiceCheckToPlayer() {
        Vector3 forcedDirection = player.transform.position - transform.position;
        rotation = Mathf.Atan2(forcedDirection.y, forcedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation - 90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.33f, transform.up);
        if(!hit.collider.CompareTag("Player"))
            return;

        StopCoroutine(RandomMovement());
        currentState = ZombieState.Attacking;
    }


    //If shot by bullet add pushback
    public void AddPushBack(Vector3 pushBack) {
        pushedBack = true;
        rgbd2D.AddForce(pushBack, ForceMode2D.Impulse);
        StopCoroutine(RandomMovement());
        currentState = ZombieState.Attacking;
        Invoke(nameof(NoPushBack), 0.2f);
    }

    void NoPushBack() {
        pushedBack = false;
        CancelInvoke();
    }
}
