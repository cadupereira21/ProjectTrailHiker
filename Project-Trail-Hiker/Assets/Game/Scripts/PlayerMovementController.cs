using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D playerRb;
    PlayerInputManager inputManager;
    PlayerColliderManager colliderManager;
    // Collider Normal offset = 0 e 0.95 size = 0.5 e 1.9

    [Header("Movimentação")]
    float maxSpeed = 10f;
    Vector2 targetVelocity;
    int playerDirection = 1;
    float inputAverageTime = 0f;
    float movementTimer = -1f;

    [Range(0f, 1f)]
    [SerializeField] float accelerationRate;

    [Range(1f, 5f)]
    [SerializeField] float decelerationForce = 2f;

    [Range(0.1f, 1f)]
    [Tooltip("Seta o time out entre os inputs necessário para o personagem começar a frear")]
    [SerializeField] float movementTimeOut = .9f;

    [Header("Pulo")]
    private bool canJump = true;
    private bool isJumping = false;

    [Range(0f, 1f)]
    [SerializeField] float jumpHeight = 5f;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<PlayerInputManager>();
        colliderManager = GetComponent<PlayerColliderManager>();
    }

    public void Update()
    {
        Debug.Log(colliderManager.isGrounded);

        if (inputManager.IsSwipeDirectionButtonDown())
        {
            FlipDirection();
        }

        if (inputManager.IsJumpButtonHold())
        {
            Jump();
        }

        // Calculando a média entre os dois cliques
        inputAverageTime = Mathf.Abs(inputManager.aTime - inputManager.dTime);

        // Normaliza o valor da media do input e seta a velocidade máxima para cada media
        NormalizeInputAverageTime();

        // Reseta os valores de input
        if (inputManager.aWasPressed && inputManager.dWasPressed)
        {
            movementTimer = Time.time;
            inputManager.aWasPressed = false;
            inputManager.dWasPressed = false;
        }
    }

    public void FixedUpdate()
    {
        // Move e desacelera o personagem conforme a média de tempo dos inputs
        if (Time.time - movementTimer < movementTimeOut)
        {
            // Acelerando
            targetVelocity = new Vector2(maxSpeed * playerDirection, playerRb.velocity.y);
            if (targetVelocity.x > maxSpeed)
            {
                targetVelocity.x = maxSpeed;
            }
            playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
        }
        else
        {
            if (Mathf.Approximately(playerRb.velocity.x, 0) || playerRb.velocity.x < 0)
            {
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            }
            else
            {
                // Desacelerando
                targetVelocity = new Vector2( decelerationForce*maxSpeed * -playerDirection, playerRb.velocity.y);
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void NormalizeInputAverageTime()
    {
        if (inputAverageTime > 1f)
        {
            maxSpeed = 1f;
            inputAverageTime = 1f;
        }
        else if (inputAverageTime > .9f)
        {
            maxSpeed = 3f;
            inputAverageTime = 0.9f;
        }
        else if (inputAverageTime > .8f)
        {
            maxSpeed = 3f;
            inputAverageTime = 0.8f;
        }
        else if (inputAverageTime > .7f)
        {
            maxSpeed = 5f;
            inputAverageTime = 0.7f;
        }
        else if (inputAverageTime > .6f)
        {
            maxSpeed = 5f;
            inputAverageTime = 0.6f;
        }
        else if (inputAverageTime > .5f)
        {
            maxSpeed = 8f;
            inputAverageTime = 0.5f;
        }
        else if (inputAverageTime > .4f)
        {
            maxSpeed = 8f;
            inputAverageTime = 0.4f;
        }
        else if (inputAverageTime > .2f)
        {
            maxSpeed = 12f;
            inputAverageTime = 0.2f;
        }
        else if (inputAverageTime > .1f)
        {
            maxSpeed = 12f;
            inputAverageTime = 0.1f;
        }
        else if (inputAverageTime > 0f)
        {
            maxSpeed = 15f;
            inputAverageTime = 0.05f;
        }
    }

    private void FlipDirection()
    {
        playerRb.transform.localScale = new Vector3(-playerRb.transform.localScale.x, playerRb.transform.localScale.y, playerRb.transform.localScale.z);
        playerDirection = playerDirection == 1 ? -1 : 1;
    }

    private void Jump()
    {
        var targetHeight = new Vector2(playerRb.transform.position.x, playerRb.transform.position.y + jumpHeight*Time.fixedDeltaTime);
        playerRb.transform.position = Vector2.Lerp(playerRb.transform.position, targetHeight, 1);
    }

   
}
