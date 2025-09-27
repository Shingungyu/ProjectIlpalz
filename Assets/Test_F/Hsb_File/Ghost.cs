using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    //고스트 생성 간격.
    public float ghostDelay;

    //다음 고스트를 생성하기까지 남은 시간
    private float ghostDelayTime;

    //고스트 프리펩
    public GameObject ghost;

    public bool makeGhost; // 고스트를 생성할지 말지 정하기.

    void Start()
    {
        // 게임 시작 시 고스트 생성 대기 시간을 초기화
        this.ghostDelayTime = this.ghostDelay;
    }


    //일정한 간격으로 호출되는 Update()와는 달리 0.02sec(기본값)마다 호출됨.
    void FixedUpdate()
    {
        //현재 고스트가 있다면
        if (this.makeGhost)
        {
            
            //현재 고스트의 딜레이가 0보다 크다면
            if (this.ghostDelayTime > 0)
            {
                //고스트딜레이 = 고스트딜레이-흐르는 시간
                this.ghostDelayTime -= Time.deltaTime;
            }
            else
            {
                //Instantiate 함수= 오브젝트를 게임 실행중 복제 및 생성시키는 함수 

                //게임오브젝트 변수에 담아 게임 오브젝트 변수에 담긴 고스트 프리펩을 현재 위치, 현재 회전위치에 맞게 "복제"하여 현재 고스트에 담는다.
                GameObject currentGhost = Instantiate(this.ghost, this.transform.position, this.transform.rotation);

                //sprite 타입 변수 현재 스프라이트= 현재 스프라이트의 값을 가져와 현재 스프라이트 라는 변수(currentSprite 변수에 담는다.)
                Sprite currentSprite = this.GetComponent<SpriteRenderer>().sprite;

                //(this.transform.localScale; 현재 스크립트가 붙어있는 오브젝트의 크기를 뜻함)
                //즉 현재 고스트의 크기를 현재 오브젝트와 동일하게 맞춘다는 뜻.
                // * .localScale =부모 오브젝트를 기준으로 한 크기(벡터3) x,y,z 비율로 저장됨.*
                currentGhost.transform.localScale = this.transform.localScale;
                

                //currentGhost의 스프라이트 값은 = currentSprite값이다.
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;


                // 다음 고스트 생성을 위해 대기 시간을 초기화
                this.ghostDelayTime = this.ghostDelay;


                //1초뒤 잔상을 사라지게 만듦.
                Destroy(currentGhost, 1f);
            }
        }
    }
}
