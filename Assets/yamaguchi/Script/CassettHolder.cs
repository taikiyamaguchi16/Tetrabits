using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteHolder : MonoBehaviour, IPlayerAction
{
    private Cassette ownCassette;

    private ItemPocket pocket;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();

        if (otherPocket.GetItem() != null)
        {
            ownCassette = otherPocket.GetItem().GetComponent<Cassette>();
            //カセットだった場合セット
            if (ownCassette != null)
            {
                //プレイヤーのアイテムを取得してセット
                pocket.SetItem(otherPocket.GetItem());
                ownCassette.Dump(_desc.playerObj);
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 150;
    }
}
