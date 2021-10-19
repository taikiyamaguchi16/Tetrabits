using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInGameManager : MonoBehaviour
{
    public string gameName = "unknown game";

    [System.NonSerialized]
    public bool isGameEnd;

    private void Awake()
    {
        //登録
        sCurrentGameInGameManager = this;
    }

    private void OnDestroy()
    {
        //登録解除
        if (sCurrentGameInGameManager == this)
        {
            sCurrentGameInGameManager = null;
        }
    }

    //static
    public static GameInGameManager sCurrentGameInGameManager { private set; get; }
}
