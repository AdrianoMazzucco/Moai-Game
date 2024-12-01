using UnityEngine;

public class SetCanvasGroupAlpha : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get the CanvasGroup component attached to the GameObject
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            // Set the alpha value to 0
            canvasGroup.alpha = 0;
        }
        else
        {
            Debug.LogWarning("No CanvasGroup component found on this GameObject.");
        }
    }
}
