using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook:MonoBehaviour {

    [SerializeField] GameObject crosshair;

    Vector3 crosshairStartPos;
    float rotation;
    Camera main;

    void Start() {
        main = Camera.main;
        crosshairStartPos = crosshair.transform.position;
    }

    void Update() {
        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 forcedDirection = transform.position - mousePos;
        rotation = Mathf.Atan2(forcedDirection.y, forcedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);

        //if(Vector3.Distance(main.ScreenToWorldPoint(Input.mousePosition), crosshairStartPos) < 1.5f)
    }
}
