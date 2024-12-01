using UnityEngine;

public class ToggleSunglasses : MonoBehaviour
{
    [SerializeField] private GameObject sunglasses;
    [HideInInspector] public bool currentlyActive = false;

    public void Toggle(bool _boolean)
    {
        sunglasses.SetActive(_boolean);
        currentlyActive = _boolean;
    }
}
