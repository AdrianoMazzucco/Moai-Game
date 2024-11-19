using UnityEngine;

public class TriggerCanvasFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // Assign the CanvasGroup in the inspector
    [SerializeField] private float fadeDuration = 0.2f; // Time to fade in/out
    [SerializeField] private string playerTag = "Player"; // Tag for the player character

    private Coroutine fadeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.playerGameObject) 
        {
            // Start fading in
            StartFade(1);

        } 
       /* 
        if (other.CompareTag(playerTag))
        {
            // Start fading in
            StartFade(1);
        }
       */
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject == GameManager.Instance.playerGameObject)
        {
            // Start fading out
            StartFade(0);
        }
        /*
        if (other.CompareTag(playerTag))
        {
            // Start fading out
            StartFade(0);
        }
        */
    }

    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeCanvasGroup(targetAlpha));
    }

    private System.Collections.IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure the final value is set
    }
}
