using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] SceneObject RoomSelect;

    [SerializeField]
    [Header("表示したいオブジェクト")]
    GameObject[] activeObjects;

    [SerializeField]
    [Header("非表示にしたいオブジェクト")]
    GameObject[] deactiveObjects;

    TitleDemoManager DemoManager;

    // Start is called before the first frame update
    void Start()
    {
        DemoManager = GameObject.Find("DemoManager").GetComponent<TitleDemoManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadRoomSelect()
    {
        GameInGameUtil.SwitchGameInGameSceneOffline(RoomSelect);
    }

    public void ShowCredit()
    {
        if(deactiveObjects != null)
        {
            for(int i = 0; i < deactiveObjects.Length; i++)
            {
                deactiveObjects[i].SetActive(false);
            }
        }

        for (int i = 0; i < activeObjects.Length; i++)
        {
            activeObjects[i].SetActive(true);
        }

        DemoManager.isStopInstantiateSwitcher(false);
        DemoManager.PlayerDestroy();
        DemoManager.PlayerCountDown();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
      
            UnityEngine.Application.Quit();
       
#endif
    }
}
