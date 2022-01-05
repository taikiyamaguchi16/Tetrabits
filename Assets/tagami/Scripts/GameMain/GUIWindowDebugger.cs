using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIWindowDebugger : MonoBehaviour
{
    [SerializeField] UnityEvent onGUIWindow;

    private bool openWindow;
    private bool oldOpenWindow;

    private Rect guiWindowRect = new Rect(0, 0, Screen.width / 8, Screen.height / 8);

    private void OnGUI()
    {
#if UNITY_EDITOR
        guiWindowRect = GUILayout.Window(0, guiWindowRect, CallbackGUIWindow, "DebugWindow");
        //guiWindowRect = GUI.Window(0, guiWindowRect, CallbackGUIWindow, "DebugWindow");
#endif
    }

    private void CallbackGUIWindow(int _windowId)
    {
        oldOpenWindow = openWindow;
        openWindow = GUILayout.Toggle(openWindow, "Open");

        if (oldOpenWindow && !openWindow)
        {//閉じる時の処理
            guiWindowRect.width = Screen.width / 8;
            guiWindowRect.height = Screen.height / 8;
        }

        if (openWindow)
        {
            //GUI処理呼び出し
            onGUIWindow.Invoke();
        }
        //Drag可能にする
        GUI.DragWindow();
    }
}
