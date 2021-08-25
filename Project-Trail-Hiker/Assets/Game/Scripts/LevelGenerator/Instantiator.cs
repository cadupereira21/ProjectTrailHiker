using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField] int initialLayerOrder;

    [SerializeField] GameObject[] straightGround;
    [SerializeField] GameObject[] slopeGroundUpward;
    [SerializeField] GameObject[] slopeGroundDownward;

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

        InstantiateObject(straightGround[0], objSpriteRenderer, newPosition, numberOfTimes);
    }

    public void InstantiateSlopeUpward(int numberOfTimes)
    {
        //float objColliderWidth = slopeGround[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGroundUpward[0].GetComponentInChildren<SpriteRenderer>();

        Vector2 newPosition = new Vector2(7.877f,1.389f);

        InstantiateObject(slopeGroundUpward[0], objSpriteRenderer, newPosition, numberOfTimes);
    }

    public void InstantiateSlopeDownward(int numberOfTimes)
    {
        //float objColliderWidth = slopeGround[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGroundDownward[0].GetComponentInChildren<SpriteRenderer>();

        Vector2 newPosition = new Vector2(7.877f, -1.389f);

        InstantiateObject(slopeGroundDownward[0], objSpriteRenderer, newPosition, numberOfTimes);
    }

    private void InstantiateObject(GameObject obj, SpriteRenderer objSpriteRenderer, Vector2 newPosition, int numberOfTimes)
    {
        for (int i = 0; i < numberOfTimes; i++) // Para cada iteração do loop, um prefab clone será instanciado, setado para falso e adicionado à lista
        {
            //TODO: colocar Random do index do array de objetos retos para randomizar os chaozinho quando tivermos mais assets
            Instantiate(obj, objectPosition, obj.transform.rotation);

            objSpriteRenderer.sortingOrder--;

            objectPosition += newPosition;
        }
    }
}
