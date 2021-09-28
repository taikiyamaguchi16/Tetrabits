using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {
    public float speed = 100.0f;
    public float lifetime = 2.0f;
    public float force = 500;
    public GameObject owner;
    public GameObject hitEffect;
    private float startTime = 0;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= startTime + lifetime) {
	        Destroy(gameObject);
	        return;
	    }

        Vector3 pos = transform.position;
	    float distance = speed * Time.deltaTime;

        RaycastHit hitInfo;

	    if (Physics.Raycast(new Ray(pos, transform.forward), out hitInfo, distance)) {
	        GameObject hitObject = hitInfo.collider.gameObject;
	        Rigidbody rigidbody = hitObject.GetComponent<Rigidbody>();

	        if (rigidbody != null) {
	            rigidbody.AddForce(hitInfo.normal * -force);
            }

            if (hitObject != owner) {
                PlayerStatus playerStatus = hitObject.GetComponent<PlayerStatus>();

                if (playerStatus != null && playerStatus.isLocal) {
                    playerStatus.RpcToRoomOwner("OnHit");
                }

                transform.position = hitInfo.point;

	            GameObject effect = Instantiate(hitEffect);
	            effect.transform.position = hitInfo.point;

	            Destroy(effect, 2);
                Destroy(gameObject);
            }

	        return;
	    }

	    transform.position = pos + transform.forward * distance;
    }
}
