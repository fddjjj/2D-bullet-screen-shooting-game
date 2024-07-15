using System;
using System.Collections;

using UnityEngine;

using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    [Header("基本组件")]
    public Rigidbody2D rb;
    public PlayerInputControl inputControl;
    public Animator animator;
    public GameObject playerImage;
    public GameObject flyPoint;
    [Header("基本参数")]
    public Vector2 moveDirection;//移动方向
    public float moveSpeed;//速度上限
    public float moveAcceleration;//加速度
    public Vector2 currentVelocity;//当前速度
    public float jumpForce;//跳跃力
    public float playerOnGroundRaduis;//检查人物是否在地面上的范围\
    public LayerMask groundLayer;//被检测的地面layer
    public float playerGravityScale;//角色重力
    public float playerOriginalGravityScale;
    public float playerFlyGravityScale;
    public float playerSlowDownSpeed;//限制角色缓降速度
    public float slideSpeed;//角色滑行速度
    public bool isFlyying;//是否在飞行
    public float flySpeed;//飞行速度
    [Header("角色状态")]
    public bool isOnGround;//检测角色是否处于地面上
    public bool isSlide;

    private void Awake()
    {
        //组件获取
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        animator = GetComponent<Animator>();


        //动作绑定
        //跳跃和缓降
        inputControl.Player.Jump.started += Jump;
        inputControl.Player.Jump.performed += SlowDown;
        inputControl.Player.Jump.canceled +=  CancelSlowDown;
        //冲刺和空中飞行
        inputControl.Player.Slide.started += Slide;
        inputControl.Player.Slide.performed += Fly;
        inputControl.Player.Slide.canceled += EndFly;

        //初始值赋予
        playerOriginalGravityScale = rb.gravityScale;
    }


    private void Update()
    {
        moveDirection = inputControl.Player.Move.ReadValue<Vector2>();
        currentVelocity = rb.velocity;
        AnimatorUpdate();
        PlayerEnvirmentCheck();






        //debug用
        //playerGravityScale = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        move();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }

    #region 人物移动控制
    public void move()
    {
        if (isSlide)
            return;
        if (moveDirection.x == 0) 
        { 
            rb.velocity = new Vector2(0,rb.velocity.y);
            return;
        } 

        //加速部分
        if(Mathf.Abs(currentVelocity.x)   < moveSpeed)
        {
            rb.velocity = new Vector2( currentVelocity.x +  moveDirection.x * moveAcceleration * Time.deltaTime ,rb.velocity.y) ;
        }else
        {
            rb.velocity = new Vector2(moveSpeed * moveDirection.x, rb.velocity.y);
        }
        //根据当前速度方向改变人物朝向
        if (rb.velocity.x >= 0) transform.localScale= new Vector3(1, 1, 1);
        else transform.localScale = new Vector3(-1, 1, 1);

    }
    private void Jump(InputAction.CallbackContext context)
    {
        // UnityEngine.Debug.Log("Jump");
        if (isSlide)
            return;
        if (!isOnGround)
            return;
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");

    }
    private void SlowDown(InputAction.CallbackContext context)
    {
      //  UnityEngine.Debug.Log("SLowDown");
        StartCoroutine("SlowDownCheck");
    }
    private void CancelSlowDown(InputAction.CallbackContext context)
    {
       // UnityEngine.Debug.Log("CancelSlowDown");
        StopCoroutine("SlowDownCheck");
       // Debug.Log("StopCoroutine");
        //rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y);
        //rb.gravityScale = playerOriginalGravityScale;
    }
    IEnumerator SlowDownCheck()
    {
        //Debug.Log(rb.velocity.y);
        while (!isOnGround || rb.velocity.y != 0)
        {
            
            if(rb.velocity.y < 0 && !isOnGround)
            {
                //rb.gravityScale = playerSlowDownGravityScale;
                //yield break;
                rb.velocity = new Vector2(rb.velocity.x, playerSlowDownSpeed);
                //Debug.Log("change Velocity");
            }
            yield return null;
        }
    }

    private void Slide(InputAction.CallbackContext context)
    {
        if (!isOnGround)
            return;
        Vector3 slideDir = transform.localScale;
        animator.SetTrigger("Slide");
        //TODO:添加无敌
        isSlide = true;
        StartCoroutine("WhileSlide");
    }
    IEnumerator WhileSlide()
    {
        while (isSlide)
        {
            rb.velocity = new Vector2(slideSpeed * transform.localScale.x, rb.velocity.y);
            yield return null;
        }
        yield break;
    }

    private void Fly(InputAction.CallbackContext context)
    {
        //TODO:添加飞行，取消和子弹的碰撞
        if (isOnGround)
            return;
        isFlyying = true;
        playerImage.SetActive(false);
        flyPoint.SetActive(true);
        rb.gravityScale = playerFlyGravityScale;
        StartCoroutine("FlyMove");
    }

    IEnumerator FlyMove()
    {
        //TODO:添加体力条消耗
        while (isFlyying)
        {
            rb.velocity = new Vector2(moveDirection.x * flySpeed,moveDirection.y * flySpeed);
            yield return null;
        }
        yield break;
    }
    private void EndFly(InputAction.CallbackContext context)
    {
        isFlyying = false;
        StopCoroutine("FlyMove");
        playerImage.SetActive(true);
        flyPoint.SetActive(false);
        rb.gravityScale = playerOriginalGravityScale;
    }


    #endregion
    #region 状态同步
    public void AnimatorUpdate()
    {
        animator.SetFloat("VelocityX",Mathf.Abs(rb.velocity.x) );
        animator.SetFloat("VelocityY",rb.velocity.y);
        animator.SetBool("OnGround", isOnGround);
    }
    public void PlayerEnvirmentCheck()
    {
        isOnGround = CheckIsOnGround();
    }
    #endregion

    #region 状态检测
    public bool CheckIsOnGround()
    {
        return  Physics2D.OverlapCircle(transform.position, playerOnGroundRaduis,groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerOnGroundRaduis);
    }
    #endregion

}
