using UnityEngine;
using DG.Tweening;
using System.Collections;


public class P_Locale_2 : MonoBehaviour
{
    //플레이어의 움직임에 따라 움직이는것은 어려워 일단 랜덤하게 이동하는것으로 스크립트 수정.
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        MoveRandomX();       // 좌우 랜덤 이동
        StartCoroutine(JumpRandomLoop()); // 랜덤 간격 점프
        RandomTwitch();      // 가끔 튕기듯 움직이게 설정함
    }

    void MoveRandomX()
    {
        //랜덤 이동 목표위치의 좌표.
        float nextX = Random.Range(-3f, 3f); //유니티의 랜덤함수.(Random.range)

        
        float dir = nextX > transform.position.x ? 1f : -1f;
        transform.localScale = new Vector3(dir * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        transform.DOMoveX(nextX, Random.Range(1.2f, 2.0f))
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                MoveRandomX(); // 반복
            });
    }

    IEnumerator JumpRandomLoop()
    {
        while (true)
        {
            float delay = Random.Range(1f, 4f); //점프의 빈도 랜덤설정.
            float height = Random.Range(1.5f, 3f);//점프의 높이 랜덤설정.

            yield return new WaitForSeconds(delay);

            Sequence jumpSeq = DOTween.Sequence();
            jumpSeq.Append(transform.DOMoveY(transform.position.y + height, 0.25f))
                   .Append(transform.DOMoveY(transform.position.y, 0.25f));
        }
    }

    void RandomTwitch()
    {
        transform.DOScale(originalScale * 1.1f, 0.1f)
                 .SetLoops(2, LoopType.Yoyo)
                 .SetDelay(Random.Range(2f, 5f))
                 .OnComplete(RandomTwitch);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // 
            Destroy(gameObject);

            // 
            Destroy(collision.gameObject);
        }
    }
}
