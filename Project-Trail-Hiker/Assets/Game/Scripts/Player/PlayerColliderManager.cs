using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollidersTag{
    public const string Ground = "Ground";
}

public class PlayerColliderManager : MonoBehaviour
{
    public LayerMask whatIsGround;

    public bool isGrounded;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(gameObject.transform.position, .25f, whatIsGround);
    }
}
