using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth:MonoBehaviour {

    public float healthPoints;

    void Start() {

    }

    void Update() {

    }

    public void TakeDamage(float damage) {
        healthPoints -= damage;
        if(healthPoints > 0)
            return;

        Destroy(gameObject);
    }
}
