using UnityEngine;

public class TpsCamera : MonoBehaviour
{

    [SerializeField] Transform Player;
    [SerializeField] float RotateSpeed;

    float yaw, pitch;

    private void Start()
    {
        RotateSpeed = 1;
    }

    void Update()
    {

        //プライヤー位置を追従する
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z);

        yaw += Input.GetAxis("Mouse X") * RotateSpeed; //横回転入力
        pitch -= Input.GetAxis("Mouse Y") * RotateSpeed; //縦回転入力

        pitch = Mathf.Clamp(pitch, -80, 60); //縦回転角度制限する

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f); //回転の実行
    }
}