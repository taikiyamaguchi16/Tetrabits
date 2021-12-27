using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChaseRaceManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    StateAIManager stateAIManager;

    [SerializeField]
    RaceStageMolder raceStageMolder;

    public RaceStageMolder GetRaceStageMolder() { return raceStageMolder; }

    [SerializeField]
    AudioClip bgm;

    [Header("スタート")]

    [SerializeField]
    CountDown countDown;

    public bool started { get; private set; } = false;

    [SerializeField]
    bool firstStage = false;

    [Header("ゴール")]

    [SerializeField]
    Transform startTrans;

    float goalPosZ;

    [SerializeField]
    SceneObject nextScene = null;

    public bool goaled { get; private set; } = false;

    [SerializeField]
    float standBytimeSeconds = 3f;

    float standByTimer = 0f;

    [SerializeField]
    List<GameObject> showObjWhenGoalList = new List<GameObject>();

    [SerializeField]
    AudioClip goalSE;

    [SerializeField]
    bool finalStage = false;

    // Start is called before the first frame update
    void Start()
    {
        goalPosZ = startTrans.position.z + raceStageMolder.GetLaneLength;

        if (firstStage)
        {
            GameInGameUtil.StartGameInGameTimer("race");
        }

        racerController.GetRacerMove().movable = false;
        racerController.GetRacerJump().jumpable = false;
        racerController.GetRacerLaneShift().movable = false;
        racerController.GetRacerLaneShift().belongingLaneId = 1;

        stateAIManager.enabled = false;

        foreach (GameObject obj in showObjWhenGoalList)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // スタート
        if (countDown.isTimeOut && !started)
        {
            started = true;
            SimpleAudioManager.PlayBGMOverride(bgm);

            racerController.GetRacerMove().movable = true;
            racerController.GetRacerJump().jumpable = true;
            racerController.GetRacerLaneShift().movable = true;

            stateAIManager.enabled = true;
        }

        // ゴール判定
        if(racerController.transform.position.z > goalPosZ && !goaled)
        {
            goaled = true;

            racerController.GetRacerMove().movable = false;
            racerController.GetRacerJump().jumpable = false;
            racerController.GetRacerLaneShift().movable = false;

            stateAIManager.enabled = false;

            foreach (GameObject obj in showObjWhenGoalList)
            {
                obj.SetActive(true);
            }

            SimpleAudioManager.PlayBGMCrossFade(bgm, 0f);
            SimpleAudioManager.PlayOneShot(goalSE);
        }

        // ゴール後
        if (goaled)
        {
            standByTimer += Time.deltaTime;
            if(standByTimer >= standBytimeSeconds)
            {
                if (finalStage)
                {
                    GameInGameUtil.StopGameInGameTimer("race");
                    GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                }
                else if (PhotonNetwork.IsMasterClient && nextScene != null)
                {
                    GameInGameUtil.SwitchGameInGameScene(nextScene);   //シーン遷移
                }
            }
        }
    }
}
