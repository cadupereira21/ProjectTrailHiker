using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] GameObject[] straightGround;

    public void InstantiateStraightGround(int numberOfTimes)
    {
        Vector3 objectPosition = transform.position;
        float objColliderWidth = straightGround[0].GetComponent<BoxCollider2D>().size.x; ;
        SpriteRenderer objSpriteRenderer = straightGround[0].GetComponentInChildren<SpriteRenderer>(); ;
        Vector3 newPosition;

        for (int i = 0; i < numberOfTimes; i++) // Para cada iteração do loop, um prefab clone será instanciado, setado para falso e adicionado à lista
        {
            //TODO: colocar Random do index do array de objetos retos para randomizar os chaozinho quando tivermos mais assets
            Instantiate(straightGround[0], objectPosition, Quaternion.identity);

            newPosition = new Vector3(objColliderWidth, straightGround[0].gameObject.transform.position.y, straightGround[0].gameObject.transform.position.y);
            objSpriteRenderer.sortingOrder--;

            objectPosition += newPosition;
        }
    }
}
