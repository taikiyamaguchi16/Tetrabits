using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIWindowDebugger : MonoBehaviour
{
    [SerializeField] UnityEvent onGUIWindow;

    private Rect guiWindowRect = new Rect(0, 0, Screen.width / 4, Screen.height / 2);

    private void OnGUI()
    {
        guiWindowRect = GUILayout.Window(0, guiWindowRect, CallbackGUIWindow, "DebugWindow");
        //guiWindowRect = GUI.Window(0, guiWindowRect, CallbackGUIWindow, "DebugWindow");
    }

    private void CallbackGUIWindow(int _windowId)
    {
        //GUI処理呼び出し
        onGUIWindow.Invoke();

        //Drag可能にする
        GUI.DragWindow();
    }
}
