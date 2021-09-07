using UnityEngine;
using UnityEngine.Serialization;

public class Instantiator : MonoBehaviour
{
    [FormerlySerializedAs("ObstacleGenerationPercentage")]
    [Tooltip("The percentage of time in which an obstacle will be instantiated with a ground object")]
    [SerializeField] private int obstacleGenerationPercentage;
    [SerializeField] private int initialLayerOrder;

    bool obsHasBeenInstantiated = true;

    [SerializeField] GameObject[] straightGround;
    [SerializeField] GameObject[] slopeGroundUpward;
    [SerializeField] GameObject[] slopeGroundDownward;
    [SerializeField] GameObject[] groundObstacle;

    private Vector2 objectPosition;

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

        InstantiateObject(straightGround[0], objColliderWidth, objSpriteRenderer, newPosition, numberOfTimes);
    }


    public void InstantiateSlope(int numberOfTimes, bool isUpward)
    {
        GameObject obj = null;

        switch (isUpward)
        {
            case true:
                obj = slopeGroundUpward[0];
                break;
            case false:
                obj = slopeGroundDownward[0];
                break;
        }

        float objColliderWidth = obj.GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();

        float objAngle = Mathf.Pow(obj.transform.eulerAngles.z, 2)/(obj.transform.eulerAngles.z * 180 / Mathf.PI);

        float distanceX = Mathf.Cos(objAngle) * objColliderWidth;
        float distanceY = Mathf.Sin(objAngle) * objColliderWidth;

        Vector2 newPosition = new Vector2(distanceX,distanceY);

        InstantiateObject(obj, objColliderWidth, objSpriteRenderer, newPosition, numberOfTimes);
    }

    /*public void InstantiateSlopeDownward(int numberOfTimes)
    {
        float objColliderWidth = slopeGroundDownward[0].GetComponent<BoxCollider2D>().size.x;
        SpriteRenderer objSpriteRenderer = slopeGroundDownward[0].GetComponentInChildren<SpriteRenderer>();

        float objAngle = Mathf.Pow(slopeGroundDownward[0].transform.eulerAngles.z, 2) / (slopeGroundDownward[0].transform.eulerAngles.z * 180 / Mathf.PI);


        float distanceX = Mathf.Cos(objAngle) * objColliderWidth;
        float distanceY = Mathf.Sin(objAngle) * objColliderWidth;

        Vector2 newPosition = new Vector2(distanceX, distanceY);

        InstantiateObject(slopeGroundDownward[0], objSpriteRenderer, newPosition, numberOfTimes);
    }*/

    private void InstantiateObject(GameObject obj, float objColliderWidth, SpriteRenderer objSpriteRenderer, Vector2 newPosition, int numberOfTimes)
    {
        for (int i = 0; i < numberOfTimes; i++) // Para cada iteração do loop, um prefab clone será instanciado, setado para falso e adicionado à lista
        {
            //TODO: colocar Random do index do array de objetos retos para randomizar os chaozinho quando tivermos mais assets
            Instantiate(obj, objectPosition, obj.transform.rotation);

            // Instanciador de obstaculos
            if (SortObstacleInstantiation(obstacleGenerationPercentage) && obj.transform.eulerAngles.z == 0)
            {
                InstantiateObstacle(groundObstacle[0], obj, objColliderWidth);
            }

            objSpriteRenderer.sortingOrder--;

            objectPosition += newPosition;
        }
    }

    private void InstantiateObstacle(GameObject obstacle, GameObject ground, float groundColliderWidth)
    {
        if(obsHasBeenInstantiated)
        {
            obsHasBeenInstantiated = false;
            return;
        }

        float posPercentage = Random.Range(0.3f, 0.7f);

        GameObject obs = obstacle;
        Vector2 obsPosition = Vector2.zero;

        if (ground.transform.eulerAngles.z != 0) { // Checa se o chão é subida ou não
            obsPosition += objectPosition + new Vector2(groundColliderWidth * posPercentage, obs.transform.position.y+0.5f); ;
        }
        else
        {
            obsPosition += objectPosition + new Vector2(groundColliderWidth * posPercentage, obs.transform.position.y);
        }

        Instantiate(obs, obsPosition, ground.transform.rotation);
        obsHasBeenInstantiated = true;
    }

    private bool SortObstacleInstantiation(int factor)
    {
        int num = Random.Range(1, 101);
        if(num <= factor) { return true; }
        else { return false; }
    }
}
