using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapons:MonoBehaviour {

    [SerializeField] GameObject[] guns;

    int chosenGun;

    void Update() {
        if(!Input.GetKeyDown(KeyCode.Q))
            return;

        chosenGun++;
        if(chosenGun == guns.Length)
            chosenGun = 0;

        for(int i = 0; i < guns.Length; i++) {
            guns[i].SetActive(chosenGun == i);
        }
    }
}
