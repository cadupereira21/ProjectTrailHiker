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
        instantiator.InstantiateSlopeUpward(5);
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlopeDownward(5);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlopeUpward(5);
        instantiator.InstantiateStraightGround(1);
        instantiator.InstantiateSlopeUpward(6);
        instantiator.InstantiateSlopeDownward(3);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlopeDownward(2);
        instantiator.InstantiateStraightGround(1);
        instantiator.InstantiateSlopeUpward(1);
        instantiator.InstantiateStraightGround(10);
        instantiator.InstantiateSlopeDownward(5);
        instantiator.InstantiateStraightGround(5);
        instantiator.InstantiateSlopeDownward(4);
        instantiator.InstantiateStraightGround(20);
        instantiator.InstantiateSlopeUpward(10);
        instantiator.InstantiateStraightGround(3);
        instantiator.InstantiateSlopeDownward(15);
        instantiator.InstantiateStraightGround(3);
        instantiator.InstantiateSlopeUpward(6);
        instantiator.InstantiateStraightGround(10);
    }
}
