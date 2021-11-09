using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] SceneObject RoomSelect;
    [SerializeField] SceneObject LocalMultiPlaySetUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoomSelect()
    {
        GameInGameUtil.SwitchGameInGameSceneOffline(RoomSelect);
    }

    public void LoadLocalMultiPlaySetUp()
    {
        GameInGameUtil.SwitchGameInGameScene(LocalMultiPlaySetUp);
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
