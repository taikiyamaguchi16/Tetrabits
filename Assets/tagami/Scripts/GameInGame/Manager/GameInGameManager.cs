using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInGameManager : MonoBehaviour
{
    public string gameName = "unknown game";

    [System.NonSerialized]
    public bool isGameEnd;
    bool isGameEndTriggered;

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

    public bool IsGameEndTrigger()
    {
        if (isGameEnd && !isGameEndTriggered)
        {
            isGameEndTriggered = true;
            return true;
        }
        else
        {
            return false;
        }

    }
    //**********************************************************
    //static
    public static GameInGameManager sCurrentGameInGameManager { private set; get; }
}
