using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftSceneHolder : MonoBehaviour
{
    [SerializeField]
    SceneObject sceneObject;

    public SceneObject GetScene()
    {
        return sceneObject;
    }
}
