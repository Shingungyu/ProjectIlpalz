using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_Locale : MonoBehaviour
{


    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        transform.DOMoveX(7f, 2f) //절대좌표,해당좌표에 도달하기까지 걸리는시간 순서 //
            .SetLoops(-1, LoopType.Yoyo)//-1은 반복시키기, 값이 아닌 yoyo는 무한루프
            .OnStepComplete(() =>
            {
                // Yoyo 한 사이클 끝날 때마다 좌우 반전(몬스터 위치전환)
                transform.localScale = new Vector3(
                    -transform.localScale.x,
                    transform.localScale.y,
                    transform.localScale.z

                    
                );
            });

        JumpLoop();
    }
    void JumpLoop()
    {
        Sequence jumpSeq = DOTween.Sequence();

        jumpSeq.Append(transform.DOMoveY(transform.position.y + 2f, 0.3f)) // 위로
                .Append(transform.DOMoveY(transform.position.y, 0.3f))     // 아래로
                .SetDelay(3f)
                .SetLoops(-1);
    }

}