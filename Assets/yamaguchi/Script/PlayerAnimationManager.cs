using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
public class PlayerAnimationManager : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        var animatorStateEvent = AnimatorStateEvent.Get(animator, 0);

        //Debug.Log(animatorStateEvent.CurrentStateName);

        // ステートが変わった時のコールバック
        animatorStateEvent.stateEntered += _ => ChangeTexture();
    }

    public void ChangeTexture()
    {
        Debug.Log("変わりました");
    }
}