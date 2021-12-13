using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class InGameTitleController : MonoBehaviour
{
    enum CanvasState
    {
        Title,
        Option
    }
    CanvasState state = CanvasState.Title;

    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject optionPanel;
    [SerializeField] Text titleText;
    [SerializeField] Text optionText;
    [SerializeField] Ease easeType;

    [SerializeField] SceneObject nextScene;
    bool loadable = true;

    RectTransform optionTransform;

    // Start is called before the first frame update
    void Start()
    {
        optionTransform = optionPanel.GetComponent<RectTransform>();
        optionTransform.localScale = new Vector3(1, 0, 1);
        //titlePanel.SetActive(true);
        //optionPanel.SetActive(false);
        if (PhotonNetwork.IsMasterClient) MonitorManager.CallResetNumDebrisInGameMainStage();
        FlashPressButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger())
        {
            if (state == CanvasState.Title) OpenOption();
            else if (state == CanvasState.Option && PhotonNetwork.IsMasterClient && loadable)
            {
                GameInGameUtil.SwitchGameInGameScene(nextScene);
                loadable = false;
            }
        }
    }

    void OpenOption()
    {
        optionTransform.DOScale(1f, .5f).SetEase(Ease.Linear).OnComplete(() =>
         {
             state = CanvasState.Option;
         });
    }

    void FlashPressButton()
    {
        titleText.DOFade(0.0f, 1f).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
        optionText.DOFade(0.0f, 1f).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
}