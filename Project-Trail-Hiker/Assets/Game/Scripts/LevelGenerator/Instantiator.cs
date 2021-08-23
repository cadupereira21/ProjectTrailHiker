using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] GameObject[] straightGround;
    [SerializeField] GameObject[] slopeGround;
    [SerializeField] int initialLayerOrder;

    Vector2 objectPosition;

    private void Awake()
    {
        objectPosition = transform.position;
    }

    public void InstantiateStraightGround(int numberOfTimes)
    {
        //Vector2 objectPosition = transform.position;
        float objColliderWidth = straightGround[0].GetComponent<BoxCollider2D>().size.x; ;
        SpriteRenderer objSpriteRenderer = straightGround[0].GetComponentInChildren<SpriteRenderer>(); ;
        Vector2 newPosition = new Vector2(objColliderWidth, straightGround[0].gameObject.transform.position.y);

        for (int i = 0; i < numberOfTimes; i++) // Para cada iteração do loop, um prefab clone será instanciado, setado para falso e adicionado à lista
        {
            //TODO: colocar Random do index do array de objetos retos para randomizar os chaozinho quando tivermos mais assets
            Instantiate(straightGround[0], objectPosition, straightGround[0].transform.rotation);

            objSpriteRenderer.sortingOrder--;

            objectPosition += newPosition;
        }
    }

    public void InstantiateSlope(int numberOfTimes)
    {
        //float objColliderWidth = slopeGround[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGround[0].GetComponentInChildren<SpriteRenderer>();
        
        Vector2 newPosition = new Vector2(7.877f,1.389f);

        for (int i = 0; i < numberOfTimes; i++) // Para cada iteração do loop, um prefab clone será instanciado, setado para falso e adicionado à lista
        {
            //TODO: colocar Random do index do array de objetos retos para randomizar os chaozinho quando tivermos mais assets
            Instantiate(slopeGround[0], objectPosition, slopeGround[0].transform.rotation);

            objSpriteRenderer.sortingOrder--;

            objectPosition += newPosition;
        }
    }
}
