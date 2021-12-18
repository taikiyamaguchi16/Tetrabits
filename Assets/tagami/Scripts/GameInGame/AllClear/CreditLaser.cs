using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CreditLaser : MonoBehaviour
{
    [SerializeField] GameObject targetObj;
    [SerializeField] VisualEffect laserEffect;
    bool oldLeverPowerdOn;
    [SerializeField] float laserForce = 1.0f;

    [SerializeField] GameObject debrisImagePrefab;
    [SerializeField] float shotSpeed = 5.0f;
    [SerializeField] Transform canvasParent;

    private void Update()
    {

        //レバーで冷却
        if (TetraInput.sTetraLever.GetPoweredOn() && !oldLeverPowerdOn)
        {
            laserEffect.Play();
        }
        else if (!TetraInput.sTetraLever.GetPoweredOn() && oldLeverPowerdOn)
        {
            laserEffect.Stop();
        }
        oldLeverPowerdOn = TetraInput.sTetraLever.GetPoweredOn();

        //ボタンでデブリ射出
        if (TetraInput.sTetraButton.GetTrigger())
        {
            var obj = Instantiate(debrisImagePrefab, transform.position, Quaternion.identity, canvasParent);
            obj.GetComponent<Rigidbody2D>().velocity = transform.right * shotSpeed;
        }

        //パッド操作
        if (targetObj)
        {
            var endRotation = Quaternion.FromToRotation(Vector3.right, (targetObj.transform.position - transform.position).normalized);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, endRotation, 0.5f);
        }
        else
        {
            var endRotation = Quaternion.FromToRotation(Vector3.right, (TetraInput.sTetraPad.GetVector()).normalized);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, endRotation, 0.5f);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            collision.attachedRigidbody?.AddForce(transform.right * laserForce);
        }
    }

}
