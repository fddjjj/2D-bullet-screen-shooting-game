using System;
using System.Collections;

using UnityEngine;

using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    [Header("�������")]
    public Rigidbody2D rb;
    public PlayerInputControl inputControl;
    public Animator animator;
    [Header("��������")]
    public Vector2 moveDirection;//�ƶ�����
    public float moveSpeed;//�ٶ�����
    public float moveAcceleration;//���ٶ�
    public Vector2 currentVelocity;//��ǰ�ٶ�
    public float jumpForce;//��Ծ��
    public float playerOnGroundRaduis;//��������Ƿ��ڵ����ϵķ�Χ\
    public LayerMask groundLayer;//�����ĵ���layer
    public float playerGravityScale;//��ɫ����
    public float playerOriginalGravityScale;
    public float playerSlowDownGravityScale;
    public float playerSlowDownSpeed;//���ƽ�ɫ�����ٶ�
    [Header("��ɫ״̬")]
    public bool isOnGround;//����ɫ�Ƿ��ڵ�����

    private void Awake()
    {
        //�����ȡ
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        animator = GetComponent<Animator>();


        //������
        inputControl.Player.Jump.started += Jump;
        inputControl.Player.Jump.performed += SlowDown;
        inputControl.Player.Jump.canceled +=  CancelSlowDown;

        //��ʼֵ����
        playerOriginalGravityScale = rb.gravityScale;
    }



    private void Update()
    {
        moveDirection = inputControl.Player.Move.ReadValue<Vector2>();
        currentVelocity = rb.velocity;
        AnimatorUpdate();
        PlayerEnvirmentCheck();






        //debug��
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

    #region �����ƶ�����
    public void move()
    {
        if (moveDirection.x == 0) 
        { 
            rb.velocity = new Vector2(0,rb.velocity.y);
            return;
        } 

        //���ٲ���
        if(Mathf.Abs(currentVelocity.x)   < moveSpeed)
        {
            rb.velocity = new Vector2( currentVelocity.x +  moveDirection.x * moveAcceleration * Time.deltaTime ,rb.velocity.y) ;
        }else
        {
            rb.velocity = new Vector2(moveSpeed * moveDirection.x, rb.velocity.y);
        }
        //���ݵ�ǰ�ٶȷ���ı����ﳯ��
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
    #region ״̬ͬ��
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

    #region ״̬���
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
