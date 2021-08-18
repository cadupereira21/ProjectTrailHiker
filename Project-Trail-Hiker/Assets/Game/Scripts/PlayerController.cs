using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D playerRb;
    PlayerInputManager inputManager;

    [SerializeField]
    float speed = 0f;
    [SerializeField]
    float maxSpeed = 25f;
    [SerializeField]
    float decelerationVelocity = 5f;

    int playerDirection = 1;

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    public void Update()
    {

    }

     public void FixedUpdate()
     {
        if (inputManager.wantsToMove)
        {
            Move();
        } else
        {
            Stop();
        }
     }

//--------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Move()
    {
        if(inputManager.walkInputAverageTime == 0 || playerRb.velocity.x > maxSpeed)
        {
            return;
        }
        playerRb.velocity = new Vector3(Time.fixedDeltaTime * playerDirection * speed / inputManager.walkInputAverageTime, playerRb.velocity.y, 0);
    }

    public void Stop()
    {
        //float newXVelocity = Mathf.Lerp(0, playerRb.velocity.x, decelerationRate);
        //playerRb.velocity = new Vector3(newXVelocity, playerRb.velocity.y, 0);
        if(playerRb.velocity.x > 1)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x + (-playerDirection * decelerationVelocity), playerRb.velocity.y, 0);
        }
    }

     public void ChangePlayerDirection()
     {
         if(playerDirection == 1)
         {
             playerDirection = -1;
         } else
         {
             playerDirection = 1;
         }
     }
}
