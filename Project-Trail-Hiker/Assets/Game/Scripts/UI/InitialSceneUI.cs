using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialSceneUI : MonoBehaviour
{
    [SerializeReference] GameObject[] ui;

    public void QuitButtonPressed()
    {
        Application.Quit();
        Debug.Log("Saindo");
    }

    public void FaseButtonClicked(int fase)
    {
        SceneManager.LoadScene("Level_" + fase);
    }

    public void PlayButtonPressed()
    {
        ui[0].SetActive(false);
        ui[1].SetActive(true);
    }

    public void BackButtonPressed()
    {
        ui[1].SetActive(false);
        ui[2].SetActive(false);
        ui[0].SetActive(true);
    }

    public void StoreButtonPressed()
    {
        ui[0].SetActive(false);
        ui[2].SetActive(true);
    }
}
