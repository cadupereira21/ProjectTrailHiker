using System.Collections;
using DG.Tweening;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Player
{
   public class QteSystem : Player
   {
      // public PhysicsMaterial2D normalPhysics;
      // public PhysicsMaterial2D slopePhysics;
      
      //private Rigidbody2D player;
      //private PlayerColliderManager colliderManager;
      private PlayerMovementController playerMovement;
      //private PlayerInputManager inputManager;
      private Hud hud;
      //private PlayerStateManager playerState;

      [SerializeField] private Rigidbody2D playerRb;

      private bool isOnObstacleQte = false;

      private bool hasEnteredQte = false;
      private void Start()
      {
         //colliderManager = FindObjectOfType<PlayerColliderManager>();
         //player = GetComponent<Rigidbody2D>();
         playerMovement = GetComponent<PlayerMovementController>();
         //inputManager = GetComponent<PlayerInputManager>();
         hud = FindObjectOfType<Hud>();
         playerRb = GetComponent<Rigidbody2D>();
         StartCoroutine(ObstacleQte());
         //playerState = GetComponent<PlayerStateManager>();
      }

      private void Update()
      {
         /*
          * 1) Checar o collider com o ObstacleQTETrigger
          * 2) Caso verdadeiro: Iniciar a corrotina para pegar o input do player
          * 3) Caso falso: Terminar a corrotina
          * 4) Checar o collider com o objeto
          * 5) Caso verdadeiro: Checar o isButtonRight
          *    5.1) Caso verdadeiro: Pular
          *    5.2) Caso falso: Cair
          *
          * CORROTINA:
          *    a) isButtonRight = false
          *    a) Setar a velocidade do player para uma possível de pular o objeto
          *    b) Enquanto estiver dentro do collider
          *       b.1) Checar se algum botão igual a w ou s foi pressionado
          *          b.1.1) Caso sim: isButtonRight = true + hud.QteRespondPlayerInput("W", isRightButton)
          *          b.1.2) Caso não: isButtonRight = false
          *       b.2) yield break;
          */
         if (!StateManager.IsMovingDown && !StateManager.IsMovingUp && !StateManager.NeedsToJump)
         {
            // StopCoroutine(SlopeQte(true));
            // StopCoroutine(SlopeQte(false));
            // hud.HideQTEButton("S");
            // hud.HideQTEButton("W");
            // PlayerRb.sharedMaterial = normalPhysics;
            return;
         }

         // PlayerRb.sharedMaterial = slopePhysics;
         //
         // if (StateManager.IsMovingUp)
         // {
         //    StartCoroutine(SlopeQte(true));
         // }
         //
         // if (StateManager.IsMovingDown)
         // {
         //    StartCoroutine(SlopeQte(false));
         // }

         if (PlayerRb.velocity.x < 0)
         {
            PlayerRb.velocity = Vector2.zero;
         }
         else if(!StateManager.NeedsToJump)
         {
            playerMovement.ApplyDeceleration();
         }

         if (StateManager.NeedsToJump && Mathf.Approximately(PlayerRb.velocity.x, 0))
         {
            StartCoroutine(GetOutOfObstacleQte());
         }
      }

      // private IEnumerator SlopeQte(bool isMovingUp)
      // {
      //    bool isButtonPressed = false;
      //    KeyCode buttonNeeded1 = KeyCode.A;
      //    KeyCode buttonNeeded2 = KeyCode.A;
      //    var whichButton = "";
      //
      //    switch (isMovingUp)
      //    {
      //       case true:
      //          whichButton = "W";
      //          buttonNeeded1 = KeyCode.W;
      //          buttonNeeded2 = KeyCode.UpArrow;
      //          break;
      //       case false:
      //          whichButton = "S";
      //          buttonNeeded1 = KeyCode.S;
      //          buttonNeeded2 = KeyCode.DownArrow;
      //          break;
      //    }
      //    
      //    hud.ShowQTEButton(whichButton);
      //    
      //    if (Input.anyKeyDown)
      //    {
      //       isButtonPressed = true;
      //    }
      //
      //    if (isButtonPressed && (Input.GetKeyDown(buttonNeeded1) || Input.GetKeyDown(buttonNeeded2)))
      //    {
      //       if (playerMovement.balanceAmount > 0 && !StateManager.IsFalling)
      //       {
      //          PlayerRb.velocity = new Vector2(playerMovement.SlopeSpeed, 0f);
      //          playerMovement.balanceAmount -= playerMovement.UnbalancePercentageRate;
      //          isButtonPressed = false;  
      //       }
      //       yield return null;  
      //    }
      //    else if (isButtonPressed)
      //    {
      //       isButtonPressed = false;
      //       yield return null;
      //    }
      // }

      /* CORROTINA:
      *    a) isButtonRight = false
      *    a) Setar a velocidade do player para uma possível de pular o objeto
      *    b) Enquanto estiver dentro do collider
         *       b.1) Checar se algum botão igual a w ou s foi pressionado
         *          b.1.1) Caso sim: isButtonRight = true + hud.QteRespondPlayerInput("W", isRightButton)
      *          b.1.2) Caso não: isButtonRight = false
      *       b.2) yield break;
      */
      // private IEnumerator ObstacleQte()
      // {
      //    hud.ShowQTEButton("W");
      //    isButtonRight = false;
      //    var isButtonPressed = false;
      //    var oldVelocity = playerRb.velocity;
      //
      //    DOTween.To(()=> playerRb.velocity, x=> playerRb.velocity = x, new Vector2(9f, playerRb.velocity.y), 0.7f);
      //
      //    while (StateManager.NeedsToJump || !isButtonPressed)
      //    {
      //       if (Input.anyKeyDown)
      //       {
      //          var buttonPressed = Input.inputString;
      //          switch (buttonPressed)
      //          {
      //             case "w":
      //                isButtonPressed = true;
      //                isButtonRight = true;
      //                break;
      //             case "s":
      //                isButtonPressed = true;
      //                isButtonRight = false;
      //                break;
      //             default:
      //                buttonPressed = "";
      //                break;
      //          }
      //       }
      //       yield return null;
      //    }
      //
      //    isOnObstacleQte = false;
      //    hasExitCorroutine = true;
      //    //DOTween.To(()=> playerRb.velocity, x=> playerRb.velocity = x, oldVelocity, 0.5f);
      // }
      
      private IEnumerator ObstacleQte()
      {
         var isButtonRight = false;
         var isButtonPressed = false;
         while (true)
         {
            var oldVelocity = playerRb.velocity;

            while (StateManager.NeedsToJump)
            {
               hasEnteredQte = true;
               hud.ShowQTEButton("W");
               DOTween.To(()=> playerRb.velocity, x=> playerRb.velocity = x, new Vector2(8.5f, playerRb.velocity.y), 1.0f);
            
               switch (isButtonPressed)
               {
                  case true:
                     hud.QteRespondPlayerInput("W", isButtonRight);
                     break;
                  default: if(Input.anyKeyDown)
                  {
                     var buttonPressed = Input.inputString;
                     switch (buttonPressed)
                     {
                        case "a":
                        case "d":
                           buttonPressed = "";
                           break;
                        case "w":
                           isButtonPressed = true;
                           isButtonRight = true;
                           break;
                        default:
                           isButtonPressed = true;
                           isButtonRight = false;
                           break;
                     }
                  } break;
               }
               yield return null;
            }

            if (hasEnteredQte)
            {
               if (!isButtonRight)
                  yield return new WaitForSeconds(0.12f);
               
               AnswerObstacleQte(isButtonRight);
               //DOTween.To(()=> playerRb.velocity, x=> playerRb.velocity = x, oldVelocity, 1.0f);
               isButtonPressed = false;
               isButtonRight = false;
               hasEnteredQte = false;
               InputManager.aWasPressed = false;
               InputManager.dWasPressed = false;
            }
         
            hud.HideQTEButton("W");
            yield return null;
         }
      }

      private void AnswerObstacleQte(bool isButtonRight)
      {
         hud.HideQTEButton("W");
         if (isButtonRight)
         {
            //pular
            StateManager.SetState("isJumping", true);
         }
         else
         {
            InputManager.fall = true;  
         }
      }

      private IEnumerator GetOutOfObstacleQte()
      {
         while (StateManager.NeedsToJump)
         {
            PlayerRb.velocity += Vector2.right.normalized*Time.deltaTime*15;
            yield return null;
         }
         StopCoroutine(GetOutOfObstacleQte());
      }
   }
}
