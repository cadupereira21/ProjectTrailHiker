using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollidersTag{
    public const string Ground = "Ground";
}

public class PlayerColliderManager : MonoBehaviour
{
    public bool isGrounded { private set; get; }

    public void OnCollisionEnter(Collision collision)
    {
        isGrounded = collision.gameObject.CompareTag(CollidersTag.Ground) ? true : false;
    }
}
