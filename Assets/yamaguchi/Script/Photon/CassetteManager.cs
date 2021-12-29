using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CassetteManager : MonoBehaviourPunCallbacks
{
    //出現条件なし設定
    [SerializeField]
    private bool noCassetteAppearConditions;
    [SerializeField]
    private List<Cassette> cassetteList = new List<Cassette> { };

    [SerializeField]
    CassetteHolder cassetHolder;

    [SerializeField]
    VisualEffect outCassetteEfect;

    private Cassette activeCassette;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var ca in cassetteList)
        {
            ca.gameObject.SetActive(false);
            ca.gameObject.transform.parent = null;
        }
        RPCHideAllCassette();
    }

    public void SetActiveCassette(Cassette _ca)
    {
        activeCassette = _ca;
        photonView.RPC(nameof(RPCSetActiveCassette), RpcTarget.Others, activeCassette.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    private void RPCSetActiveCassette(int _id)
    {
        activeCassette = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<Cassette>();
    }
    public void CallHideAllCassette()
    {
        photonView.RPC(nameof(RPCHideAllCassette), RpcTarget.All);
    }

    [PunRPC]
    public void RPCHideAllCassette()
    {
        foreach (var ca in cassetteList)
            ca.gameObject.SetActive(false);
    }

    public void AppearAllCassette()
    {
        foreach (var ca in cassetteList)
        {
            if (!ca.GetIsClear())
            {
                ca.gameObject.SetActive(true);
            }
        }
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
            activeCassette.CallDumpCassette(photonView.ViewID);

            activeCassette.GetComponent<Rigidbody>().AddForce(-this.transform.forward * 3f + Vector3.up * 15f,ForceMode.Impulse);

            photonView.RPC(nameof(RPCPlayCassetEfect), RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPCPlayCassetEfect()
    {
        outCassetteEfect.SendEvent("OnPlay");
    }
}