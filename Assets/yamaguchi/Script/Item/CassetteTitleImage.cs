using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CassetteTitleImage : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Image titleImage;
    // Start is called before the first frame update

    private void Start()
    {
        titleImage.enabled = false;
    }
    public void SetCassetteTexture(Sprite _texture,Vector2 size)
    {
        if (photonView.IsMine)
        {
            titleImage.rectTransform.sizeDelta = size;
            titleImage.sprite = _texture;
        }
    }

    public void SetCassetteImageActive(bool _fg)
    {
        if (photonView.IsMine)
            titleImage.enabled = _fg;
    }
}
