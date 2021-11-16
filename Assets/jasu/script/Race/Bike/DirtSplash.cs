using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplash : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    GameObject dirtPrefab = null;

    [SerializeField]
    float gravity = -100f; // 重力

    [SerializeField]
    Vector3 moveForce = Vector3.forward;

    public Vector3 parentMoveForce { get; set; } = Vector3.zero;

    public int parentInstanceID { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        moveForce += parentMoveForce;
        rb.AddForce(moveForce, ForceMode.Impulse);
    }
    

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "FlatRoadInRace" &&
            rb.velocity.y < 0f)
        {
            GameObject dirt = Instantiate(dirtPrefab, other.transform.parent.parent);
            Vector3 pos = Vector3.zero;
            dirt.transform.localPosition = pos;
            pos = dirt.transform.position;
            pos.y = (other.transform.localPosition.y * 2) - 2.5f;
            pos.z = transform.position.z;
            dirt.transform.position = pos;
            Destroy(gameObject);
        }
        //else if(other.gameObject.transform.parent.tag == "SlopeRoadInRace")
        //{
        //    GameObject dirt = Instantiate(dirtPrefab, other.transform.parent.parent);
        //    Vector3 pos = Vector3.zero;
        //    dirt.transform.localPosition = pos;
        //    pos = dirt.transform.position;
        //    pos.z = transform.position.z;
        //    dirt.transform.position = pos;

        //    dirt.transform.localRotation = other.transform.parent.localRotation;

        //    Destroy(gameObject);
        //}
    }
}
