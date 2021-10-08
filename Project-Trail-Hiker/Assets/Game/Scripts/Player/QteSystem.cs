using System.Collections;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Player
{
   public class QteSystem : Player
   {
      public PhysicsMaterial2D normalPhysics;
      public PhysicsMaterial2D slopePhysics;
      
      //private Rigidbody2D player;
      //private PlayerColliderManager colliderManager;
      private PlayerMovementController playerMovement;
      //private PlayerInputManager inputManager;
      private Hud hud;
      //private PlayerStateManager playerState;

      private bool isOnObstacleQte = false;
      private void Start()
      {
         //colliderManager = FindObjectOfType<PlayerColliderManager>();
         //player = GetComponent<Rigidbody2D>();
         playerMovement = GetComponent<PlayerMovementController>();
         //inputManager = GetComponent<PlayerInputManager>();
         hud = FindObjectOfType<Hud>();
         //playerState = GetComponent<PlayerStateManager>();
      }

      private void Update()
      {
         if (!StateManager.IsMovingDown && !StateManager.IsMovingUp && !StateManager.NeedsToJump)
         {
            isOnObstacleQte = false;
            StopCoroutine(SlopeQte(true));
            StopCoroutine(SlopeQte(false));
            hud.HideQTEButton("S");
            hud.HideQTEButton("W");
            PlayerRb.sharedMaterial = normalPhysics;
            return;
         }
         
         PlayerRb.sharedMaterial = slopePhysics;

         if (StateManager.IsMovingUp)
         {
            StartCoroutine(SlopeQte(true));
         }

         if (StateManager.IsMovingDown)
         {
            StartCoroutine(SlopeQte(false));
         }

         if (StateManager.NeedsToJump && !isOnObstacleQte)
         {
            isOnObstacleQte = true;
            StartCoroutine(ObstacleQte());
         }

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

      private IEnumerator SlopeQte(bool isMovingUp)
      {
         bool isButtonPressed = false;
         KeyCode buttonNeeded1 = KeyCode.A;
         KeyCode buttonNeeded2 = KeyCode.A;
         var whichButton = "";

         switch (isMovingUp)
         {
            case true:
               whichButton = "W";
               buttonNeeded1 = KeyCode.W;
               buttonNeeded2 = KeyCode.UpArrow;
               break;
            case false:
               whichButton = "S";
               buttonNeeded1 = KeyCode.S;
               buttonNeeded2 = KeyCode.DownArrow;
               break;
         }
         
         hud.ShowQTEButton(whichButton);
         
         if (Input.anyKeyDown)
         {
            isButtonPressed = true;
         }

         if (isButtonPressed && (Input.GetKeyDown(buttonNeeded1) || Input.GetKeyDown(buttonNeeded2)))
         {
            if (playerMovement.balanceAmount > 0 && !StateManager.IsFalling)
            {
               PlayerRb.velocity = new Vector2(playerMovement.SlopeSpeed, 0f);
               playerMovement.balanceAmount -= playerMovement.UnbalancePercentageRate;
               isButtonPressed = false;  
            }
            yield return null;  
         }
         else if (isButtonPressed)
         {
            isButtonPressed = false;
            yield return null;
         }
      }

      private IEnumerator ObstacleQte()
      {
         hud.ShowQTEButton("W");
         yield return new WaitForSeconds(0.12f);
         InputManager.aWasPressed = false;
         InputManager.dWasPressed = false;
         
         bool isButtonPressed = false;
         bool isRightButton = false;

         while (!isButtonPressed)
         {
            if (!StateManager.NeedsToJump)
            {
               isRightButton = false;
               Debug.Log("Voce nao apertou nenhum botao!");
               break;
            }
            if (Input.anyKeyDown)
            {
               isButtonPressed = true;
            }
            
            if (isButtonPressed && InputManager.IsJumpButtonDown())
            {
               Debug.Log("Botao certo!");
               isRightButton = true;
            }
            else if(isButtonPressed)
            {
               Debug.Log("Botao errado!");
               isRightButton = false;
            }

            yield return null;
         }
         
         hud.QteRespondPlayerInput("W", isRightButton);
         switch (isRightButton)
         {
            case true: Debug.Log("Botao Certo!");
               StateManager.SetState("isJumping", true);
               break;
            case false: Debug.Log("Botao Errado!");
               InputManager.fall = true;
               break;
         }

         while (StateManager.NeedsToJump)
         {
            yield return null;
         }
         
         //hud.HideQTEButton("W");
         StopCoroutine(ObstacleQte());
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
