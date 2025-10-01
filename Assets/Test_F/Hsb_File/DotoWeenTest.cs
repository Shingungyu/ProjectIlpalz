using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotoWeenTest : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
        transform.DOMove(new Vector2(5, 0),3);
    }
}
