using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehaviour:MonoBehaviour {

    [HideInInspector] public enum ZombieState {
        Idle, Attacking, Die
    }

    public ZombieState currentState;

    [SerializeField] float movementSpeed, idelSpeed;
    //float currentSpeed;

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

        //Field of view, if see player, attack

        switch(currentState) {
            case ZombieState.Idle:
                if(!idleCoroutineRunning)
                    StartCoroutine(RandomMovement());

                break;
            case ZombieState.Attacking:
                MoveTowardsPlayer();
                break;
            case ZombieState.Die:

                break;
        }
    }

    //Random idle movement
    IEnumerator RandomMovement() {
        idleCoroutineRunning = true;
        yield return new WaitForSeconds(Random.Range(2.5f, 10f));
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        rgbd2D.velocity = transform.up * idelSpeed;
        Debug.Log(rgbd2D.velocity);
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
    public void CheckNoiceToPlayer() {
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
        Invoke(nameof(NoPushBack), 0.1f);
    }

    void NoPushBack() {
        pushedBack = false;
        CancelInvoke();
    }
}
