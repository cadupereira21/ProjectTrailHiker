using System.Collections;
using UnityEngine;

namespace Game.Scripts.Player
{
   public class QteSystem : MonoBehaviour
   {
      private Rigidbody2D player;
      private PlayerColliderManager colliderManager;
      private PlayerMovementController playerMovement;

      //private float timeThreshold = 0;

      private void Start()
      {
         colliderManager = FindObjectOfType<PlayerColliderManager>();
         player = GetComponent<Rigidbody2D>();
         playerMovement = GetComponent<PlayerMovementController>();
      }

      private void Update()
      {
         //timeThreshold = Time.deltaTime;

         if (!colliderManager.isMovingDown && !colliderManager.isMovingUp)
         {
            StopAllCoroutines();
            return;
         }

         if (colliderManager.isMovingUp)
         {
            StartCoroutine(ProcessQte(true));
         }

         if (colliderManager.isMovingDown)
         {
            StartCoroutine(ProcessQte(false));
         }

         if (player.velocity.x < 0)
         {
            player.velocity = Vector2.zero;
         }
         else
         {
            playerMovement.ApplyDeceleration();
         }
      }

      private IEnumerator ProcessQte(bool isMovingUp)
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
            player.velocity = new Vector2(playerMovement.SlopeSpeed, 0f);
            playerMovement.balanceAmount -= playerMovement.UnbalancePercentageRate;
            isButtonPressed = false;
            yield return null;  
         }
         else if (isButtonPressed)
         {
            isButtonPressed = false;
            yield return null;
         }
      }
   }
}
