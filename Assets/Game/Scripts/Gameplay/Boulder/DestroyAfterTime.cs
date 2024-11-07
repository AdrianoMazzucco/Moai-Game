using UnityEngine;
using System.Collections.Generic; // To use List

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private List<GameObject> childrenToEnable; // Assign multiple children in the Inspector

    private void Start()
    {
        // Start the destruction countdown
        Invoke(nameof(DestroyObject), destroyDelay);
    }

    private void DestroyObject()
    {
        // Perform any action before the object is destroyed
        PerformPreDestroyAction();

        // Destroy the GameObject
        Destroy(gameObject);
    }

    private void PerformPreDestroyAction()
    {
        // Check if the list of children is not empty
        if (childrenToEnable != null && childrenToEnable.Count > 0)
        {
            foreach (GameObject child in childrenToEnable)
            {
                // Detach each child so it won't be destroyed with the parent
                if (child != null)
                {
                    child.transform.parent = null;
                    child.SetActive(true); // Enable the child GameObject
                    Debug.Log($"{child.name} enabled and detached before parent destruction.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No children objects assigned in the Inspector!");
        }
    }
}
