using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalMultiSetUper : MonoBehaviour
{
    [SerializeField] SceneObject nextScene;

    [SerializeField] List<GameObject> moveToActiveSceneObjects;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var obj in moveToActiveSceneObjects)
        {
            SceneManager.MoveGameObjectToScene(obj, SceneManager.GetActiveScene());
        }

        //カセット表示オン
        //とりあえずFindでテスト
        var cassetHolderObj = GameObject.Find("CassetHolder");
        cassetHolderObj.GetComponent<CassetteManager>().AppearAllCassette();
    }

    // Update is called once per frame
    void Update()
    {
        //bool trigger = false;
        //for (int i = 0; i < 4; i++)
        //{
        //    if (XInputManager.GetButtonTrigger(i, XButtonType.A))
        //    {
        //        trigger = true;
        //        break;
        //    }
        //}
        //if (trigger)
        //{
        //    var managerObj = GameObject.Find("GameMainManager");
        //    managerObj.GetComponent<GameInGameSwitcher>().SwitchGameInGameScene(nextScene);
        //}
    }
}
