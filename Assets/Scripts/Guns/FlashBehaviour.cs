using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBehaviour:MonoBehaviour {

    ParticleSystem flash;

    void Start() {
        flash = GetComponent<ParticleSystem>();
    }

    void Update() {
        if(flash.isStopped)
            gameObject.SetActive(false);
    }
}
