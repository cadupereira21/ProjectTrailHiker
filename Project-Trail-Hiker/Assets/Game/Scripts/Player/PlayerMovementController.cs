using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Character;

public class PlayerMovementController : MonoBehaviour
{
    Rigidbody2D playerRb;
    PlayerInputManager inputManager;
    PlayerColliderManager colliderManager;
    BoxCollider2D collider;
    GameManager gameManager;
    ScoreManager scoreManager;

    // Collider Normal offset = 0 e 0.95 size = 0.5 e 1.9

    Vector2 targetVelocity;

    public Transform cameraTarget;

    public int playerDirection = 1;

    public float inputAverageTime = 0f;
    private float maxSpeed = 10f;
    float movementTimer = -1f;

    public bool isFalling;

    [Header("Movimentação")]
    [Range(0f, 1f)]
    [SerializeField] float accelerationRate;
    [Range(1f, 5f)]
    [SerializeField] float decelerationForce = 2f;
    [SerializeField] float fallSpeedDeceleration;
    [Range(0.1f, 1f)]
    [Tooltip("Seta o time out entre os inputs necessário para o personagem começar a frear")]
    [SerializeField] float movementTimeOut = 1f;
    [SerializeField] float maxSpeedTimeOut;
    [Range(20f, 60f)]
    [SerializeField] float speedInfluece;
    [SerializeField] float velocidadeSuperLenta = 0.5f;
    [SerializeField] float velocidadeLenta = 0.35f;
    [SerializeField] float velocidadeMédia = 0.22f;
    [SerializeField] float velocidadeAlta = 0.12f;
    [SerializeField] float velocidadeSuperAlta = 0.03f;


    [Header("Pulo")]
    [SerializeField] bool canJump = true;
    [SerializeField] bool isJumping = false;
    [Range(5f, 20f)]
    [SerializeField] float jumpForce = 5f;
    [Range(5f, 20f)]
    [SerializeField] float gravityForce = 5f;
    [SerializeField] float jumpAbortDecceleration = 4f;

    bool isCrouched;
    Vector2 normalColliderOffset;
    Vector2 normalColliderSize;
    [Header("Agachar")]
    [Range(0f, 1f)]
    [SerializeField] float crouchResizePercentage = 0.5f;
    [Range(1f, 10f)]
    [SerializeField] float crouchMovementSpeed;

    //--------------------------------------------------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    public void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        inputManager = GetComponent<PlayerInputManager>();
        colliderManager = GetComponentInChildren<PlayerColliderManager>();
        collider = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

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

        if (!gameManager.IsGameRunning)
        {
            return;
        }

        //Cair
        if (inputManager.fall)
        {
            Debug.Log("Cai");
            scoreManager.fallNumber += 1;
            StartCoroutine(Fall());
            inputManager.fall = false;
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
        if (!gameManager.IsGameRunning)
        {
            return;
        }

        cameraTarget.localPosition = new Vector2(2 + playerRb.velocity.x * speedInfluece * Time.fixedDeltaTime * playerDirection, cameraTarget.localPosition.y); // Seta a posição da camera conforme a velocidade

        if (isJumping)
        {
            ApplyGravity();
        }

        if (isFalling)
        {
            return;
        }

        // Move e desacelera o personagem conforme a média de tempo dos inputs
        if ((Time.time - movementTimer <= movementTimeOut) && !isFalling)
        {
            // Acelerando
            if (isCrouched) { targetVelocity = new Vector2(crouchMovementSpeed * playerDirection, playerRb.velocity.y); }
            else { targetVelocity = new Vector2(maxSpeed * playerDirection, playerRb.velocity.y); }

            if (targetVelocity.x > maxSpeed)
            {
                return;
            }

            playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
        }
        else
        {
            if (Mathf.Approximately(playerRb.velocity.x, 0) || playerRb.velocity.x < 0*playerDirection)
            {
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
                inputManager.aTime = 0f;
                inputManager.dTime = 0f;
            }
            else if(!isJumping)
            {
                // Desacelerando
                targetVelocity = new Vector2(decelerationForce * maxSpeed * -playerDirection, playerRb.velocity.y);
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void NormalizeInputAverageTime()
    {
        if (inputAverageTime > velocidadeSuperLenta) // Super lento
        {
            maxSpeed = 2f;
            inputAverageTime = velocidadeSuperLenta;
        }
        else if (inputAverageTime > velocidadeLenta) // lento
        {
            maxSpeed = 4f;
            inputAverageTime = velocidadeLenta;
        }
        else if (inputAverageTime > velocidadeMédia) // Médio
        {
            maxSpeed = 7f;
            inputAverageTime = velocidadeMédia;
        }
        else if (inputAverageTime > velocidadeAlta) // Rápido
        {
            maxSpeed = 10f;
            inputAverageTime = velocidadeAlta;
        }
        else if (inputAverageTime > velocidadeSuperAlta) // Super Rápido
        {
            maxSpeed = 15f;
            inputAverageTime = velocidadeSuperAlta;
        }
        else if(inputAverageTime > 0f)
        {
            StartCoroutine(Fall());
        }
    }

    private void FlipDirection()
    {
        playerRb.transform.localScale = new Vector3(-playerRb.transform.localScale.x, playerRb.transform.localScale.y, playerRb.transform.localScale.z);
        playerDirection = playerDirection == 1 ? -1 : 1;
        inputManager.aTime = 0f;
        inputManager.dTime = 0f;
        inputManager.aWasPressed = false;
        inputManager.dWasPressed = false;
    }

    private void Jump()
    {
        if (!isCrouched)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            inputManager.aWasPressed = false;
            inputManager.dWasPressed = false;
        }
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
            inputManager.aWasPressed = false;
            inputManager.dWasPressed = false;
        }
    }

    private void Uncrouch()
    {
        collider.offset = normalColliderOffset;
        collider.size = normalColliderSize;
        isCrouched = false;
        canJump = true;
        inputManager.aWasPressed = false;
        inputManager.dWasPressed = false;
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        while(playerRb.velocity != Vector2.zero && playerRb.velocity.x > 0*playerDirection)
        {
            targetVelocity = new Vector2(fallSpeedDeceleration * maxSpeed * -playerDirection, playerRb.velocity.y);
            playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            yield return null;
        }
        isFalling = false;

        inputManager.aWasPressed = false;
        inputManager.dWasPressed = false;

        StopCoroutine(Fall());
    }
}
