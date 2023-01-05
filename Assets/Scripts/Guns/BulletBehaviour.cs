using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour:MonoBehaviour {

    [SerializeField] float pushBackStrength;
    [HideInInspector] public Vector3 lastPos;

    Rigidbody2D rgbd2D;
    GameObject player;
    float minDamage, maxDamage;

    void Start() {
        rgbd2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if(!gameObject.activeSelf)
            return;

        if(Vector2.Distance(lastPos, transform.position) > 0.1f)
            CheckRay(Vector2.Distance(lastPos, transform.position));

        if(Vector2.Distance(player.transform.position, transform.position) < 16)
            return;

        rgbd2D.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    void CheckRay(float distance) {
        RaycastHit2D hit = Physics2D.Raycast(lastPos, transform.up, distance);
        if(hit) {
            if(!hit.collider.CompareTag("Zombie"))
                return;

            GameObject zombieTagged = hit.collider.gameObject;

            zombieTagged.GetComponent<ZombieBehaviour>().AddPushBack(rgbd2D.velocity.normalized * pushBackStrength);
            zombieTagged.GetComponent<ZombieHealth>().TakeDamage(Random.Range(minDamage, maxDamage));
            gameObject.SetActive(false);
        }

        lastPos = transform.position;
    }

    public void BulletDamage(float minDamageInput, float maxDamageInput) {
        minDamage = minDamageInput;
        maxDamage = maxDamageInput;
    }
}
