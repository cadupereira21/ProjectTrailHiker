using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct LevelNames
{
    public const string Fase1 = "Fase_1";
}

public class LevelCreator : MonoBehaviour
{
    Instantiator instantiator;
    private string currentLevel;

    private void Awake()
    {
        currentLevel = SceneManager.GetActiveScene().name;
        instantiator = GetComponent<Instantiator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (currentLevel)
        {
            case LevelNames.Fase1:
                CreateTestLevel();
                break;
            default:
                Debug.Log("Level '" + currentLevel + "' não foi encontrado!");
                break;
        }
    }

    private void CreateTestLevel()
    {
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlope(2, true);
        instantiator.InstantiateStraightGround(3);
        instantiator.InstantiateSlope(2, false);
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlope(2, true);
        instantiator.InstantiateStraightGround(2);
        instantiator.InstantiateSlope(1, true);
        instantiator.InstantiateStraightGround(1);
        instantiator.InstantiateSlope(1, false);
        instantiator.InstantiateStraightGround(5);
    }
}
