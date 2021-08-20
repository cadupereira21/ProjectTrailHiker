using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Character;

public class PlayerMovementController : MonoBehaviour
{
    CharacterMovement2D playerMovement;
    Rigidbody2D playerRb;
    PlayerInputManager inputManager;
    PlayerColliderManager colliderManager;
    BoxCollider2D collider;
    // Collider Normal offset = 0 e 0.95 size = 0.5 e 1.9

    public float maxSpeed = 10f;
    Vector2 targetVelocity;
    int playerDirection = 1;
    public float inputAverageTime = 0f;
    float movementTimer = -1f;
    [Header("Movimentação")]
    [Range(0f, 1f)]
    [SerializeField] float accelerationRate;
    [Range(1f, 5f)]
    [SerializeField] float decelerationForce = 2f;
    [Range(0.1f, 1f)]
    [Tooltip("Seta o time out entre os inputs necessário para o personagem começar a frear")]
    [SerializeField] float movementTimeOut = .9f;

    private Vector2 oldGroundPosition;
    public bool canJump = true;
    public bool isJumping = false;
    [Header("Pulo")]
    [Range(5f, 20f)]
    [SerializeField] float jumpForce = 5f;
    [Range(5f, 20f)]
    [SerializeField] float gravityForce = 5f;
    [SerializeField] float jumpAbortDecceleration = 4f;

    public bool isCrouched;
    Vector2 normalColliderOffset;
    Vector2 normalColliderSize;
    [Header("Agachar")]
    [Range(0f, 1f)]
    [SerializeField] float crouchResizePercentage = 0.5f;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        playerMovement = GetComponent<CharacterMovement2D>();
        playerRb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<PlayerInputManager>();
        colliderManager = GetComponent<PlayerColliderManager>();
        collider = GetComponent<BoxCollider2D>();

        normalColliderOffset = collider.offset;
        normalColliderSize = collider.size;
        
        canJump = true;
    }

    public void Update()
    {
        // Flip
        if (inputManager.IsSwipeDirectionButtonDown())
        {
            FlipDirection();
        }

        // Pulo
        canJump = colliderManager.isGrounded;
        isJumping = !colliderManager.isGrounded;

        if (inputManager.IsJumpButtonDown() && canJump)
        {
            Jump();
        }
        if(inputManager.IsJumpButtonReleased())
        {
            AbortJump();
        }

        //Agachar
        if (inputManager.IsCrouchButtonDown())
        {
            Debug.Log("Agachei");
            Crouch();
        }
        if (inputManager.IsCrouchButtonReleased())
        {
            Debug.Log("Desagachei");
            Uncrouch();
        }

        // Andar
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
        if (isJumping)
        {
            ApplyGravity();
        }

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
            else if(!isJumping)
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
            maxSpeed = 2f;
            inputAverageTime = 0.9f;
        }
        else if (inputAverageTime > .8f)
        {
            maxSpeed = 2f;
            inputAverageTime = 0.8f;
        }
        else if (inputAverageTime > .7f)
        {
            maxSpeed = 3f;
            inputAverageTime = 0.7f;
        }
        else if (inputAverageTime > .6f)
        {
            maxSpeed = 3f;
            inputAverageTime = 0.6f;
        }
        else if (inputAverageTime > .5f)
        {
            maxSpeed = 4f;
            inputAverageTime = 0.5f;
        }
        else if (inputAverageTime > .4f)
        {
            maxSpeed = 4f;
            inputAverageTime = 0.4f;
        }
        else if (inputAverageTime > .3f)
        {
            maxSpeed = 7f;
            inputAverageTime = 0.3f;
        }
        else if (inputAverageTime > .2f)
        {
            maxSpeed = 10f;
            inputAverageTime = 0.2f;
        }
        else if (inputAverageTime > .0f)
        {
            maxSpeed = 15f;
            inputAverageTime = 0.1f;
        }
    }

    private void FlipDirection()
    {
        playerRb.transform.localScale = new Vector3(-playerRb.transform.localScale.x, playerRb.transform.localScale.y, playerRb.transform.localScale.z);
        playerDirection = playerDirection == 1 ? -1 : 1;
    }

    private void Jump()
    {
        if(!isCrouched)
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    public void AbortJump()
    {
        if (isJumping)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Lerp(playerRb.velocity.y, 0, jumpAbortDecceleration*Time.deltaTime));
        }
    }

    private void ApplyGravity()
    {
        playerRb.AddForce(Vector2.up * -gravityForce);
    }

    private void Crouch()
    {
        if (!isJumping)
        {
            collider.size = new Vector2(collider.size.x, collider.size.y * crouchResizePercentage);
            collider.offset = new Vector2(collider.offset.x, collider.offset.y - 0.45f);
            isCrouched = true;
            canJump = false;
        }
    }

    private void Uncrouch()
    {
        collider.offset = normalColliderOffset;
        collider.size = normalColliderSize;
        isCrouched = false;
        canJump = true;
    }
   
}
