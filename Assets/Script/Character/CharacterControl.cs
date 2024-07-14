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
    public float playerSlowDownGravityScale;
    public float playerSlowDownSpeed;//限制角色缓降速度
    [Header("角色状态")]
    public bool isOnGround;//检测角色是否处于地面上

    private void Awake()
    {
        //组件获取
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        animator = GetComponent<Animator>();


        //动作绑定
        inputControl.Player.Jump.started += Jump;
        inputControl.Player.Jump.performed += SlowDown;
        inputControl.Player.Jump.canceled +=  CancelSlowDown;

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
        playerGravityScale = rb.gravityScale;
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
