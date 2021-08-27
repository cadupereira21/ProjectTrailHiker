using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct LevelNames
{
    public const string TestLevel = "TestScene";
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
            case LevelNames.TestLevel:
                CreateTestLevel();
                break;
            default:
                Debug.Log("Level '" + currentLevel + "' não foi encontrado!");
                break;
        }
    }

    private void CreateTestLevel()
    {
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlope(5, true);
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlope(5, false);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlope(5, true);
        instantiator.InstantiateStraightGround(1);
        instantiator.InstantiateSlope(6, true);
        instantiator.InstantiateSlope(3, false);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlope(2, false);
        instantiator.InstantiateStraightGround(1);
        instantiator.InstantiateSlope(1, true);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlope(5, false);
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlope(4, false);
        instantiator.InstantiateStraightGround(20);
        instantiator.InstantiateSlope(10, true);
        instantiator.InstantiateStraightGround(3);
        instantiator.InstantiateSlope(15, false);
        instantiator.InstantiateStraightGround(3);
        instantiator.InstantiateSlope(6, true);
        instantiator.InstantiateStraightGround(10);
    }
}
