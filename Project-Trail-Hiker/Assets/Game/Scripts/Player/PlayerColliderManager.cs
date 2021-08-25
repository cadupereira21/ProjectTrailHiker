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

    //Anteriormente tava assim, mas tava quebrando a movimentação quando colocava os assets do gerador kkkkk
    /*public void OnCollisionEnter2D(Collision2D collision)
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
    }*/
}
