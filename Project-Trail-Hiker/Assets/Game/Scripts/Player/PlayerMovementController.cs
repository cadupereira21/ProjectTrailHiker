using System.Collections;
using DG.Tweening;
using Game.Scripts.GameManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Player
{
    public class PlayerMovementController : Player
    {
        // private Rigidbody2D playerRb;
        // private PlayerInputManager inputManager;
        // private PlayerColliderManager colliderManager;
        private new BoxCollider2D collider;
        private GameManager.GameManager gameManager;
        private ScoreManager scoreManager;
        // private PlayerStateManager stateManager;

        // Collider Normal offset = 0 e 0.95 size = 0.5 e 1.9

        private Vector2 targetVelocity;

        public int PlayerDirection { private set; get; } = 1;

        public float inputAverageTime = 0f;
        private float maxSpeed = 10f;
        private float movementTimer = -1f;
        private float initialPlayerPositionY = 0.0f;
        private float timeWithoutInput = 0.0f;

        private IEnumerator iCheckingPosition;
        // private IEnumerator iFall;
        // private IEnumerator iFlip;

        //public bool isFalling { private set; get; }
        //public bool IsFlipping { private set; get; }
        
        [Header("Movimentação")]
        [Range(0f, 1f)]
        [SerializeField] private float accelerationRate;
        [Range(1f, 5f)]
        [SerializeField] private float decelerationForce = 2f;
        [FormerlySerializedAs("decelerationRate")] [Range(0.01f, 10.0f)] [SerializeField] private float decelerationAnimTime;
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
        //[SerializeField] private bool canJump = true;
        //[SerializeField] private bool isJumping = false;
        [Range(5f, 20f)]
        [SerializeField] private float jumpForce = 5f;
        [Range(5f, 20f)]
        [SerializeField] private float gravityForce = 5f;
        [SerializeField] private float jumpAbortDecceleration = 4f;

        //bool isCrouched;
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

        // public float VelocidadeLenta => velocidadeLenta;
        //
        // public float VelocidadeMedia => velocidadeMédia;
        //
        // public float VelocidadeSuperAlta => velocidadeSuperAlta;
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------

        // Start is called before the first frame update
        
        public void Start()
        {
            // playerRb = GetComponent<Rigidbody2D>();
            // inputManager = GetComponent<PlayerInputManager>();
            // colliderManager = GetComponentInChildren<PlayerColliderManager>();
            collider = GetComponent<BoxCollider2D>();
            gameManager = FindObjectOfType<GameManager.GameManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            // stateManager = GetComponent<PlayerStateManager>();

            normalColliderOffset = collider.offset;
            normalColliderSize = collider.size;

            initialPlayerPositionY = this.transform.position.y;
            iCheckingPosition = CheckPosition();
            // iFall = Fall();
            //iFlip = Flipping();
            StartCoroutine(iCheckingPosition);
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
                InputManager.fall = true;
                balanceAmount = 0.1f;
            }

            // Flip
            // if (InputManager.IsSwipeDirectionButtonDown())
            // {
            //     FlipDirection();
            // }
            //
            // if (!gameManager.IsGameRunning || StateManager.IsFlipping)
            // {
            //     return;
            // }

            //Cair
            if (InputManager.fall)
            {
                Debug.Log("Cai");
                //TODO: Play animation
                // StartCoroutine(iFall);
                Fall();
                InputManager.fall = false;
                scoreManager.fallNumber += 1;
            }

            /* Pulo
            if (InputManager.IsJumpButtonDown() && StateManager.CanJump && !StateManager.IsOnQte)
            {
                Jump();
            }
            if(InputManager.IsJumpButtonReleased())
            {
                AbortJump();
            }

            //Agachar
            if (InputManager.IsCrouchButtonDown() && !StateManager.IsOnQte)
            {
                Debug.Log("Agachei");
                Crouch();
            }
            if (InputManager.IsCrouchButtonReleased() && !StateManager.IsOnQte)
            {
                Debug.Log("Desagachei");
                Uncrouch();
            }*/

            // Andar
            // Calculando a média entre os dois cliques
            if (!StateManager.IsJumping && !StateManager.IsFalling && !StateManager.IsOnQte && InputManager.walk)
            {
                InputManager.walk = false;
                timeWithoutInput = 0.0f;
                inputAverageTime = Mathf.Abs(InputManager.aTime - InputManager.dTime);   
            }
            else
            {
                inputAverageTime = 0;
                timeWithoutInput += Time.deltaTime;
                if (timeWithoutInput > 1.5f)
                {
                    timeWithoutInput = 0.0f;
                    Debug.Log("Demorou para andar");
                    InputManager.aWasPressed = false;
                    InputManager.dWasPressed = false;
                }
            }

            // Normaliza o valor da media do input e seta a velocidade máxima para cada media
            NormalizeInputAverageTime();

            // Reseta os valores de input
            if (InputManager.aWasPressed && InputManager.dWasPressed)
            {
                movementTimer = Time.time;
                InputManager.aWasPressed = false;
                InputManager.dWasPressed = false;
            }
        }

        public void FixedUpdate()
        {
            if (!gameManager.IsGameRunning || StateManager.IsFalling || StateManager.IsOnQte || StateManager.IsFlipping)
            {
                return;
            }
            
            if (StateManager.IsJumping)
            {
                ApplyGravity();
            }

            // Move e desacelera o personagem conforme a média de tempo dos inputs
            if ((Time.time - movementTimer <= movementTimeOut))
            {
                // Acelerando
                targetVelocity = new Vector2(maxSpeed * PlayerDirection, PlayerRb.velocity.y);

                if (targetVelocity.x > maxSpeed)
                {
                    return;
                }

                PlayerRb.velocity = Vector2.Lerp(PlayerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
            else
            {
                InputManager.aTime = 0;
                InputManager.dTime = 0;
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
                balanceAmount -= balanceRechargeRate/5;
            }
            else if (inputAverageTime > velocidadeSuperAlta) // Super Rápido
            {
                maxSpeed = 15f;
                inputAverageTime = velocidadeSuperAlta;
                balanceAmount -= unbalancePercentageRate;
            }
            else if(inputAverageTime > 0f)
            {
                Fall();
            }
        }

        // private void FlipDirection()
        // {
        //     PlayerRb.transform.localScale = new Vector3(-PlayerRb.transform.localScale.x, PlayerRb.transform.localScale.y, PlayerRb.transform.localScale.z);
        //     PlayerDirection = PlayerDirection == 1 ? -1 : 1;
        //     StartCoroutine(iFlip);
        //     InputManager.aTime = 0f;
        //     InputManager.dTime = 0f;
        //     InputManager.aWasPressed = false;
        //     InputManager.dWasPressed = false;
        // }

        // private void Jump()
        // {
        //     if (!StateManager.IsCrouched)
        //     {
        //         PlayerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //         InputManager.aWasPressed = false;
        //         InputManager.dWasPressed = false;
        //     }
        // }

        public void AbortJump()
        {
            if (StateManager.IsJumping)
            {
                PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, Mathf.Lerp(PlayerRb.velocity.y, 0, jumpAbortDecceleration*Time.deltaTime));
            }
        }

        private void ApplyGravity()
        {
            PlayerRb.AddForce(Vector2.up * -gravityForce);
        }

        public void ApplyDeceleration()
        {
            if (Mathf.Approximately(PlayerRb.velocity.x, 0) || PlayerRb.velocity.x < 0 * PlayerDirection)
            {
                PlayerRb.velocity = new Vector2(0, PlayerRb.velocity.y);
            }
            else if(!StateManager.IsJumping)
            {
                targetVelocity = new Vector2(decelerationForce * maxSpeed * -PlayerDirection, PlayerRb.velocity.y);
                PlayerRb.velocity = Vector2.Lerp(PlayerRb.velocity, targetVelocity, Time.deltaTime * accelerationRate);
            }
        }

        // private void Crouch()
        // {
        //     if (!StateManager.IsJumping)
        //     {
        //         collider.size = new Vector2(collider.size.x, collider.size.y * crouchResizePercentage);
        //         collider.offset = new Vector2(collider.offset.x, collider.offset.y - 0.45f);
        //         StateManager.SetState("isCrouched", true);
        //         //canJump = false;
        //         InputManager.aWasPressed = false;
        //         InputManager.dWasPressed = false;
        //     }
        // }

        // private void Uncrouch()
        // {
        //     collider.offset = normalColliderOffset;
        //     collider.size = normalColliderSize;
        //     StateManager.SetState("isCrouched", false);
        //     //canJump = true;
        //     InputManager.aWasPressed = false;
        //     InputManager.dWasPressed = false;
        // }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        // private IEnumerator Fall()
        // {
        //     Debug.Log("Entrei na corrotina Fall()");
        //     InputManager.fall = false;
        //     StateManager.SetState("isFalling", true);
        //     
        //     // while(PlayerRb.velocity.x != 0)
        //     // {
        //     //     //targetVelocity = new Vector2(fallSpeedDeceleration * maxSpeed * -PlayerDirection, playerRb.velocity.y);
        //     //     PlayerRb.velocity = Vector2.Lerp(PlayerRb.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
        //     //     //balanceAmount += balanceRechargeRate * Time.deltaTime * 1.5f;
        //     //     yield return null;
        //     // }
        //     DOTween.To(()=> PlayerRb.velocity, x=> PlayerRb.velocity = x, Vector2.zero, decelerationAnimTime);
        //     //StateManager.SetState("isFalling", false);
        //
        //     InputManager.aWasPressed = false;
        //     InputManager.dWasPressed = false;
        //     //StateManager.ResetStates();
        //     //balanceAmount = 1;
        //     yield break;
        // }
        
        private void Fall()
        {
            InputManager.fall = false;
            StateManager.SetState("isFalling", true);
            
            // while(PlayerRb.velocity.x != 0)
            // {
            //     //targetVelocity = new Vector2(fallSpeedDeceleration * maxSpeed * -PlayerDirection, playerRb.velocity.y);
            //     PlayerRb.velocity = Vector2.Lerp(PlayerRb.velocity, Vector2.zero, Time.deltaTime * decelerationRate);
            //     //balanceAmount += balanceRechargeRate * Time.deltaTime * 1.5f;
            //     yield return null;
            // }
            DOTween.To(()=> PlayerRb.velocity, x=> PlayerRb.velocity = x, Vector2.zero, decelerationAnimTime);
            //StateManager.SetState("isFalling", false);

            InputManager.aWasPressed = false;
            InputManager.dWasPressed = false;
            //StateManager.ResetStates();
            //balanceAmount = 1;
        }

        // private IEnumerator Flipping()
        // {
        //     StateManager.SetState("isFlipping", true);
        //
        //     while(!Mathf.Approximately(PlayerRb.velocity.x, 0.0f))
        //     {
        //         //targetVelocity = new Vector2( maxSpeed * -PlayerDirection, playerRb.velocity.y);
        //         PlayerRb.velocity = Vector2.Lerp(PlayerRb.velocity, Vector2.zero, Time.deltaTime * decelerationRate*2f);
        //         //balanceAmount += balanceRechargeRate * Time.deltaTime * 1.5f;
        //         yield return null;
        //     }
        //     StateManager.SetState("isFlipping", false);
        //     InputManager.aWasPressed = false;
        //     InputManager.dWasPressed = false;
        //     StateManager.ResetStates();
        //     //balanceAmount = 1;
        //
        //     yield break;
        // }

        private IEnumerator CheckPosition()
        {
            //int aux = 0;
            while (true)
            {
                // if (!StateManager.IsGrounded)
                // {
                //     aux++;
                //     if (aux > 5)
                //     {
                //         gameObject.transform.position = new Vector3(gameObject.transform.position.x, initialPlayerPositionY+1, gameObject.transform.position.z);
                //         aux = 0;
                //         yield return new WaitForSeconds(2.0f);
                //     }
                //     else
                //     {
                //         yield return new WaitForSeconds(0.2f);   
                //     }
                // }
                // else
                // {
                //     aux = 0;
                //     yield return new WaitForSeconds(2.0f);
                // }
                if (PlayerRb.velocity.y < -20.0f)
                {
                    PlayerRb.velocity = Vector2.zero;
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, initialPlayerPositionY + 1, gameObject.transform.position.z);   
                }

                yield return new WaitForSeconds(1.0f);
            }
        }
    }
}
