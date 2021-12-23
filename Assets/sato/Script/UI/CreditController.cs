using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    [SerializeField]
    [Header("表示したいオブジェクト")]
    GameObject[] activeObjects;

    [SerializeField]
    [Header("決定音")]
    AudioClip decisionSe;

    [SerializeField]
    float Volume = 0.3f;

    int controllerID = 0;

    TitleDemoManager DemoManager;

    // Start is called before the first frame update
    void Start()
    {
        DemoManager = GameObject.Find("DemoManager").GetComponent<TitleDemoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || XInputManager.GetButtonTrigger(controllerID, XButtonType.B))
        {
            for (int i = 0; i < activeObjects.Length; i++)
            {
                activeObjects[i].SetActive(true);
            }

            gameObject.SetActive(false);
            DemoManager.isStopInstantiateSwitcher(true);

            SimpleAudioManager.PlayOneShot(decisionSe, Volume);
        }
    }
}
