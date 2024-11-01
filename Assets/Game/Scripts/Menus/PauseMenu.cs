using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : Menu
{
    public SceneReference LevelToUnLoad;

    public MenuClassifier hudClassifier;
    public MenuClassifier mainMenuClassifier;

    public void ClosePauseMenu()
    {
        MenuManager.Instance.HideMenu(menuClassifier);
        MenuManager.Instance.ShowMenu(hudClassifier);
    }

    public void QuitGame()
    {
        MenuManager.Instance.HideMenu(menuClassifier);
        MenuManager.Instance.ShowMenu(mainMenuClassifier);
        SceneLoader.Instance.UnloadScene(LevelToUnLoad);
    }
}
