using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    CRTRamdomNoise crtNoise = null;

    // Start is called before the first frame update
    void Start()
    {
        // カメラ寄せる
        VirtualCameraManager.OnlyActive(0);

        foreach (GameObject after in afterObjList)
        {
            after.SetActive(false);
        }

        GameObject mainManager = GameObject.Find("GameMainManager");
        if (mainManager)
            gameSwitcher = mainManager.GetComponent<GameInGameSwitcher>();
        else
            Debug.Log("GameMainManager取得失敗");

        GameObject display = GameObject.Find("Display");
        if (display)
            crtNoise = display.GetComponent<CRTRamdomNoise>();
        else
            Debug.Log("Display取得失敗");
    }

    // Update is called once per frame
    void Update()
    {
        if (modeSelectFlag)
        {
            if(crtNoise != null)
            {
                crtNoise.noiseActive = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) || XInputAnyButton.GetAnyButtonTrigger(XButtonType.A))
            {
                if (gameSwitcher)
                    gameSwitcher.SwitchGameInGameScene(cursor.GetSelectedObj().GetComponent<SceneHolder>().GetScene());
                else
                    SceneManager.LoadScene(cursor.GetSelectedObj().GetComponent<SceneHolder>().GetScene());
            }
        }
        else
        {
            if (crtNoise != null)
            {
                crtNoise.noiseActive = false;
            }

            if (Input.anyKeyDown || XInputAnyButton.GetAnyButtonTrigger())
            {
                modeSelectFlag = true;
                beforeObj.DestroyObject();
                foreach (GameObject after in afterObjList)
                {
                    after.SetActive(true);
                }
            }
        }
    }
}
