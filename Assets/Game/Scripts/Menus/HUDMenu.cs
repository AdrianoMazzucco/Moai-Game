using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDMenu : Menu
{
    [SerializeField] MenuClassifier pauseClassifier;

    public void OpenPauseMenu()
    {
        MenuManager.Instance.HideMenu(menuClassifier);
        MenuManager.Instance.ShowMenu(pauseClassifier);
    }
}
