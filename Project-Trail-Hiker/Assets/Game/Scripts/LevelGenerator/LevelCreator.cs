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
    }
}
