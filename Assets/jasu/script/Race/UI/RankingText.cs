using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingText : MonoBehaviour
{
    Text text = null;

    [SerializeField]
    RacerInfo racerInfo;

    // Start is called before the first frame update
    void Start()
    {
        if ((text = GetComponent<Text>()) == null)
        {
            Debug.Log("text取得失敗");
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = racerInfo.ranking + " th";
    }
}
