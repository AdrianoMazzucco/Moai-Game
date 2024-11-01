using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    public SceneReference LevelToLoad;
    public MenuClassifier hudClassifier;

    public float volume = 1.0f;

    public void OnStartGame()
    {
        SceneLoader.Instance.LoadScene(LevelToLoad);
        MenuManager.Instance.HideMenu(menuClassifier);

        MenuManager.Instance.ShowMenu(hudClassifier);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
