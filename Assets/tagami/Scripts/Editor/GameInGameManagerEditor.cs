using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR

//using UnityEditor;

//[CustomEditor(typeof(GameInGameManager))]
//public class GameInGameManagerEditor : Editor
//{
//    GameInGameManager _target;

//    private void OnEnable()
//    {
//        //_target = (GameInGameManager)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        //EditorGUI.BeginChangeCheck();
//        serializedObject.Update();

//        //base.OnInspectorGUI();



//        // インスペクターに表示する
//        //EditorGUILayout.PropertyField(publicValue);

//        var isGame = serializedObject.FindProperty("isGame");
//        isGame.boolValue = EditorGUILayout.Toggle(isGame.boolValue);

//        //EditorGUILayout.PropertyField(serializedObject.FindProperty("gameName"));


//        //_target.isGame = EditorGUILayout.Toggle(_target.isGame);
//        //if (_target.isGame)
//        //{
//        //    _target.gameName = EditorGUILayout.TextField(_target.gameName);
//        //}

//        serializedObject.ApplyModifiedProperties();

//        //if (EditorGUI.EndChangeCheck())
//        //{
//        //    Undo.RecordObject(_target, "Change Property");
//        //    EditorUtility.SetDirty(_target);
//        //}
//    }
//}
//#endif
