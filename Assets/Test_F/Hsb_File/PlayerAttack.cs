using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //플레이어 어택 스크립트를 분리한이유는 무기가 한 종류가 아니기 때문. 이 스크립트에 칼, 스킬 등 추가하면 될듯함


    public GameObject bulletPrefab;   
    public Transform firePoint;     
    public float bulletSpeed = 15f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Shoot();
            
        }
    }

    void Shoot()
    {
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Z축 0으로 고정

        // firePoint  mouse 방향 
        Vector2 direction = (mousePos - firePoint.position).normalized;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 총알 Rigidbody2D 이용해서 이동 방향 설정
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
