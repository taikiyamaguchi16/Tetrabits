using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField] SceneObject nextScene;

    RectTransform optionTransform;

    // Start is called before the first frame update
    void Start()
    {
        optionTransform = optionPanel.GetComponent<RectTransform>();
        optionTransform.localScale = new Vector3(1, 0, 1);
        //titlePanel.SetActive(true);
        //optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger())
        {
            if (state == CanvasState.Title) OpenOption();
            else if (state == CanvasState.Option) GameInGameUtil.SwitchGameInGameScene(nextScene);
        }
    }

    void OpenOption()
    {
        optionTransform.DOScale(1f, .5f).SetEase(Ease.Linear).OnComplete(() =>
         {
             state = CanvasState.Option;
         });
    }
}