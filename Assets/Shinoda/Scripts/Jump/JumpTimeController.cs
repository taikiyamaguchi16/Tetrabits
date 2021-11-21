using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JumpTimeController : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")] float time;
    [SerializeField, Tooltip("増える時間")] float plusTime;
    [SerializeField, Tooltip("何枚で増えるか")] int plusValue;

    Transform timeTransform;
    Text timeText;
    int remainingTime;
    Transform coinTransform;
    Text coinText;
    int coin;

    // Start is called before the first frame update
    void Start()
    {
        timeTransform = transform.Find("Time");
        timeText = timeTransform.GetComponent<Text>();
        coinTransform = transform.Find("Coin");
        coinText = coinTransform.GetComponent<Text>();
        remainingTime = (int)time;
        coin = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0) time = 0;
        else if (time > 999) time = 999;

        remainingTime = (int)time;
        timeText.text = remainingTime.ToString("d3");
        coinText.text = coin.ToString();
    }

    public void AddCoin()
    {
        coin += 1;
        if (coin >= plusValue)
        {
            time += plusTime;
            coin = 0;
        }
    }
}