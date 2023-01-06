using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPushing:MonoBehaviour {

    [SerializeField] float knockBackArea, knockBackForce, knockBackCoolDown;

    Movement movementScript;
    bool isKnockBack;

    void Start() {
        movementScript = GetComponent<Movement>();
    }

    void Update() {
        if(isKnockBack)
            return;

        if(Input.GetKeyDown(KeyCode.F))
            StartCoroutine(MeleeKnockBack());
    }

    IEnumerator MeleeKnockBack() {
        isKnockBack = true;
        movementScript.cantMove = true;
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.4f, knockBackArea);
        if(zombies.Length > 0) {
            for(int i = 0; i < zombies.Length; i += 2) { //Zombies has two colliders, increasing with two so not to double check
                if(!zombies[i].CompareTag("Zombie"))
                    continue;

                ZombieBehaviour currentZombie = zombies[i].GetComponent<ZombieBehaviour>();
                currentZombie.AddPushBack(-currentZombie.transform.up * knockBackForce);
                if(currentZombie.currentState == ZombieBehaviour.ZombieState.Idle)
                    currentZombie.currentState = ZombieBehaviour.ZombieState.Attacking;
            }
        }

        yield return new WaitForSeconds(knockBackCoolDown);
        isKnockBack = false;
        movementScript.cantMove= false;
    }
}
