using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShift : MonoBehaviour
{
    [SerializeField]
    SceneObject sceneObject;

    public SceneObject GetScene()
    {
        return sceneObject;
    }
}
