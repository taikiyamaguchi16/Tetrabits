using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInGameResultController : MonoBehaviour
{
    [SerializeField] string gameInGameTag;
    [SerializeField] Text timerText;
    bool sorted = false;

    public void SetTime(float _startTime, float _endTime)
    {
        var seconds = (_endTime - _startTime);
        int minutes = (int)(seconds / 60.0f);
        seconds -= minutes * 60.0f;

        timerText.text = minutes+"分"+(int)seconds + "秒";
    }

    public void SetPosition(Vector3 _pos)
    {
        transform.position = _pos;
        sorted = true;
    }

    public bool IsSorted()
    {
        return sorted;
    }

    public bool CompareGameInGameTag(string _tag)
    {
        return gameInGameTag == _tag;
    }

}
