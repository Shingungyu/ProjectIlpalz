using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform stageParent;          // 스테이지 배치 위치 (빈 오브젝트)
    public GameObject[] stagePrefabs;      // Stage1, Stage2, Stage3... 순서대로 넣기
    private GameObject currentStage;

    void Update()
    {
        // 숫자 1~9 키로 스테이지 변경 예시
        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadStage(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadStage(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) LoadStage(2);
    }

    void LoadStage(int index)
    {
        // 기존 스테이지 제거
        if (currentStage != null)
            Destroy(currentStage);

        // 새 스테이지 생성
        currentStage = Instantiate(stagePrefabs[index], stageParent);
    }
}
