using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPools:MonoBehaviour {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject casingPrefab, muzzleFlashPrefab;

    [Space]
    [SerializeField] GameObject bulletPoolParent;
    [SerializeField] GameObject casingPoolParent, muzzelFlashParent;

    [Space]
    public int bulletPoolAmount;

    [HideInInspector] public GameObject[] bulletPool, casingPool, flashPool;

    void Start() {
        bulletPool = new GameObject[bulletPoolAmount];
        casingPool = new GameObject[bulletPoolAmount];
        flashPool = new GameObject[3];

        for(int i = 0; i < bulletPoolAmount; i++) {
            bulletPool[i] = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bulletPoolParent.transform);
            bulletPool[i].SetActive(false);

            casingPool[i] = Instantiate(casingPrefab, Vector3.zero, Quaternion.identity, casingPoolParent.transform);
            casingPool[i].SetActive(false);
        }

        for(int i = 0; i < 3; i++) {
            flashPool[i] = Instantiate(muzzleFlashPrefab, Vector3.zero, Quaternion.identity, muzzelFlashParent.transform);
            flashPool[i].SetActive(false);
        }
    }
}
