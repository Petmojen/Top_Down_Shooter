using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SilencedPistol:MonoBehaviour {
    [SerializeField] TMP_Text ammoCount;

    [Space]
    [SerializeField] Transform barrelTip;
    [SerializeField] Transform casingOutput;

    [Space]
    [SerializeField] float rateOfFire;
    [SerializeField] float bulletSpeed, casingEjectionSpeed, reloadTime, soundDistanceDetection, bulletMinDamage, bulletMaxDamage;

    GunPools gunPools;
    bool bulletCycling, reloading;
    int magazine, magazineSize = 13, totalAmmo = 39;

    void OnEnable() {
        ammoCount.text = magazine.ToString() + "/" + totalAmmo.ToString();
    }

    void Start() {
        gunPools = GameObject.Find("Player").GetComponent<GunPools>();

        totalAmmo -= magazineSize;
        magazine += magazineSize;
        ammoCount.text = magazine.ToString() + "/" + totalAmmo.ToString();
    }

    void Update() {
        if(reloading)
            return;

        if(!Input.GetMouseButtonDown(0))
            return;

        if(magazine <= 0) {
            StartCoroutine(Reload());
            return;
        }

        if(bulletCycling)
            return;

        StartCoroutine(Fire());
    }

    IEnumerator Fire() {
        bulletCycling = true;
        magazine--;
        ammoCount.text = magazine.ToString() + "/" + totalAmmo.ToString();

        //Check which bullets in array are avaible to use
        for(int i = 0; i < gunPools.bulletPoolAmount; i++) {
            if(gunPools.bulletPool[i].activeSelf)
                continue;

            gunPools.bulletPool[i].SetActive(true);
            gunPools.bulletPool[i].transform.position = gunPools.bulletPool[i].GetComponent<BulletBehaviour>().lastPos = barrelTip.position; //Sets bullet start pos
            gunPools.bulletPool[i].GetComponent<BulletBehaviour>().BulletDamage(bulletMinDamage, bulletMaxDamage); //Sets min/max damage of bullet
            gunPools.bulletPool[i].GetComponent<Rigidbody2D>().velocity = bulletSpeed * (Quaternion.Euler(0, 0, Random.Range(-5, 5)) * transform.up); //Sets bullet velocity and random direction
            gunPools.bulletPool[i].transform.up = gunPools.bulletPool[i].GetComponent<Rigidbody2D>().velocity; //Rotates bullet towards its velocity
            break;
        }

        //Check casing pool
        for(int i = 0; i < gunPools.bulletPoolAmount; i++) {
            if(gunPools.casingPool[i].activeSelf)
                continue;

            gunPools.casingPool[i].SetActive(true);
            gunPools.casingPool[i].transform.position = casingOutput.position; //Set casing start pos
            gunPools.casingPool[i].GetComponent<Rigidbody2D>().velocity = casingEjectionSpeed * (Quaternion.Euler(0, 0, Random.Range(-20, 20)) * transform.right); //Set casing velocity and random direction
            break;
        }

        //Check muzzle flash pool
        for(int i = 0; i < gunPools.bulletPoolAmount; i++) {
            if(gunPools.flashPool[i].activeSelf)
                continue;

            gunPools.flashPool[i].SetActive(true);
            gunPools.flashPool[i].transform.position = barrelTip.position; //Set muzzle flash start pos
            gunPools.flashPool[i].transform.up = barrelTip.up; //Set muzzle flash rotation
            gunPools.flashPool[i].GetComponent<ParticleSystem>().Play(); //Play muzzleflash
            break;
        }

        //Zombie sound checker
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, soundDistanceDetection); //Add all zombies within collider to array
        if(zombies.Length > 0) {
            for(int i = 0; i < zombies.Length; i += 2) { //Zombies has two colliders, increasing every two is enough
                if(!zombies[i].CompareTag("Zombie"))
                    continue;

                ZombieBehaviour currentZombie = zombies[i].GetComponent<ZombieBehaviour>(); 
                if(currentZombie.currentState == ZombieBehaviour.ZombieState.Attacking)
                    continue;

                currentZombie.CheckNoiceToPlayer();
            }
        }

        yield return new WaitForSeconds(rateOfFire);
        bulletCycling = false;
        StopCoroutine(Fire());
    }

    IEnumerator Reload() {
        reloading = true;
        totalAmmo -= magazineSize;
        if(totalAmmo >= 0) {
            yield return new WaitForSeconds(reloadTime);
            magazine += magazineSize;
            ammoCount.text = magazine.ToString() + "/" + totalAmmo.ToString();
        }
        reloading = false;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, soundDistanceDetection);
    }
}