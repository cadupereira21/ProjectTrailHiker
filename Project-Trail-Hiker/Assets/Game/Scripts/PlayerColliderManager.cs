﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollidersTag{
    public const string Ground = "Ground";
}

public class PlayerColliderManager : MonoBehaviour
{
    public bool isGrounded;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CollidersTag.Ground))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(CollidersTag.Ground))
        {
            isGrounded = false;
        }
    }
}
