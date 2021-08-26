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
        float objColliderWidth = slopeGroundUpward[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGroundUpward[0].GetComponentInChildren<SpriteRenderer>();

        float objAngle = Mathf.Pow(slopeGroundUpward[0].transform.eulerAngles.z, 2)/(slopeGroundUpward[0].transform.eulerAngles.z * 180 / Mathf.PI);

        float distanceX = Mathf.Cos(objAngle) * objColliderWidth;
        float distanceY = Mathf.Sin(objAngle) * objColliderWidth;

        Vector2 newPosition = new Vector2(distanceX,distanceY);

        InstantiateObject(slopeGroundUpward[0], objSpriteRenderer, newPosition, numberOfTimes);
    }

    public void InstantiateSlopeDownward(int numberOfTimes)
    {
        float objColliderWidth = slopeGroundDownward[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGroundDownward[0].GetComponentInChildren<SpriteRenderer>();

        float objAngle = Mathf.Pow(slopeGroundDownward[0].transform.eulerAngles.z, 2) / (slopeGroundDownward[0].transform.eulerAngles.z * 180 / Mathf.PI);


        float distanceX = Mathf.Cos(objAngle) * objColliderWidth;
        float distanceY = Mathf.Sin(objAngle) * objColliderWidth;

        Vector2 newPosition = new Vector2(distanceX, distanceY);

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
