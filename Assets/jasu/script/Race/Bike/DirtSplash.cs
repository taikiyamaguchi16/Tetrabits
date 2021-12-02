using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplash : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    //[SerializeField]
    //GameObject dirtPrefab = null;

    [SerializeField]
    float gravity = -100f; // 重力

    [SerializeField]
    Vector3 moveForce = Vector3.forward;

    //public GameObject parentObj { get; set; } = null;

    public GameObject[] parentObjs { get; set; } = null;

    public Vector3 parentMoveForce { get; set; } = Vector3.zero;

    public int parentInstanceID { get; set; }

    public float laneLength { get; set; }

    //public DummyStageMolder dummyStageMolder { get; set; }
    public RaceStageMolder raceStageMolder { get; set; }

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
        foreach(GameObject parent in parentObjs)
        {
            if (other.gameObject.GetInstanceID() == parent.GetInstanceID())
                return;
        }

        if ((other.gameObject.transform.tag == "FlatRoadInRace" && rb.velocity.y > 0f) ||
            other.gameObject.transform.tag == "Slip")
            return;

        Debug.Log(other.transform.parent.name + "にぶつかった");
        Debug.Log(rb.velocity.y);
        Destroy(gameObject);
    }

    // 泥だまり生成
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.transform.tag == "FlatRoadInRace" &&
    //        rb.velocity.y < 0f)
    //    {
    //        GameObject dirt = Instantiate(dirtPrefab, parentObj.transform);
    //        Vector3 pos = Vector3.zero;
    //        dirt.transform.localPosition = pos;
    //        //pos = dirt.transform.position;
    //        pos.x = other.transform.position.x;
    //        pos.y = (other.transform.localPosition.y * 2) - 2.5f;
    //        if(transform.position.z > laneLength)
    //        {
    //            pos.z = transform.position.z - laneLength;
    //        }
    //        else
    //        {
    //            pos.z = transform.position.z;
    //        }
    //        dirt.transform.position = pos;

    //        dirt.GetComponent<DirtDestroy>().raceStageMolder = raceStageMolder;

    //        SpriteRenderOnRoadCtrl spriteRenderOnRoadCtrl;
    //        if ((spriteRenderOnRoadCtrl = dirt.GetComponent<SpriteRenderOnRoadCtrl>()) != null)
    //        {
    //            spriteRenderOnRoadCtrl.GetSetLaneInfo = other.transform.parent.parent.GetComponent<LaneInfo>();
    //        }

    //        raceStageMolder.GetDummyRoadMolder.DummyRoadMold(); // ダミー作成
    //        Destroy(gameObject);
    //    }
    //    //else if(other.gameObject.transform.parent.tag == "SlopeRoadInRace")
    //    //{
    //    //    GameObject dirt = Instantiate(dirtPrefab, other.transform.parent.parent);
    //    //    Vector3 pos = Vector3.zero;
    //    //    dirt.transform.localPosition = pos;
    //    //    pos = dirt.transform.position;
    //    //    pos.z = transform.position.z;
    //    //    dirt.transform.position = pos;

    //    //    dirt.transform.localRotation = other.transform.parent.localRotation;

    //    //    Destroy(gameObject);
    //    //}
    //}
}
