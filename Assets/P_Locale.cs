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

        transform.DOMoveX(4f, 10f) //이동속도,거리
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
    }
}