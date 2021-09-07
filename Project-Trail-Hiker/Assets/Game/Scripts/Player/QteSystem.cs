using System.Collections;
using UnityEngine;

namespace Game.Scripts.Player
{
   public class QteSystem : MonoBehaviour
   {
      private Rigidbody2D player;
      private PlayerColliderManager colliderManager;
      private PlayerMovementController playerMovement;
      private PlayerInputManager inputManager;

      private bool isOnObstacleQte = false;
      private void Start()
      {
         colliderManager = FindObjectOfType<PlayerColliderManager>();
         player = GetComponent<Rigidbody2D>();
         playerMovement = GetComponent<PlayerMovementController>();
         inputManager = GetComponent<PlayerInputManager>();
      }

      private void Update()
      {
         if (!colliderManager.isMovingDown && !colliderManager.isMovingUp && !colliderManager.needsToJump)
         {
            isOnObstacleQte = false;
            StopCoroutine(SlopeQte(true));
            StopCoroutine(SlopeQte(false));
            return;
         }

         if (colliderManager.isMovingUp)
         {
            StartCoroutine(SlopeQte(true));
         }

         if (colliderManager.isMovingDown)
         {
            StartCoroutine(SlopeQte(false));
         }

         if (colliderManager.needsToJump && !isOnObstacleQte)
         {
            isOnObstacleQte = true;
            StartCoroutine(ObstacleQte());
         }

         if (player.velocity.x < 0)
         {
            player.velocity = Vector2.zero;
         }
         else if(!colliderManager.needsToJump)
         {
            playerMovement.ApplyDeceleration();
         }
      }

      private IEnumerator SlopeQte(bool isMovingUp)
      {
         bool isButtonPressed = false;
         KeyCode buttonNeeded1 = KeyCode.A;
         KeyCode buttonNeeded2 = KeyCode.A;

         switch (isMovingUp)
         {
            case true:
               buttonNeeded1 = KeyCode.W;
               buttonNeeded2 = KeyCode.UpArrow;
               break;
            case false:
               buttonNeeded1 = KeyCode.S;
               buttonNeeded2 = KeyCode.DownArrow;
               break;
         }

         if (Input.anyKeyDown)
         {
            isButtonPressed = true;
         }

         if (isButtonPressed && (Input.GetKeyDown(buttonNeeded1) || Input.GetKeyDown(buttonNeeded2)))
         {
            if (playerMovement.balanceAmount > 0 && !playerMovement.isFalling)
            {
               player.velocity = new Vector2(playerMovement.SlopeSpeed, 0f);
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
         inputManager.aWasPressed = false;
         inputManager.dWasPressed = false;
         
         bool isButtonPressed = false;
         bool isRightButton = false;

         while (!isButtonPressed)
         {
            if (Input.anyKeyDown)
            {
               isButtonPressed = true;
            }
            
            if (isButtonPressed && inputManager.IsJumpButtonDown())
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

         while (colliderManager.needsToJump)
         {
            yield return null;
         }
         
         switch (isRightButton)
         {
            case true: Debug.Log("Botao Certo!");
               break;
            case false: Debug.Log("Botao Errado!");
               inputManager.fall = true;
               break;
         }
      }
   }
}
