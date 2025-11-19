using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Player_DashController : MonoBehaviour
{
    float moveSpeed = 6.0f; // 플레이어 이동 속도
    float jumpForce = 13.5f; // 플레이어 점프력
    private int jumpCount = 2; // 점프 카운트 추적

    private bool onGround = true; // 플레이어 지면 접촉
    public bool isColliding = false; // 벽 접촉 확인

    private Rigidbody2D rigid;
    private Animator anim;

    public AudioSource audioSource; // 오디오 소스

    public AudioClip jumpSound; //점프사운드
    public AudioClip portalKeySound; //포탈사운드
    public AudioClip Deathsound; //죽음사운드

   
    //

    // [SerializeField]사용이유:C#스크립트간에서는 접근하지못하도록 막으며, 인스펙터창에 띄우기 위함.(인스펙터 창에서 수정하기위해)

    [SerializeField] private float dashSpeed = 15f;    // 대쉬 시 속도
    [SerializeField] private float dashDuration = 0.2f; // 대쉬 지속 시간
    [SerializeField] private float dashCooldown = 1.0f; // 대쉬 쿨타임
    private bool isDashing = false;                     // 대쉬 중인지 확인
    private bool canDash = true;                         // 대쉬 가능 여부

    [SerializeField] private Ghost ghostEffect;


    //

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화

        // Resources 폴더에서 GameSounds/Playerjump 사운드 로드
        jumpSound = Resources.Load<AudioClip>("GameSounds/Playerjump");
        portalKeySound = Resources.Load<AudioClip>("GameSounds/Potalkey");
        Deathsound = Resources.Load<AudioClip>("GameSounds/Deathsound");
        // 게임 시작 시 Rigidbody2D의 중력 설정
        rigid.gravityScale = 4.0f; // 원하는 중력 값으로 설정
    }

    void Update()
    {
        // 플레이어 좌,우 이동
        if (!isDashing)
        {
            float moveDirection = Input.GetAxis("Horizontal");
            rigid.velocity = new Vector2(moveDirection * moveSpeed, rigid.velocity.y);

            // 바라보는 방향 전환 & 걷기 애니메이션
            if (moveDirection > 0)
            {
                transform.localScale = new Vector2(1, 1);
                anim.SetBool("isWalking", true);
            }
            else if (moveDirection < 0)
            {
                transform.localScale = new Vector2(-1, 1);
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }

        // 지면 접촉 시, 점프 카운트 초기화
        if (onGround)
        {
            jumpCount = 1;
        }

        // 점프 키 누를 시, 점프 카운트 조건에 따라 점프 발생
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpCount--;

            // y축 속도를 0으로 초기화하여 이전 점프/하강 속도를 제거
            rigid.velocity = new Vector2(rigid.velocity.x, 0);

            // 점프력을 적용
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            PlayJumpSound();


        }
        if (Input.GetKeyDown(KeyCode.Z) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }

     



        // 점프, 추락 애니메이션 업데이트
        UpdateAnimation();

    }

    void FixedUpdate()
    {
        Debug.DrawRay(rigid.position, Vector2.down, new Color(0, 1, 0));

        RaycastHit2D bottomRay = Physics2D.Raycast(rigid.position, Vector2.down, 1, LayerMask.GetMask("BG1"));

        if (bottomRay.collider != null)
        {
            if (bottomRay.distance < 0.6f)
            {
                onGround = true;
            }
        }
        else
        {
            onGround = false;
        }

        if (isColliding)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    void UpdateAnimation()
    {
        if (onGround)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
        else
        {
            if (rigid.velocity.y > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
            else
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
            }
        }
    }

    private void Die()
    {

        rigid.velocity = Vector2.zero;

        anim.SetTrigger("isDie");
        rigid.bodyType = RigidbodyType2D.Static; // 플레이어 위치 고정

        // 애니메이션이 전환될 시간을 잠시 기다립니다.
        StartCoroutine(WaitForAnimation());
    }

    private void GoUp()
    {
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return null; // 한 프레임 대기
        yield return new WaitForSeconds(0.12f); // 약간의 추가 대기

        // 사망 애니메이션 길이를 가져옵니다.
        float dieDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(RestartScene(dieDuration));
    }

    private IEnumerator RestartScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("JumpingBar"))
        {
            // GoUp(); 위로 튕김
            // jumpForce = 7; 점프력 더 강해짐
        }*/
    }

    //대쉬 코루틴
    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        // === 대쉬 시작 ===
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f;

        // 바라보는 방향으로 대쉬
        float dashDirection = transform.localScale.x;
        rigid.velocity = new Vector2(dashDirection * dashSpeed, 0f);

        if (ghostEffect != null) ghostEffect.enabled = true;
       

        yield return new WaitForSeconds(dashDuration);

        // === 대쉬 종료 ===
        rigid.gravityScale = originalGravity;
        isDashing = false;

        // --- 고스트 이펙트 비활성화 ---
        if (ghostEffect != null) ghostEffect.enabled = false;
      

        // 쿨타임 대기
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            PlayerDeathSound();
            Die();


        }
        else if (collision.gameObject.CompareTag("ClearKey"))
        {
            Destroy(collision.gameObject); // CleayKey 아이템 삭제
            PlayPortalKeySound(); // Potalkey 사운드 재생
        }
    }

    //점프 사운드 함수

    private void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }

    //포탈 사운드 함수.
    private void PlayPortalKeySound()
    {
        if (portalKeySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(portalKeySound);
        }
    }

    //죽음 사운드 함수.

    private void PlayerDeathSound()
    {

        if (Deathsound != null && audioSource != null)
        {
            audioSource.PlayOneShot(Deathsound);
        }

    }


}
