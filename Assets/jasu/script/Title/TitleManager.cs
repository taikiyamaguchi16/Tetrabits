using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    DestroyObjectCpt beforeObj;

    [SerializeField]
    List<GameObject> afterObjList = new List<GameObject>();

    bool modeSelectFlag = false;

    [SerializeField]
    Cursor cursor;
    
    GameInGameSwitcher gameSwitcher = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject after in afterObjList)
        {
            after.SetActive(false);
        }

        GameObject mainManager = GameObject.Find("GameMainManager");
        if (mainManager)
            gameSwitcher = mainManager.GetComponent<GameInGameSwitcher>();
        else
            Debug.Log("GameMainManager取得失敗");
    }

    // Update is called once per frame
    void Update()
    {
        if (!modeSelectFlag && Input.anyKey && !Input.GetKey(KeyCode.E))
        {
            modeSelectFlag = true;
            beforeObj.DestroyObject();
            foreach(GameObject after in afterObjList)
            {
                after.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(gameSwitcher)
                gameSwitcher.SwitchGameInGameScene(cursor.GetSelectedObj().GetComponent<SceneShift>().GetScene());
        }
    }
}
