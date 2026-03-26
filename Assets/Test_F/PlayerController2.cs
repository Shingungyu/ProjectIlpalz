using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*polygon collider를 통해 캐릭터의 움직임이 바닥이나 모서리부분에 닿았을때 이상하지 않도록 하였음*/
/*새 코드 위주로 이해 쉽도록 다시 설계(추가로 공부하면서 하는거라 당분간 메모 많을것)*/
/*입력작업 => update메서드, 물리처리 => FixedUpdate메서드*/
public class PlayerController2 : MonoBehaviour
{


    Rigidbody2D rbody;
    float axisH = 0.0f; //입력==>0.0의 입력?
    public float speed = 3.0f; //이동속도 변수

    public float jump = 9.0f;//점프력 변수
    public LayerMask groundLayer;
    bool goJump = false;//점프 개시 플래그
    bool onGround = false;//지면에 서있는 플래그

    private bool isDashing = false;                     // 대쉬 중인지 확인
    private bool canDash = true;                         // 대쉬 가능 여부


    [SerializeField] private Transform groundCheck;
    //private지만 인스펙터에 나타나서 드래그앤 드롭으로 연결가능함

    // Start is called before the first frame update
    //씬에서 한번만 호출
    void Awake()
    {
        //Rigidbody2D 가져오기
        rbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //1프레임에 1번씩 호출됨
    void Update()
    {
        //수평방향으로 입력 확인
        axisH = Input.GetAxisRaw("Horizontal");//GetAxisRaw메서드 = 방향키 플마1.0f 반환
        //방향조절
        if(axisH > 0.0f)
        {
            //우측이동
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            //좌측이동
            transform.localScale = new Vector3(-1, 1); //좌우반전됨!
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();//점프하기!
        }

        ////대쉬
        //if (Input.GetKeyDown(KeyCode.Z) && canDash && !isDashing)
        //{
        //    StartCoroutine(Dash());
        //}


    }
    /*FixedUpdate메서드 = 프레임마다 같은간격으로 호출 메서드*/
    private void FixedUpdate()
    {
        
        onGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        //OverlapCircle메서드를 사용하고, 0.1f 반경 원을 그려 주변에 GroundLayer가 있는지 검사
        //LayerMask에 Ground 레이어를 할당해야 감지함! 밑에 2개는 적었던 흔적
        //LincCast메서드 = 지정한 두 점을 연결하는 선에 오브젝트 접촉검사 후 bool값 반환 메서드
        //transform.up = 벡터(x=0,y=1,z=0)-> *0.1f는 따라서 y축 0.1아래지점이 된다.
        if( onGround || axisH != 0)
        {
            //onGround or axish가 0이 아닌경우=키입력이 0이 아님
            //지면 위 or 속도가 0이 아님
            //속도 갱신하기
            rbody.velocity = new Vector2(speed * axisH, rbody.velocity.y);
        }
        if(onGround && goJump)
        {
            //지면위에서 점프키 눌림
            //점프하기
            Vector2 jumpPw = new Vector2(0, jump); //점프를 위한 점프Pw 벡터 생성
            rbody.AddForce(jumpPw, ForceMode2D.Impulse); //순간적인 힘 가하기
            goJump = false; //점프 플래그 끄기
        }
       
        //속도 갱신
        //rbody.velocity = new Vector2(axisH * 4.0f, rbody.velocity.y);
    }
    public void Jump()
    {
        goJump = true; //점프 플래그 켜기
        Debug.Log("점프 버튼 누름");
    }


    //*수정중*
    //private IEnumerator Dash()
    ////{
    ////    isDashing = true;
    ////    canDash = false;

    ////    // === 대쉬 시작 ===
    ////    float originalGravity = rigid.gravityScale;
    ////    rigid.gravityScale = 0f;

    ////    // 바라보는 방향으로 대쉬
    ////    float dashDirection = transform.localScale.x;
    ////    rigid.velocity = new Vector2(dashDirection * dashSpeed, 0f);

    ////    if (ghostEffect != null) ghostEffect.enabled = true;


    ////    yield return new WaitForSeconds(dashDuration);

    ////    // === 대쉬 종료 ===
    ////    rigid.gravityScale = originalGravity;
    ////    isDashing = false;

    ////    // --- 고스트 이펙트 비활성화 ---
    ////    if (ghostEffect != null) ghostEffect.enabled = false;


    ////    // 쿨타임 대기
    ////    yield return new WaitForSeconds(dashCooldown);
    ////    canDash = true;
    ////}
    ///


    // ** --기존 플레이어 로직-- **

    //float moveSpeed = 6.0f; // 플레이어 이동 속도
    //float jumpForce = 13.5f; // 플레이어 점프력
    //private int jumpCount = 2; // 점프 카운트 추적

    //private bool onGround = true; // 플레이어 지면 접촉
    //public bool isColliding = false; // 벽 접촉 확인

    //private Rigidbody2D rigid;
}
