using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingBehaviour:MonoBehaviour {

    [HideInInspector] public float startSize;
    [SerializeField] float rotationSpeed;
    float rotation, size, sizeSin;

    void OnEnable() {
        sizeSin = 0;    
    }

    void Update() {
        //Rotate casing
        rotation -= rotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, rotation);

        //Change size making it seem to fly up then down to the ground
        transform.localScale = new Vector3(size, size, 1);
        size = startSize + Mathf.Sin(sizeSin * 5);
        sizeSin += Time.deltaTime;

        if(size <= 1.1f)
            gameObject.SetActive(false);
    }
}
