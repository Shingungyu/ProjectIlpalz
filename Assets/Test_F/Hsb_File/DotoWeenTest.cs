using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotoWeenTest : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveTime = 1f;
    public int loopCount = -1;

    private Animator anim;
    private SpriteRenderer sprite;
    private bool goingRight = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        float startX = transform.position.x;

        anim.SetBool("isWalking", true); // 걷기 모션 켜기

        transform.DOMoveX(startX + moveDistance, moveTime)
            .SetLoops(loopCount, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .OnStepComplete(() =>
            {
                // Yoyo에서 한 사이클(오른쪽 → 왼쪽) 끝날 때마다 방향 전환
                goingRight = !goingRight;
                sprite.flipX = !goingRight; // 오른쪽일 때 false, 왼쪽일 때 true
            });
    }

}
