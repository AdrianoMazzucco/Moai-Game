using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Startup : MonoBehaviour
{
    public SceneReference UIScene;
    public bool ShowMainMenu = true;

    void Start()
    {
        Input.multiTouchEnabled = false;

        Scene scene = SceneManager.GetSceneByPath(UIScene);
        if (scene.isLoaded == false)
        {
            StartCoroutine(BootSequence());
        }
        else if (scene.buildIndex == -1)
        {
            Debug.Assert(false, $"Scene no found {UIScene}");
        }
        else
        {
            StartCoroutine(IgnoreBootSequence());
        }
    }

    IEnumerator IgnoreBootSequence()
    {
        yield return new WaitForSeconds(1);
        SceneLoadedCallback(null);
    }

    IEnumerator BootSequence()
    {
        yield return new WaitForSeconds(1);
        SceneLoader.Instance.OnSceneLoadedEvent += SceneLoadedCallback;
        SceneLoader.Instance.LoadScene(UIScene, false);
    }

    void SceneLoadedCallback(List<string> scenesLoaded)
    {
        SceneLoader.Instance.OnSceneLoadedEvent -= SceneLoadedCallback;
        MenuManager.Instance.HideMenu(MenuManager.Instance.LoadingScreenClassifier);

#if UNITY_EDITOR
        if (ShowMainMenu)
        {
            MenuManager.Instance.ShowMenu(MenuManager.Instance.MainMenuClassifier);
        }
        else
        {
            MenuManager.Instance.HideMenu(MenuManager.Instance.MainMenuClassifier);
        }
    }
#else
            MenuManager.Instance.ShowMenu(MainMenuClassifier);
#endif
}
