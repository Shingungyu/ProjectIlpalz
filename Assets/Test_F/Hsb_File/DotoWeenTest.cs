using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotoWeenTest : MonoBehaviour
{

    public float moveDistance = 2f; // 오른쪽으로 이동할 거리
    public float moveTime = 1f;     // 이동하는 데 걸리는 시간
    public int loopCount = -1;      // 왕복 횟수

    private Animator anim;
    private SpriteRenderer sprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

       
    }

    void Start()
    {
        float startX = transform.position.x;

        transform.DOMoveX(startX + moveDistance, moveTime)
            .SetLoops(loopCount, LoopType.Yoyo) // 왕복 loopCount번 반복
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                anim.SetBool("isWalking", true); // 시작할 때 걷기 모션 켜기
            })
            .OnUpdate(() =>
            {
                // 방향 판정 (현재 x - 시작 x)
                float dir = transform.position.x - startX;

                if (dir > 0.01f) // 오른쪽 이동
                    sprite.flipX = false;
                else if (dir < -0.01f) // 왼쪽 이동
                    sprite.flipX = true;
                
            })
            .OnComplete(() =>
            {
                anim.SetBool("isWalking", false); // 끝나면 Idle로 복귀
            });
    }

}
