using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //플레이어 어택 스크립트를 분리한이유는 무기가 한 종류가 아니기 때문.


    public GameObject bulletPrefab;     // 총알 프리팹
    public Transform firePoint;         // 총알이 나가는 위치
    public float bulletSpeed = 5f;     // 총알 속도

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
