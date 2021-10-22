using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class CassetteManager : MonoBehaviourPunCallbacks
{
    //出現条件なし設定
    [SerializeField]
    private bool noConditions;
    [SerializeField]
    private List<Cassette> cassetteList = new List<Cassette> { };

    [SerializeField]
    CassetteHolder cassetHolder;

    private Cassette activeCassette;
    // Start is called before the first frame update
    void Start()
    {
        if(noConditions)
        {
            foreach (var　ca in cassetteList)
            {
                ca.gameObject.SetActive(true);
                ca.gameObject.transform.parent = null;
                
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ActiveCassetIsClearOn();
        }
    }

    //ルームにプレイヤーが参加したとき
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.PlayerList.Length + newPlayer.NickName);
    }

    public void SetActiveCassette(Cassette _ca)
    {
        activeCassette = _ca;
    }

    public void HideAllCassette()
    {
        foreach (var ca in cassetteList)
            ca.gameObject.SetActive(false);
    }

    public void AppearAllCassette()
    {
        foreach (var ca in cassetteList)
            ca.gameObject.SetActive(true);
    }
    //全クリチェック
    public bool CheckAllCassette()
    {
        foreach (var ca in cassetteList)
        {
            if (!ca.GetIsClear())
                return false;
        }
        return true;
    }

    //カセットのクリアフラグを立てる
    public void ActiveCassetIsClearOn()
    {
        if (activeCassette != null)
        {
            activeCassette.SetIsClearOn();
            AppearAllCassette();
            activeCassette.Dump(this.gameObject);

            activeCassette.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 3f + Vector3.up * 15f,ForceMode.Impulse);
        }
        Debug.Log("ここまで");
    }
}