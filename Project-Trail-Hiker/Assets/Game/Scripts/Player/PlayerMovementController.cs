using System.Collections;
using UnityEngine;

namespace Game.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private Rigidbody2D playerRb;
        private PlayerInputManager inputManager;
        private PlayerColliderManager colliderManager;
        private new BoxCollider2D collider;
        private GameManager.GameManager gameManager;
        private ScoreManager scoreManager;

        // Collider Normal offset = 0 e 0.95 size = 0.5 e 1.9

        private Vector2 targetVelocity;

        public int PlayerDirection { private set; get; } = 1;

        private float inputAverageTime = 0f;
        private float maxSpeed = 10f;
        private float movementTimer = -1f;

        public bool isFalling { private set; get; }
        public bool IsFlipping { private set; get; }
        
        [Header("Movimentação")]
        [Range(0f, 1f)]
        [SerializeField] private float accelerationRate;
        [Range(1f, 5f)]
        [SerializeField] private float decelerationForce = 2f;
        [SerializeField] private float decelerationRate;
        [Range(0.1f, 1f)]
        [Tooltip("Seta o time out entre os inputs necessário para o personagem começar a frear")]
        [SerializeField] private float movementTimeOut = 1f;
        [SerializeField] private float maxSpeedTimeOut;
        [SerializeField] private float velocidadeSuperLenta = 0.5f;
        [SerializeField] private float velocidadeLenta = 0.35f;
        [SerializeField] private float velocidadeMédia = 0.22f;
        [SerializeField] private float velocidadeAlta = 0.12f;
        [SerializeField] private float velocidadeSuperAlta = 0.03f;
        [Range(5f, 15f)]
        [SerializeField] private float slopeSpeed;


        [Header("Pulo")]
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool isJumping = false;
        [Range(5f, 20f)]
        [SerializeField] private float jumpForce = 5f;
        [Range(5f, 20f)]
        [SerializeField] private float gravityForce = 5f;
        [SerializeField] private float jumpAbortDecceleration = 4f;

        bool isCrouched;
        Vector2 normalColliderOffset;
        Vector2 normalColliderSize;
        [Header("Agachar")]
        [Range(0f, 1f)]
        [SerializeField] private float crouchResizePercentage = 0.5f;
        [Range(1f, 10f)]
        [SerializeField] private float crouchMovementSpeed;
        
        [Header("Estabilidade do Jogador")]
        public float balanceAmount = 1.0f;
        [Range(0f, 1f)]
        [SerializeField]private float balanceRechargeRate;
        [Range(0f, 1f)]
        [SerializeField]private float unbalancePercentageRate;
        [Range(0.5f, 1f)]
        [SerializeField]private float unbalanceSpeedInfluence;
        
        public float SlopeSpeed => slopeSpeed;
        public float UnbalancePercentageRate => unbalancePercentageRate;
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Start is called before the first frame update
        public void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();
            inputManager = GetComponent<PlayerInputManager>();
            colliderManager = GetComponentInChildren<PlayerColliderManager>();
            collider = GetComponent<BoxCollider2D>();
            gameManager = FindObjectOfType<GameManager.GameManager>();
            scoreManager = FindObjectOfType<ScoreManager>();

            normalColliderOffset = collider.offset;
            normalColliderSize = collider.size;
        
            canJump = true;
        }

        public void Update()
        {
            // Recharhes the balance amount
            if (balanceAmount < 1)
            {
                balanceAmount += balanceRechargeRate * Time.deltaTime;
            }
            else
            {
                balanceAmount = 1;
            }
            // Triggers the fall when balance reaches 0
            if (balanceAmount < 0)
            {
                inputManager.fall = true;
                balanceAmount = 0.1f;
            }
            
            // Flip
            if (inputManager.IsSwipeDirectionButtonDown())
            {
                FlipDirection();
            }

            if (!gameManager.IsGameRunning || IsFlipping)
            {
                return;
            }

            //Cair
            if (inputManager.fall)
            {
                Debug.Log("Cai");
                //TODO: Play animation
                StartCoroutine(Fall());
                inputManager.fall = false;
                scoreManager.fallNumber += 1;
            }

            // Pulo
            canJump = colliderManager.isGrounded && !colliderManager.isMovingUp && !colliderManager.isMovingDown;
            isJumping = !colliderManager.isGrounded;

            if (inputManager.IsJumpButtonDown() && canJump && !colliderManager.needsToJump)
            {
                Jump();
            }
            if(inputManager.IsJumpButtonReleased())
            {
                AbortJump();
            }

            //Agachar
            if (inputManager.IsCrouchButtonDown() && !colliderManager.isMovingDown && !colliderManager.needsToJump)
            {
                Debug.Log("Agachei");
                Crouch();
            }
            if (inputManager.IsCrouchButtonReleased() && !colliderManager.isMovingDown)
            {
                Debug.Log("Desagachei");
                Uncrouch();
            }

            // Andar
            // Calculando a média entre os dois cliques
            if (!isJumping && !colliderManager.isMovingDown && !colliderManager.isMovingUp && !isFalling && !colliderManager.needsToJump)
                inputAverageTime = Mathf.Abs(inputManager.aTime - inputManager.dTime);
            else
                inputAverageTime = 0;

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
            if (!gameManager.IsGameRunning || isFalling || colliderManager.isMovingDown || colliderManager.isMovingUp || colliderManager.needsToJump || IsFlipping)
            {
                return;
            }
            
            if (isJumping)
            {
                ApplyGravity();
            }

            // Move e desacelera o personagem conforme a média de tempo dos inputs
            if ((Time.time - movementTimer <= movementTimeOut))
            {
                // Acelerando
                if (isCrouched) { targetVelocity = new Vector2(crouchMovementSpeed * PlayerDirection, playerRb.velocity.y); }
                else { targetVelocity = new Vector2(maxSpeed * PlayerDirection, playerRb.velocity.y); }

                if (targetVelocity.x > maxSpeed)
                {
                    return;
                }

                playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
            else
            {
                inputManager.aTime = 0;
                inputManager.dTime = 0;
                ApplyDeceleration();
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
                balanceAmount -= unbalanceSpeedInfluence * Time.deltaTime;
            }
            else if(inputAverageTime > 0f)
            {
                StartCoroutine(Fall());
            }
        }

        private void FlipDirection()
        {
            
            playerRb.transform.localScale = new Vector3(-playerRb.transform.localScale.x, playerRb.transform.localScale.y, playerRb.transform.localScale.z);
            PlayerDirection = PlayerDirection == 1 ? -1 : 1;
            StartCoroutine(Flipping());
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

        public void ApplyDeceleration()
        {
            if (Mathf.Approximately(playerRb.velocity.x, 0) || playerRb.velocity.x < 0 * PlayerDirection)
            {
                playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            }
            else if(!isJumping)
            {
                targetVelocity = new Vector2(decelerationForce * maxSpeed * -PlayerDirection, playerRb.velocity.y);
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
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
            
            while(playerRb.velocity.x != 0)
            {
                //targetVelocity = new Vector2(fallSpeedDeceleration * maxSpeed * -PlayerDirection, playerRb.velocity.y);
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
                //balanceAmount += balanceRechargeRate * Time.deltaTime * 1.5f;
                yield return null;
            }
            isFalling = false;

            inputManager.aWasPressed = false;
            inputManager.dWasPressed = false;
            //balanceAmount = 1;

            StopCoroutine(Fall());
        }

        private IEnumerator Flipping()
        {
            IsFlipping = true;
            while(!Mathf.Approximately(playerRb.velocity.x, 0))
            {
                //targetVelocity = new Vector2( maxSpeed * -PlayerDirection, playerRb.velocity.y);
                playerRb.velocity = Vector2.Lerp(playerRb.velocity, Vector2.zero, Time.deltaTime * decelerationRate*2f);
                //balanceAmount += balanceRechargeRate * Time.deltaTime * 1.5f;
                yield return null;
            }
            IsFlipping = false;
            inputManager.aWasPressed = false;
            inputManager.dWasPressed = false;
            //balanceAmount = 1;

            StopCoroutine(Flipping());
        }
    }
}
