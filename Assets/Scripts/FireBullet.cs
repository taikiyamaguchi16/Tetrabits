using System.Collections;
using System.Collections.Generic;
using SoftGear.Strix.Unity.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBullet : StrixBehaviour {
    public GameObject bullet;
    private PlayerStatus playerStatus;

    // Use this for initialization
    void Start () {
        playerStatus = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update () {
        if (!isLocal) {
            return;
        }

        if (playerStatus != null && playerStatus.health <= 0) {
            return;
        }

	    if (Input.GetButtonDown("Fire1")) {
	        GameObject instance = Instantiate(bullet);
	        Transform firePos = transform.Find("FirePos");

	        BulletControl bulletControl = instance.GetComponent<BulletControl>();
	        bulletControl.owner = gameObject;

            instance.transform.position = firePos.position;
	        instance.transform.rotation = firePos.rotation;
	    }
    }
}
