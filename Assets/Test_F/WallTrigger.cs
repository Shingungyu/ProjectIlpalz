using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public PlayerController controller;
    //½Â¹ü commit test
    private void Awake()
    {
        controller = transform.GetComponentInParent<PlayerController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            controller.isColliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            controller.isColliding = false;
        }
    }
}
