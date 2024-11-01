using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public MenuClassifier MainMenuClassifier;
    public MenuClassifier LoadingScreenClassifier;

    private Dictionary<string, Menu> menuList = new Dictionary<string, Menu>();

    public T GetMenu<T>(MenuClassifier menuClassifier) where T : Menu
    {
        Menu menu;
        if (menuList.TryGetValue(menuClassifier.menuName, out menu))
        {
            return (T)menu;
        }
        return null;
    }

    public void AddMenu(Menu menu)
    {
        if (menuList.ContainsKey(menu.menuClassifier.menuName))
        {
            Debug.Assert(false, $"{menu.name} menu is already registered sing {menu.menuClassifier.name}");
            return;
        }
        menuList.Add(menu.menuClassifier.menuName, menu);
    }

    public void RemoveMenu(Menu menu)
    {
        menuList.Remove(menu.menuClassifier.menuName);
    }

    public void ShowMenu(MenuClassifier classifier, string options = "")
    {
        Menu menu;
        if (menuList.TryGetValue(classifier.menuName, out menu))
        {
            menu.OnShowMenu(options);
        }
    }

    public void HideMenu(MenuClassifier classifier, string options = "")
    {
        Menu menu;
        if (menuList.TryGetValue(classifier.menuName, out menu))
        {
            menu.OnHideMenu(options);
        }
    }
}
