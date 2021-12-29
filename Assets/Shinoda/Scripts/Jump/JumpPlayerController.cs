using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;

public class JumpPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer myRenderer;
    PhotonTransformViewClassic photonTransformView;
    Vector3 beforeLocal;
    Vector3 beforeWorld;
    bool isParasol = false;
    bool isJump = false;
    public bool GetJump() { return isJump; }
    public GameObject lastFlag;
    Vector2 padVec;
    Animator animator;
    float beforeVelocityY;
    bool beforeParasol;
    bool playAnim = false;

    [Header("Player")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float parasolGravity = .5f;
    [SerializeField] float normalMoveScale = 1f;
    [SerializeField] float normalSpeedLimit = 3f;
    [SerializeField] float parasolMoveScale = 3f;
    [SerializeField] float parasolSpeedLimit = 5f;
    [SerializeField] bool upOnly;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip hitSE;

    [Header("Arrow")]
    Vector3 originScale;
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowScaleRatio = 1f;
    SpriteRenderer arrowRenderer;

    [Header("Camera")]
    [SerializeField] GameObject cameraObject;
    Camera cameraComponent;
    [SerializeField] float plusY = 2f;
    [SerializeField] float size = 6.5f;

    // Start is called before the first frame update
    void Start()
    {
        beforeLocal = transform.localPosition;
        beforeWorld = transform.position;
        SimpleAudioManager.PlayBGMCrossFade(BGM, 1.0f);
        GameInGameUtil.StartGameInGameTimer("jump");
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        originScale = arrow.transform.localScale;
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        photonTransformView = GetComponent<PhotonTransformViewClassic>();
        arrowRenderer = arrow.GetComponent<SpriteRenderer>();

        if (cameraObject == null) cameraObject = GameObject.Find("MonitorCamera");
        cameraComponent = cameraObject.GetComponent<Camera>();
        cameraComponent.orthographicSize = size;
        cameraObject.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + plusY,
            cameraObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        padVec = TetraInput.sTetraPad.GetVector();
        ArrowControll(padVec);
        if (TetraInput.sTetraButton.GetTrigger() && !isJump) Jump(padVec);

        if (TetraInput.sTetraLever.GetPoweredOn()) isParasol = true;
        else isParasol = false;

        if (beforeParasol && !isParasol)
        {
            AnimAllOff();
            animator.SetBool("standClose", true);
        }
        else if (!beforeParasol && isParasol)
        {
            AnimAllOff();
            animator.SetBool("standOpen", true);
        }

        //isParasol中の落下速度軽減
        if (isParasol && rb.velocity.y < 0) rb.gravityScale = parasolGravity;
        else rb.gravityScale = gravityScale;

        beforeVelocityY = rb.velocity.y;
        beforeParasol = isParasol;
    }

    void FixedUpdate()
    {
        if (isJump)
        {
            if (isParasol)
            {
                if (padVec.x > 0 && rb.velocity.x < parasolSpeedLimit)
                {
                    rb.AddForce(transform.right * padVec.x * parasolMoveScale);
                }
                else if (padVec.x < 0 && rb.velocity.x > -parasolSpeedLimit)
                {
                    rb.AddForce(transform.right * padVec.x * parasolMoveScale);
                }
            }
            else
            {
                if (padVec.x > 0 && rb.velocity.x < normalSpeedLimit)
                {
                    rb.AddForce(transform.right * padVec.x * normalMoveScale);
                }
                else if (padVec.x < 0 && rb.velocity.x > -normalSpeedLimit)
                {
                    rb.AddForce(transform.right * padVec.x * normalMoveScale);
                }
            }
        }

        photonTransformView.SetSynchronizedValues(speed: rb.velocity, turnSpeed: 0);
    }

    void LateUpdate()
    {
        bool isDanger = false;

        if(transform.parent.tag=="JumpMove")
        {
            if(transform.localPosition.y>2.0f)
            {
                // 不正じゃね
                Debug.Log("Danger!!!" + "Move乗ってるはずなのにY座標でかくね");
                isDanger = true;
            }
        }

        if(transform.parent.tag=="Ground")
        {
            float dis = (transform.position - beforeWorld).magnitude;
            if(dis>5)
            {
                // 不正じゃね
                Debug.Log("Danger!!!" + "直前とくそ離れてね?" + "current" + transform.position + "before" + beforeWorld + "dis" + dis);
                isDanger = true;
            }
        }

        if(isDanger)    // 実際の補正処理
        {
            transform.position = beforeWorld;
            Debug.Log("parent：" + transform.parent + "local：" + transform.localPosition + "world：" + transform.position);
        }

        beforeLocal = transform.localPosition;
        beforeWorld = transform.position;

        cameraObject.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y + plusY,
            cameraObject.transform.position.z);
    }

    void Jump(Vector2 _jumpDirection)
    {
        SimpleAudioManager.PlayOneShot(jumpSE);
        if (padVec.x == 0 && padVec.y == 0) rb.AddForce(transform.up.normalized * 2f, ForceMode2D.Impulse);
        else if (padVec.y > 0) rb.AddForce(_jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    void ArrowControll(Vector2 _dir)
    {
        if (padVec.magnitude == 0) arrowRenderer.enabled = false;
        else
        {
            arrowRenderer.enabled = true;
            arrow.transform.localScale = originScale * (_dir.magnitude * arrowScaleRatio);
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        }
    }

    public void JumpOn()
    {
        isJump = true;
    }

    public void JumpOff()
    {
        isJump = false;
        AnimAllOff();
        if (isParasol) animator.SetBool("standParasol", true);
        else animator.SetBool("standNormal", true);
    }

    public void DamageAnimation()
    {
        myRenderer.DOColor(Color.red, 0.5f).SetEase(Ease.OutFlash, 4, 0.5f);
    }

    void AnimAllOff()
    {
        animator.SetBool("standOpen", false);
        animator.SetBool("standClose", false);
        animator.SetBool("standParasol", false);
        animator.SetBool("standNormal", false);
    }
}