using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingCameraStopper : MonoBehaviour
{

    [SerializeField] ShootingCamera shootingCamera;

    private void OnEnable()
    {
        shootingCamera.StopCamera();
    }

    private void OnDisable()
    {
        shootingCamera.RestartCamera();
    }
}
