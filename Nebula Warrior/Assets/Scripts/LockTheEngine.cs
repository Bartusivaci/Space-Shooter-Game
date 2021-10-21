using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTheEngine : MonoBehaviour
{

    [SerializeField] PlayerShip playerShip;


    // Start is called before the first frame update
    void Start()
    {
        transform.parent = playerShip.gameObject.transform;
    }


}
